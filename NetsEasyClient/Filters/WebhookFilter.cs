using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetTools;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Logging.SolidNetsEasyIPFilterAttributeLogging;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.Filters;

/// <summary>
/// Webhook filter
/// </summary>
public class WebhookFilter : IEndpointFilter
{
    private readonly ILogger logger;
    private readonly NetsEasyOptions options;
    private readonly List<IPAddressRange> allowRanges = [];
    private readonly List<IPAddress> allowSingleIPs = [];
    private readonly List<IPAddressRange> denyRanges = [];
    private readonly List<IPAddress> denySingleIPs = [];

    /// <param name="loggerFactory">The logger factory</param>
    /// <param name="options">The nets easy options</param>
    public WebhookFilter(ILoggerFactory loggerFactory,
                         IOptions<NetsEasyOptions> options)
    {
        logger = loggerFactory.CreateLogger<WebhookFilter>();
        this.options = options.Value;

        var allowed = options.Value.WhitelistIPsForWebhook?.Split(';', System.StringSplitOptions.RemoveEmptyEntries);
        if (allowed?.Length > 0)
        {
            foreach (var ip in allowed)
            {
                if (IPAddressRange.TryParse(ip, out var ipRange))
                {
                    allowRanges.Add(ipRange);
                }
                else if (IPAddress.TryParse(ip, out var single))
                {
                    allowSingleIPs.Add(single);
                }
            }
        }
        else
        {
            allowRanges.Add(IPAddressRange.Parse(NetsEndpoints.WebhookIPs.TestIPRange));
            allowRanges.Add(IPAddressRange.Parse(NetsEndpoints.WebhookIPs.LiveIPRange));
        }

        var denied = options.Value.BlacklistIPsForWebhook?.Split(';', System.StringSplitOptions.RemoveEmptyEntries);
        if (denied?.Length > 0)
        {
            foreach (var ip in denied)
            {
                if (IPAddressRange.TryParse(ip, out var ipRange))
                {
                    denyRanges.Add(ipRange);
                }
                else if (IPAddress.TryParse(ip, out var single))
                {
                    denySingleIPs.Add(single);
                }
            }
        }
    }

    /// <inheritdoc />
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // Remote IP
        var remoteIP = context.HttpContext.Connection.RemoteIpAddress;
        logger.TraceRemoteIP(remoteIP);

        if (remoteIP is null)
        {
            return Results.Unauthorized();
        }

        var whiteListed = allowRanges.Any(x => x.Contains(remoteIP));
        whiteListed = whiteListed || allowSingleIPs.Any(x => x.Equals(remoteIP));

        if (whiteListed)
        {
            return await next(context);
        }

        var blackListed = denyRanges.Any(x => x.Contains(remoteIP));
        blackListed = blackListed || denySingleIPs.Any(x => x.Equals(remoteIP));

        if (blackListed)
        {
            return Results.Unauthorized();
        }

        // Allow by default?!
        return options.DefaultDenyWebhook
            ? Results.Unauthorized()
            : await next(context);
    }
}
