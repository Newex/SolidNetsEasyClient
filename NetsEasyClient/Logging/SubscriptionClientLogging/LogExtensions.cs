using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SolidNetsEasyClient.Clients;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;

namespace SolidNetsEasyClient.Logging.SubscriptionClientLogging;

/// <summary>
/// Log extensions for the <see cref="SubscriptionClient"/>
/// </summary>
public static partial class LogExtensions
{
    /// <summary>
    /// Error invalid subscription ID
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="subscriptionID">The subscription ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Invalid subscription ID {SubscriptionID}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidSubscriptionID(this ILogger logger, Guid subscriptionID);

    /// <summary>
    /// Trace subscription retrieval
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="subscriptionID">The subscription ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Retrieving subscription with {SubscriptionID}"
    )]
    public static partial void TraceSubscriptionRetrieval(this ILogger logger, Guid subscriptionID);

    /// <summary>
    /// Error deserializing response for retrieving subscription
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="subscriptionID">The subscription ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Could not deserialize response for retrieving subscription {SubscriptionID}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorSubscriptionDetailSerialization(this ILogger logger, Guid subscriptionID);

    /// <summary>
    /// Info retrieved subscriptions
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="subscription">The subscriptions</param>
    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Retrieved subscription {Subscription}",
        SkipEnabledCheck = true
    )]
    public static partial void InfoRetrievedSubscription(this ILogger logger, SubscriptionDetails subscription);

    /// <summary>
    /// Exception trying to retrieve subscriptions
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="subscriptionID">The subscription ID</param>
    /// <param name="ex">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "An exception occurred trying to retrieve subscription with {SubscriptionID}",
        SkipEnabledCheck = true
    )]
    public static partial void ExceptionRetrieveSubscription(this ILogger logger, Guid subscriptionID, Exception ex);

    /// <summary>
    /// Error invalid external reference
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="externalReference">The external reference</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Invalid external reference {ExternalReference}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidExternalReference(this ILogger logger, string externalReference);

    /// <summary>
    /// Trace retrieval of subscriptions by external reference
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="externalReference">The external reference</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Retrieving subscription with external reference {ExternalReference}"
    )]
    public static partial void TraceRetrieveByExternal(this ILogger logger, string externalReference);

    /// <summary>
    /// Error deserializing
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="externalReference">The external reference</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Could not deserialize response for retrieving subscription by external reference {ExternalReference}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorRetrieveByExternal(this ILogger logger, string externalReference);

    /// <summary>
    /// Info retrieved subscription by external reference
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="subscription">The subscription</param>
    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Retrieved subscription by external reference {Subscription}",
        SkipEnabledCheck = true
    )]
    public static partial void InfoRetrieveByExternal(this ILogger logger, SubscriptionDetails subscription);

    /// <summary>
    /// Error response
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="externalReference">The external reference</param>
    /// <param name="ex">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "An exception occurred trying to retrieve subscription with {ExternalReference}",
        SkipEnabledCheck = true
    )]
    public static partial void ExceptionRetrieveByExternal(this ILogger logger, string externalReference, Exception ex);

    /// <summary>
    /// Error invalid bulk charges
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="subscriptions">The subscriptions</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Invalid subscriptions {Subscriptions}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidBulkCharge(this ILogger logger, IList<SubscriptionCharge> subscriptions);

    /// <summary>
    /// Trace bulk charge
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="subscriptions">The subscriptions</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Bulk charge {Subscriptions}"
    )]
    public static partial void TraceBulkCharge(this ILogger logger, IList<SubscriptionCharge> subscriptions);

    /// <summary>
    /// Error deserializing
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="subscriptions">The subscriptions</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Could not deserialize response for bulk charging subscriptions {Subscriptions}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorBulkCharge(this ILogger logger, IList<SubscriptionCharge> subscriptions);

    /// <summary>
    /// Info bulk charge
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="bulkChargeID">The bulk charge ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Bulk charge id: {BulkChargeId}",
        SkipEnabledCheck = true
    )]
    public static partial void InfoBulkCharge(this ILogger logger, BulkId bulkChargeID);

    /// <summary>
    /// Exception occurred
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="subscriptions">The subscriptions</param>
    /// <param name="ex">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "An exception occurred trying to bulk charge {Subscriptions}",
        SkipEnabledCheck = true
    )]
    public static partial void ExceptionBulkCharge(this ILogger logger, IList<SubscriptionCharge> subscriptions, Exception ex);

    /// <summary>
    /// Trace retrieval by bulk ID
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="bulkId">The bulk ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Retrieving bulk {BulkId}"
    )]
    public static partial void TraceBulkId(this ILogger logger, Guid bulkId);

    /// <summary>
    /// Trace response message content
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="messageContent">The message content</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Content is: {MessageContent}"
    )]
    public static partial void TraceMessageContent(this ILogger logger, string messageContent);

    /// <summary>
    /// Error deserializing
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="message">The response</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Could not deserialize response {Message} to paginated subscriptions",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorPaginatedSubscription(this ILogger logger, string message);

    /// <summary>
    /// Info retrieved paginated subscriptions
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="paginatedSubscriptions">The paginated subscriptions</param>
    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Retrieved bulk subscriptions: {PaginatedSubscriptions}",
        SkipEnabledCheck = true
    )]
    public static partial void InfoPaginatedSubscriptions(this ILogger logger, PageResult<SubscriptionProcessStatus> paginatedSubscriptions);

    /// <summary>
    /// Exception occurred
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="bulkID">The bulk ID</param>
    /// <param name="ex">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "An exception occurred trying to bulk retrieve {BulkId}",
        SkipEnabledCheck = true
    )]
    public static partial void ExceptionPaginatedSubscriptions(this ILogger logger, Guid bulkID, Exception ex);

    /// <summary>
    /// Trace bulk pagination
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="bulkID">The bulk ID</param>
    /// <param name="skipping">The number of skip</param>
    /// <param name="taking">The number of take</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Retrieving bulk {BulkId}, {Skipping} and {Taking}"
    )]
    public static partial void TracePageSkipTake(this ILogger logger, Guid bulkID, int skipping, int taking);

    /// <summary>
    /// Trace bulk pagination
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="bulkID">The bulk ID</param>
    /// <param name="pageSize">The page size</param>
    /// <param name="pageNumber">The page number</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Retrieving bulk {BulkId}, {PageSize} and {PageNumber}"
    )]
    public static partial void TracePageSizeNumber(this ILogger logger, Guid bulkID, int pageSize, int pageNumber);

    /// <summary>
    /// Error invalid subscriptions
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="verifications">The subscription verifications</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Invalid subscription verifications {Verifications}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidSubscriptionVerifications(this ILogger logger, BulkSubscriptionVerification verifications);

    /// <summary>
    /// Trace verification
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="subscriptions">The subscriptions</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Verifying subscriptions {Subscriptions}"
    )]
    public static partial void TraceVerification(this ILogger logger, BulkSubscriptionVerification subscriptions);

    /// <summary>
    /// Error deserializing
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="response">The response</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Could not deserialize {Response} to BulkId",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorResponseToBulkID(this ILogger logger, string response);

    /// <summary>
    /// Info subscription verified
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="subscriptions">The subscriptions</param>
    /// <param name="bulkID">The bulk ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Verified {Subscriptions}, with {BulkId}",
        SkipEnabledCheck = true
    )]
    public static partial void InfoVerifications(this ILogger logger, BulkSubscriptionVerification subscriptions, BulkId bulkID);

    /// <summary>
    /// Exception verifying
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="bulk">The bulk</param>
    /// <param name="ex">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "An exception occurred trying to verify bulk subscriptions of {Bulk}",
        SkipEnabledCheck = true
    )]
    public static partial void ExceptionVerifications(this ILogger logger, BulkSubscriptionVerification bulk, Exception ex);

    /// <summary>
    /// Error invalid bulk ID or pagination
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="bulkID">The bulk ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Invalid {BulkId} or paging parameters must have values greater than or equal to zero",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidBulkIdOrPage(this ILogger logger, Guid bulkID);

    /// <summary>
    /// Trace retrieval of verification
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="bulkID">The bulk ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Retrieving verifications of {BulkId}"
    )]
    public static partial void TracePageVerifications(this ILogger logger, Guid bulkID);

    /// <summary>
    /// Exception retrieving verifications
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="bulkID">The bulk ID</param>
    /// <param name="ex">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "An exception occurred trying to retrieve verifications of {BulkId}",
        SkipEnabledCheck = true
    )]
    public static partial void ExceptionPageVerifications(this ILogger logger, Guid bulkID, Exception ex);
}
