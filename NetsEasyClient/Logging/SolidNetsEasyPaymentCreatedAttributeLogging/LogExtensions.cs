using Microsoft.Extensions.Logging;

namespace SolidNetsEasyClient.Logging.SolidNetsEasyPaymentCreatedAttributeLogging;

/// <summary>
/// Log extensions for the <see cref="SolidNetsEasyPaymentCreatedAttributeLogging"/>
/// </summary>
public static partial class LogExtensions
{
    /// <summary>
    /// Info request is not a POST or validation has been disabled
    /// </summary>
    /// <param name="logger">The logger</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Information,
        Message = "Webhook request is not a POST; or validation of requests has been disabled",
        SkipEnabledCheck = true
    )]
    public static partial void InfoNotPOSTRequest(this ILogger logger);

    /// <summary>
    /// Error missing authorization header
    /// </summary>
    /// <param name="logger">The logger</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Forbidden,
        Level = LogLevel.Error,
        Message = "Missing authorization header",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorMissingAuthorizationHeader(this ILogger logger);

    /// <summary>
    /// Error missing argument
    /// </summary>
    /// <param name="logger">The logger</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Missing,
        Level = LogLevel.Error,
        Message = "Missing payment argument in the action method",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorMissingPaymentArgument(this ILogger logger);

    /// <summary>
    /// Error missing nonce to authorize
    /// </summary>
    /// <param name="logger">The logger</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Missing,
        Level = LogLevel.Error,
        Message = "Missing nonce and/or complement value",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorMissingNonce(this ILogger logger);

    /// <summary>
    /// Error invalid authorization header and nonce and/or complement
    /// </summary>
    /// <param name="logger">The logger</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Forbidden,
        Level = LogLevel.Error,
        Message = "Invalid authorization headers",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidAuthorizationHeader(this ILogger logger);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Missing,
        Level = LogLevel.Error,
        Message = "Missing webhook encryption option configuration",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorMissingEncryptionConfiguration(this ILogger logger);
}
