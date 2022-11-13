using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NetTools;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.Filters;

/// <summary>
/// Either whitelist or blacklist IPs that webhook should originate from
/// </summary>
public sealed class WebhookIPFilterAttribute : ActionFilterAttribute
{
    /// <summary>
    /// Override the blacklist defined in the configuration
    /// </summary>
    public string Blacklist { get; set; } = string.Empty;

    /// <inheritdoc />
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        // Load settings
        var logger = context.HttpContext.RequestServices.GetService<ILogger<WebhookIPFilterAttribute>>() ?? NullLogger<WebhookIPFilterAttribute>.Instance;
        var options = context.HttpContext.RequestServices.GetService<IOptions<PlatformPaymentOptions>>();
        var ipWhitelist = options?.Value.NetsIPWebhookEndpoints?.Split(";") ?? new string[] { NetsEndpoints.WebhookIPs.LiveIPRange, NetsEndpoints.WebhookIPs.TestIPRange };
        var ipBlacklist = options?.Value.NetsIPWebhookEndpoints?.Split(";") ?? Blacklist.Split(";");

        var remoteIp = context.HttpContext.Connection.RemoteIpAddress;
        logger.LogTrace("Remote IP address: {@IP}", remoteIp);

        if (remoteIp is null)
        {
            logger.LogWarning("Cannot retrieve the remote IP of the client. Denying request for {@HttpContext}", context);
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            return;
        }

        // var combinedWhite = new string[ipWhitelist.Length + Whitelist.Length];
        // ipWhitelist.CopyTo(combinedWhite, 0);
        // Whitelist.CopyTo(combinedWhite, ipWhitelist.Length);
        var safelist = ipWhitelist.Select(w => IPAddressRange.Parse(w));

        // var combinedBlack = new string[ipBlacklist.Length + Blacklist.Length];
        // ipBlacklist.CopyTo(combinedBlack, 0);
        // Blacklist.CopyTo(combinedBlack, ipBlacklist.Length);
        var blacklist = ipBlacklist.Select(b => IPAddressRange.Parse(b));

        if (remoteIp.IsIPv4MappedToIPv6)
        {
            remoteIp = remoteIp.MapToIPv4();
        }

        foreach (var deny in blacklist)
        {
            if (deny.Contains(remoteIp))
            {
                logger.LogWarning("Blacklisted {@IP} in {@BlacklistRange}", remoteIp, deny);
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                return;
            }
        }

        var whitelisted = false;
        foreach (var allow in safelist)
        {
            if (allow.Contains(remoteIp))
            {
                whitelisted = true;
                break;
            }
        }

        if (!whitelisted)
        {
            logger.LogWarning("IP {@IP} not specified as Nets Easy endpoint", remoteIp);
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            return;
        }

        base.OnActionExecuted(context);
    }
}
