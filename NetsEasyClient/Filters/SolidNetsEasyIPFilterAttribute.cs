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
    /// Override the configured blacklist of IPs separated by a semi-colon (;)
    /// </summary>
    public string? BlacklistIPs { get; set; }

    /// <summary>
    /// Override the configured Nets Easy endpoint IPs. Each IP must be separated by a semi-colon (;).
    /// </summary>
    public string? WhitelistIPs { get; set; }

    /// <summary>
    /// True if an IP MUST be in the whitelist to allow request.
    /// If false then the requestor IP is allowed even when it is not in the whitelist.
    /// </summary>
    public bool AllowOnlyWhitelistedIPs { get; set; } = true;

    /// <summary>
    /// Check the client IP against the blacklist and known Nets Easy endpoint IPs.
    /// Blacklist IPs take precedence over the whitelist.
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

        // Retrieve client IP
        var remoteIp = context.HttpContext.Connection.RemoteIpAddress;
        var clientIP = context.HttpContext.Request.Headers["X-Forwarded-For"].ToString().Split(',').FirstOrDefault();
        logger.TraceRemoteIP(remoteIp);

        if (remoteIp is null || clientIP is null)
        {
            logger.WarningNoRemoteIP(context);
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            return;
        }

        if (!string.IsNullOrWhiteSpace(clientIP))
        {
            // Note-Security: This can be spoofed by an adversary
            // Use the first element in the forwarded header
            remoteIp = IPAddress.Parse(clientIP);
        }

        if (remoteIp.IsIPv4MappedToIPv6)
        {
            remoteIp = remoteIp.MapToIPv4();
        }

        var blacklist = string.Concat(BlacklistIPs, ";", options?.Value.BlacklistIPsForWebhook);
        var denied = ContainsIP(blacklist, remoteIp);
        if (denied)
        {
            logger.WarningBlacklistedIP(remoteIp, blacklist);
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            return;
        }

        var whitelist = string.Concat(WhitelistIPs, ";", options?.Value.NetsIPWebhookEndpoints);
        var allowed = ContainsIP(whitelist, remoteIp);
        if (!allowed || !AllowOnlyWhitelistedIPs)
        {
            logger.WarningNotNetsEasyEndpoint(remoteIp, whitelist);
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
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

    private static bool ContainsIP(string listing, IPAddress ip)
    {
        var ips = listing.Split(';', StringSplitOptions.RemoveEmptyEntries);
        return ips.Any(ipOrRange =>
        {
            if (IPAddressRange.TryParse(ipOrRange, out var range))
            {
                return range.Contains(ip);
            }

            if (IPAddress.TryParse(ipOrRange, out var listedIp))
            {
                return listedIp.Equals(ip);
            }

            return false;
        });
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
