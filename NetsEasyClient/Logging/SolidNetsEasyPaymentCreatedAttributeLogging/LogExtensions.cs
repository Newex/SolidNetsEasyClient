using System;
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
        Message = "Missing event payload argument in the action method",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorMissingEventPayloadArgument(this ILogger logger);

    /// <summary>
    /// Error must only have 1 event payload argument
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="ex">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Must only have 1 payload argument",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorMoreThanOneEventPayloadArgument(this ILogger logger, InvalidOperationException ex);

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

    /// <summary>
    /// Error missing webhook encryption option configuration
    /// </summary>
    /// <param name="logger">The logger</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Missing,
        Level = LogLevel.Error,
        Message = "Missing webhook encryption option configuration",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorMissingEncryptionConfiguration(this ILogger logger);
}
