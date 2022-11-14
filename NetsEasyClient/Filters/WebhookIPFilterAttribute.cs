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
using SolidNetsEasyClient.Encryption;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.Filters;

/// <summary>
/// An IP filter which first checks the blacklist and then lastly checks if the request comes from the configured Nets IP range. If the request is on a blacklist or not found in any list the http request will be denied with a status 403 forbidden response.
/// </summary>
/// <remarks>
/// Remember to add the authorization middleware to the pipeline. If there are calls to app.UseRouting() and app.UseEndpoints(...), the call to app.UseAuthorization() must go between them.
/// </remarks>
public sealed class WebhookIPFilterAttribute : ActionFilterAttribute, IAuthorizationFilter
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
    /// Verify the authorization header using the in-built encryption and key
    /// </summary>
    /// <remarks>
    /// The action method must have a parameter called 'orderId' to decrypt and verify the signature.
    /// Furthermore the key must be defined in the configuration <see cref="PlatformPaymentOptions.WebhookAuthorizationKey"/>
    /// </remarks>
    public bool VerifyAuthorization { get; set; } = true;

    /// <summary>
    /// Check the client IP against the blacklist and known Nets Easy endpoint IPs
    /// </summary>
    /// <param name="context">The context</param>
    public void OnAuthorization(AuthorizationFilterContext context)
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
            logger.LogWarning("Cannot retrieve the remote IP of the client. Denying request for webhook {@HttpContext}", context);
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
            logger.LogWarning("Webhook request blacklisted {@IP} in {@Blacklist}", remoteIp, ipBlacklist);
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            return;
        }

        deny = ipRangeBlacklist
            .Select(x => IPAddressRange.Parse(x))
            .Any(x => x.Contains(remoteIp));
        if (deny)
        {
            logger.LogWarning("Webhook request blacklisted {@IP} in {@Blacklist}", remoteIp, ipRangeBlacklist);
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            return;
        }

        var allowed = ipWhitelist.Select(w => IPAddressRange.Parse(w)).Any(x => x.Contains(remoteIp));
        if (!allowed)
        {
            logger.LogWarning("Webhook request IP {@IP} not specified as Nets Easy endpoint", remoteIp);
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
        }
    }

    /// <summary>
    /// Validate the request and verify the authorization signature. If it is a Nets Easy webhook callback
    /// </summary>
    /// <param name="context">The action executing context</param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!VerifyAuthorization)
        {
            // Continue
            return;
        }

        var logger = GetLogger(context.HttpContext.RequestServices);
        if (!HttpMethods.IsPost(context.HttpContext.Request.Method))
        {
            logger.LogWarning("Webhook request is not a POST {@Request}", context.HttpContext.Request);

            // 400 user error
            context.Result = new BadRequestResult();
            return;
        }

        // Must have orderId!
        var hasOrderId = context.ActionArguments.TryGetValue("orderId", out var orderObject);
        if (!hasOrderId || orderObject is not string)
        {
            logger.LogWarning("Webhook must have an order id to validate the request {@Context}", context);
            context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            return;
        }

        var orderId = (string)orderObject;

        // Must have key
        var options = GetOptions(context.HttpContext.RequestServices);
        var key = options?.Value.WebhookAuthorizationKey;
        if (key is null)
        {
            logger.LogWarning("Webhook must have a signing key defined in the options startup or in configuration settings. Currently found: {@Options}", options);
            context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            return;
        }

        var isValid = context.HttpContext.Request.ValidateOrderReference(orderId, key);
        if (!isValid)
        {
            logger.LogWarning("Webhook request does not have a valid authorization {@Header} in the {@Context}", context.HttpContext.Request.Headers, context);
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
        }
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
