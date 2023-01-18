using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.Logging.SolidNetsEasyIPFilterAttributeLogging;

/// <summary>
/// Log extension methods for the logger
/// </summary>
public static partial class LogExtensions
{
    /// <summary>
    /// Log request to the webhook is not a POST
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="request">The http request</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Warning,
        Message = "Webhook request is not a POST {Request}",
        SkipEnabledCheck = true
    )]
    public static partial void WarningNotPOSTRequest(this ILogger logger, HttpRequest request);

    /// <summary>
    /// Trace log ip request
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="ip">The ip address</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Remote IP address: {IP}"
    )]
    public static partial void TraceRemoteIP(this ILogger logger, IPAddress? ip);

    /// <summary>
    /// Warning request does not contain remote IP
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="context">The authorization context</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Forbidden,
        Level = LogLevel.Warning,
        Message = "Cannot retrieve the remote IP of the client. Denying request for webhook {Context}",
        SkipEnabledCheck = true
    )]
    public static partial void WarningNoRemoteIP(this ILogger logger, AuthorizationFilterContext context);

    /// <summary>
    /// Warning request IP has been blacklisted
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="ip">The IP address</param>
    /// <param name="blacklist">The blacklist</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Forbidden,
        Level = LogLevel.Warning,
        Message = "Webhook request blacklisted {IP} in {Blacklist}",
        SkipEnabledCheck = true
    )]
    public static partial void WarningBlacklistedIP(this ILogger logger, IPAddress ip, string[] blacklist);

    /// <summary>
    /// Warning request is not from a Nets Easy endpoint range
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="ip">The IP address</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Forbidden,
        Level = LogLevel.Warning,
        Message = "Webhook request IP {IP} not specified as Nets Easy endpoint",
        SkipEnabledCheck = true
    )]
    public static partial void WarningNotNetsEasyEndpoint(this ILogger logger, IPAddress ip);

    /// <summary>
    /// Warning request is not from a Nets Easy endpoint range
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="context">The context</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Missing,
        Level = LogLevel.Warning,
        Message = "Webhook must have an order id to validate the request {Context}",
        SkipEnabledCheck = true
    )]
    public static partial void WarningMissingOrderID(this ILogger logger, ActionExecutingContext context);

    /// <summary>
    /// Warning missing signing key
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="options">The platform payment options</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Missing,
        Level = LogLevel.Warning,
        Message = "Webhook must have a signing key defined in the options startup or in configuration settings. Currently found: {Options}",
        SkipEnabledCheck = true
    )]
    public static partial void WarningMissingSigningKey(this ILogger logger, PlatformPaymentOptions? options);

    /// <summary>
    /// Warning headers do not have valid authorization
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="headers">The request headers</param>
    /// <param name="context">The context</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Warning,
        Message = "Webhook request does not have a valid authorization {Headers} in the {Context}",
        SkipEnabledCheck = true
    )]
    public static partial void WarningInvalidAuthorizationHeader(this ILogger logger, IHeaderDictionary headers, ActionExecutingContext context);

    /// <summary>
    /// Warning success response must be 200 OK but was something else
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="statusCode">The response status code</param>
    /// <param name="request">The http request</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Warning,
        Message = "Webhook success response must be 200 OK - instead found {StatusCode} for {Request}",
        SkipEnabledCheck = true
    )]
    public static partial void WarningWrongResponseCode(this ILogger logger, int statusCode, HttpRequest request);
}
