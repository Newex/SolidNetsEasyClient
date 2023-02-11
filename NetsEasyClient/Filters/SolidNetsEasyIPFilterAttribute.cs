using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NetTools;
using SolidNetsEasyClient.Extensions;
using SolidNetsEasyClient.Logging.SolidNetsEasyIPFilterAttributeLogging;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.Filters;

/// <summary>
/// An IP filter which first checks the blacklist and then lastly checks if the request comes from the configured Nets IP range. If the request is on a blacklist or not found in any list the http request will be denied with a status 403 forbidden response.
/// To change the default behavior when no IP has been found, set <see cref="AllowOnlyWhitelistedIPs"/> property to false.
/// </summary>
/// <remarks>
/// Remember to add the authorization middleware to the pipeline. If there are calls to app.UseRouting() and app.UseEndpoints(...), the call to app.UseAuthorization() must go between them.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class SolidNetsEasyIPFilterAttribute : Attribute, IAuthorizationFilter, IEndpointFilter
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
    /// <remarks>
    /// Setting this to false will have the same effect as skipping the whitelist and only use the blacklist
    /// </remarks>
    public bool AllowOnlyWhitelistedIPs { get; set; } = true;

    /// <summary>
    /// Check the client IP against the blacklist and known Nets Easy endpoint IPs.
    /// Blacklist IPs take precedence over the whitelist.
    /// </summary>
    /// <param name="context">The context</param>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var logger = ServiceProviderExtensions.GetLogger<SolidNetsEasyIPFilterAttribute>(context.HttpContext.RequestServices);
        if (!HttpMethods.IsPost(context.HttpContext.Request.Method))
        {
            logger.WarningNotPOSTRequest(context.HttpContext.Request);

            // 400 user error
            context.Result = new BadRequestResult();
            return;
        }

        // Load settings
        var options = ServiceProviderExtensions.GetOptions<NetsEasyOptions>(context.HttpContext.RequestServices);

        // Retrieve client IP
        // Note-Security: This can be spoofed by an adversary
        // Use the first element in the forwarded header
        var remoteIp = context.HttpContext.Connection.RemoteIpAddress;
        var clientIP = context.HttpContext.Request.Headers["X-Forwarded-For"].ToString().Split(',').FirstOrDefault();
        logger.TraceRemoteIP(remoteIp);

        if (remoteIp is null || clientIP is null)
        {
            logger.ErrorNoRemoteIP(remoteIp, clientIP);
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            return;
        }

        if (!string.IsNullOrWhiteSpace(clientIP) && !IPAddress.TryParse(clientIP, out remoteIp))
        {
            logger.ErrorCannotParseProxyToIPAddress(clientIP);
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            return;
        }

        if (remoteIp.IsIPv4MappedToIPv6)
        {
            remoteIp = remoteIp.MapToIPv4();
        }

        var blacklist = string.Concat(BlacklistIPs, ";", options?.Value.BlacklistIPsForWebhook);
        var denied = ContainsIP(blacklist, remoteIp);
        if (denied)
        {
            logger.ErrorBlacklistedIP(remoteIp, blacklist);
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            return;
        }

        if (!AllowOnlyWhitelistedIPs)
        {
            // Proceed successfully
            return;
        }

        var whitelist = string.Concat(WhitelistIPs, ";", options?.Value.NetsIPWebhookEndpoints);
        var allowed = ContainsIP(whitelist, remoteIp);
        if (!allowed)
        {
            logger.ErrorNotNetsEasyEndpoint(remoteIp, whitelist);
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
        }
    }

    /// <summary>
    /// Invoke the nets endpoint IP filter
    /// </summary>
    /// <param name="context">The endpoint filter context</param>
    /// <param name="next">The next filter delegate</param>
    /// <returns>An awaitable object</returns>
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // FIXME: Copy-pasted from above and then adapted. Better to abstract into another so we don't have to maintain 2 methods in sync.
        var logger = ServiceProviderExtensions.GetLogger<SolidNetsEasyIPFilterAttribute>(context.HttpContext.RequestServices);
        if (!HttpMethods.IsPost(context.HttpContext.Request.Method))
        {
            logger.WarningNotPOSTRequest(context.HttpContext.Request);

            // 400 user error
            return TypedResults.BadRequest("Only POST requests allowed");
        }

        // Load settings
        var options = ServiceProviderExtensions.GetOptions<NetsEasyOptions>(context.HttpContext.RequestServices);

        // Retrieve client IP
        // Note-Security: This can be spoofed by an adversary
        // Use the first element in the forwarded header
        var remoteIp = context.HttpContext.Connection.RemoteIpAddress;
        var clientIP = context.HttpContext.Request.Headers["X-Forwarded-For"].ToString().Split(',').FirstOrDefault();
        logger.TraceRemoteIP(remoteIp);

        if (remoteIp is null || clientIP is null)
        {
            logger.ErrorNoRemoteIP(remoteIp, clientIP);
            return TypedResults.Forbid();
        }

        if (!string.IsNullOrWhiteSpace(clientIP) && !IPAddress.TryParse(clientIP, out remoteIp))
        {
            logger.ErrorCannotParseProxyToIPAddress(clientIP);
            return TypedResults.Forbid();
        }

        if (remoteIp.IsIPv4MappedToIPv6)
        {
            remoteIp = remoteIp.MapToIPv4();
        }

        var blacklist = string.Concat(BlacklistIPs, ";", options?.Value.BlacklistIPsForWebhook);
        var denied = ContainsIP(blacklist, remoteIp);
        if (denied)
        {
            logger.ErrorBlacklistedIP(remoteIp, blacklist);
            return TypedResults.Forbid();
        }

        if (!AllowOnlyWhitelistedIPs)
        {
            // Proceed successfully
            return await next(context);
        }

        var whitelist = string.Concat(WhitelistIPs, ";", options?.Value.NetsIPWebhookEndpoints);
        var allowed = ContainsIP(whitelist, remoteIp);
        if (!allowed)
        {
            logger.ErrorNotNetsEasyEndpoint(remoteIp, whitelist);
            return TypedResults.Forbid();
        }

        return await next(context);
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
}
