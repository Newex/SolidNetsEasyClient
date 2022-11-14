using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NetTools;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.Filters;

/// <summary>
/// An IP filter which first checks the blacklist and then lastly checks if the request comes from the configured Nets IP range. If the request is on a blacklist or not found in any list the http request will be denied with a status 403 forbidden response.
/// </summary>
public sealed class WebhookIPFilterAttribute : ActionFilterAttribute
{
    /// <summary>
    /// Override the configured blacklist of single IPs separated by a semi-colon (;)
    /// </summary>
    public string? BlacklistIPs { get; set; }

    /// <summary>
    /// Override the configured blacklist of IP ranges separated by a semi-colon (;). The ranges must be specified in the CIDR format e.g. 192.168.0.1/24
    /// </summary>
    public string? BlacklistIPRanges { get; set; }

    /// <inheritdoc />
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        // Load settings
        var logger = GetLogger(context.HttpContext.RequestServices);
        var options = GetOptions(context.HttpContext.RequestServices);
        var ipWhitelist = options?.Value.NetsIPWebhookEndpoints?.Split(";") ?? new string[] { NetsEndpoints.WebhookIPs.LiveIPRange, NetsEndpoints.WebhookIPs.TestIPRange };
        var ipBlacklist = BlacklistIPs?.Split(";") ?? options?.Value.BlacklistIPsForWebhook?.Split(";") ?? Array.Empty<string>();
        var ipRangeBlacklist = BlacklistIPRanges?.Split(";") ?? options?.Value.BlacklistIPRangesForWebhook?.Split(";") ?? Array.Empty<string>();

        var remoteIp = context.HttpContext.Connection.RemoteIpAddress;
        logger.LogTrace("Remote IP address: {@IP}", remoteIp);

        if (remoteIp is null)
        {
            logger.LogWarning("Cannot retrieve the remote IP of the client. Denying request for {@HttpContext}", context);
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            return;
        }

        if (remoteIp.IsIPv4MappedToIPv6)
        {
            remoteIp = remoteIp.MapToIPv4();
        }

        var deny = ipBlacklist.Select(b => IPAddress.Parse(b)).Any(x => x.Equals(remoteIp));
        if (deny)
        {
            logger.LogWarning("Blacklisted {@IP} in {@Blacklist}", remoteIp, ipBlacklist);
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            return;
        }

        deny = ipRangeBlacklist
            .Select(x => IPAddressRange.Parse(x))
            .Any(x => x.Contains(remoteIp));
        if (deny)
        {
            logger.LogWarning("Blacklisted {@IP} in {@Blacklist}", remoteIp, ipRangeBlacklist);
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            return;
        }

        var allowed = ipWhitelist.Select(w => IPAddressRange.Parse(w)).Any(x => x.Contains(remoteIp));
        if (!allowed)
        {
            logger.LogWarning("IP {@IP} not specified as Nets Easy endpoint", remoteIp);
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            return;
        }

        base.OnActionExecuted(context);
    }

    private static ILogger<WebhookIPFilterAttribute> GetLogger(IServiceProvider services)
    {
        // Reason for this is to circumvent extension method to make this class testable
        return (services.GetService(typeof(ILogger<WebhookIPFilterAttribute>)) as ILogger<WebhookIPFilterAttribute>) ?? NullLogger<WebhookIPFilterAttribute>.Instance;
    }

    private static IOptions<PlatformPaymentOptions>? GetOptions(IServiceProvider services)
    {
        // Reason for this is to circumvent extension method to make this class testable
        return services.GetService(typeof(IOptions<PlatformPaymentOptions>)) as IOptions<PlatformPaymentOptions>;
    }
}
