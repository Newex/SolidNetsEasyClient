using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NetTools;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Logging.SolidNetsEasyIPFilterAttributeLogging;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.Filters;

/// <summary>
/// An IP filter which first checks the blacklist and then lastly checks if the request comes from the configured Nets IP range. If the request is on a blacklist or not found in any list the http request will be denied with a status 403 forbidden response.
/// </summary>
/// <remarks>
/// Remember to add the authorization middleware to the pipeline. If there are calls to app.UseRouting() and app.UseEndpoints(...), the call to app.UseAuthorization() must go between them.
/// </remarks>
public sealed class SolidNetsEasyIPFilterAttribute : ActionFilterAttribute, IAuthorizationFilter
{
    /// <summary>
    /// Override the configured blacklist of single IPs separated by a semi-colon (;)
    /// </summary>
    public string? BlacklistIPs { get; set; }

    /// <summary>
    /// Override the configured blacklist of IP ranges separated by a semi-colon (;). The ranges must be specified in the CIDR format e.g. 192.168.0.1/24
    /// </summary>
    public string? BlacklistIPRanges { get; set; }

    /// <summary>
    /// Override the configured Nets Easy endpoint IPs. Each IP must be separated by a semi-colon (;).
    /// </summary>
    public string? WhitelistIPs { get; set; }

    /// <summary>
    /// Override the configured Nets Easy endpoints of IP ranges separated by a semi-colon (;). The ranges must be specified in the CIDR format e.g. 192.168.0.1/24
    /// </summary>
    public string? WhitelistIPRanges { get; set; }

    /// <summary>
    /// Check the client IP against the blacklist and known Nets Easy endpoint IPs
    /// </summary>
    /// <param name="context">The context</param>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var logger = GetLogger(context.HttpContext.RequestServices);
        if (!HttpMethods.IsPost(context.HttpContext.Request.Method))
        {
            logger.WarningNotPOSTRequest(context.HttpContext.Request);

            // 400 user error
            context.Result = new BadRequestResult();
            return;
        }

        // Load settings
        var options = GetOptions(context.HttpContext.RequestServices);
        var ipWhitelistRange = WhitelistIPRanges?.Split(";") ?? options?.Value.NetsIPWebhookEndpoints?.Split(";") ?? new string[] { NetsEndpoints.WebhookIPs.LiveIPRange, NetsEndpoints.WebhookIPs.TestIPRange };
        var ipBlacklist = BlacklistIPs?.Split(";") ?? options?.Value.BlacklistIPsForWebhook?.Split(";") ?? Array.Empty<string>();
        var ipRangeBlacklist = BlacklistIPRanges?.Split(";") ?? options?.Value.BlacklistIPRangesForWebhook?.Split(";") ?? Array.Empty<string>();

        var remoteIp = context.HttpContext.Connection.RemoteIpAddress;
        logger.TraceRemoteIP(remoteIp);

        if (remoteIp is null)
        {
            logger.WarningNoRemoteIP(context);
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            return;
        }

        if (remoteIp.IsIPv4MappedToIPv6)
        {
            remoteIp = remoteIp.MapToIPv4();
        }

        var deny = ipBlacklist.Select(IPAddress.Parse).Any(x => x.Equals(remoteIp));
        if (deny)
        {
            logger.WarningBlacklistedIP(remoteIp, ipBlacklist);
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            return;
        }

        deny = ipRangeBlacklist
            .Select(x => IPAddressRange.Parse(x))
            .Any(x => x.Contains(remoteIp));
        if (deny)
        {
            logger.WarningBlacklistedIP(remoteIp, ipRangeBlacklist);
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            return;
        }

        var allowedSingle = WhitelistIPs?.Split(";").Select(IPAddress.Parse).Any(x => x.Equals(remoteIp)) ?? false;
        var allowed = allowedSingle || ipWhitelistRange.Select(w => IPAddressRange.Parse(w)).Any(x => x.Contains(remoteIp));
        if (!allowed)
        {
            logger.WarningNotNetsEasyEndpoint(remoteIp);
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
        }
    }

    /// <summary>
    /// Validate the request and verify the authorization signature. If it is a Nets Easy webhook callback
    /// </summary>
    /// <param name="context">The action executing context</param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var logger = GetLogger(context.HttpContext.RequestServices);

        // Must have orderId!
        var hasOrderId = context.ActionArguments.TryGetValue("orderId", out var orderObject);
        if (!hasOrderId || orderObject is not string)
        {
            logger.WarningMissingOrderID(context);
            context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// If successfully executed the action, then change the response to 200 OK if not
    /// </summary>
    /// <param name="context">The context</param>
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        context.HttpContext.Response.OnStarting(obj =>
        {
            var ctx = (ResultExecutingContext)obj;
            var logger = GetLogger(ctx.HttpContext.RequestServices);
            var status = ctx.HttpContext.Response.StatusCode;
            if (status == StatusCodes.Status200OK)
            {
                return Task.CompletedTask;
            }

            if (status is > 200 and <= 299)
            {
                logger.WarningWrongResponseCode(status, ctx.HttpContext.Request);
                ctx.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }, context);
    }

    private static ILogger<SolidNetsEasyIPFilterAttribute> GetLogger(IServiceProvider services)
    {
        // Reason for this is to circumvent extension method to make this class testable
        return (services.GetService(typeof(ILogger<SolidNetsEasyIPFilterAttribute>)) as ILogger<SolidNetsEasyIPFilterAttribute>) ?? NullLogger<SolidNetsEasyIPFilterAttribute>.Instance;
    }

    private static IOptions<PlatformPaymentOptions>? GetOptions(IServiceProvider services)
    {
        // Reason for this is to circumvent extension method to make this class testable
        return services.GetService(typeof(IOptions<PlatformPaymentOptions>)) as IOptions<PlatformPaymentOptions>;
    }
}
