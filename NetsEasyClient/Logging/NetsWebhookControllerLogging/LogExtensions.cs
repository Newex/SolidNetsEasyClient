using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using SolidNetsEasyClient.Helpers.Controllers;

namespace SolidNetsEasyClient.Logging.NetsWebhookControllerLogging;

/// <summary>
/// <see cref="NetsWebhookController"/> logging extensions
/// </summary>
public static partial class LogExtensions
{
    /// <summary>
    /// Log webhook request headers
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="headers">The request headers</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Information,
        Message = "The header: {Headers}"
    )]
    public static partial void InfoHeader(this ILogger logger, IHeaderDictionary headers);

    /// <summary>
    /// Log webhook request authorization header value
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="authorization">The authorization header value</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Information,
        Message = "The authorization header: {Authorization}"
    )]
    public static partial void InfoAuthorizationHeader(this ILogger logger, StringValues authorization);
}
