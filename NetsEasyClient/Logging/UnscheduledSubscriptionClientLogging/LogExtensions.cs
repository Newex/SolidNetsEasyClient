using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SolidNetsEasyClient.Clients;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;
using SolidNetsEasyClient.Models.DTOs.Requests.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;

namespace SolidNetsEasyClient.Logging.UnscheduledSubscriptionClientLogging;

/// <summary>
/// Log extension methods for <see cref="UnscheduledSubscriptionClient"/>
/// </summary>
public static partial class LogExtensions
{
    /// <summary>
    /// Error invalid external reference
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="externalReference">The external reference</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Invalid {ExternalReference}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidExternalReference(this ILogger logger, string externalReference);

    /// <summary>
    /// Trace retrieve by external
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="externalReference">The external reference</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Retrieving unscheduled subscription with {ExternalReference}"
    )]
    public static partial void TraceRetrieveByExternal(this ILogger logger, string externalReference);

    /// <summary>
    /// Trace response message content
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="response">The response content</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Content is: {Response}"
    )]
    public static partial void TraceResponse(this ILogger logger, string response);

    /// <summary>
    /// Error deserializing retrieval by external reference
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="content">The response content</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Could not deserialize response from http client with {Content} to UnscheduledSubscriptionDetails",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorDeserializeRetrieveByExternal(this ILogger logger, string content);

    /// <summary>
    /// Info retrieve by external
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="unscheduledSubscription">The unscheduled subscription</param>
    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Retrieved unscheduled subscription {UnscheduledSubscription}",
        SkipEnabledCheck = true
    )]
    public static partial void InfoRetrievedByExternal(this ILogger logger, UnscheduledSubscriptionDetails unscheduledSubscription);

    /// <summary>
    /// Exception retrieving by external reference
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="externalReference">The external reference</param>
    /// <param name="ex">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "An exception occurred trying to retrieve unscheduled subscription with {ExternalReference}",
        SkipEnabledCheck = true
    )]
    public static partial void ExceptionRetrieveByExternal(this ILogger logger, string externalReference, Exception ex);

    /// <summary>
    /// Error invalid subscription ID or Order items
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="unscheduledSubscriptionID">The unscheduled subscription ID</param>
    /// <param name="order">The order</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Unscheduled subscription must contain an {UnscheduledSubscriptionId} and the {Order} must have at least 1 item! And the max webhooks are 32",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidCharge(this ILogger logger, Guid unscheduledSubscriptionID, Order order);

    /// <summary>
    /// Trace unscheduled subscription ID and order
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="unscheduledSubscriptionID">The unscheduled subscription ID</param>
    /// <param name="order">The order</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Charging unscheduled subscription {UnscheduledSubscriptionId}, with {Order}"
    )]
    public static partial void TraceCharge(this ILogger logger, Guid unscheduledSubscriptionID, Order order);

    /// <summary>
    /// Error deserializing charge
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="content">The response content</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not deserialize response {Content} from http client to UnscheduledSubscriptionChargeResult",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorDeserializeCharge(this ILogger logger, string content);

    /// <summary>
    /// Info charge result
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="unscheduledSubscription">The unscheduled subscription</param>
    /// <param name="result">The result</param>
    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Charged {UnscheduledSubscription} with result {Result}",
        SkipEnabledCheck = true
    )]
    public static partial void InfoCharge(this ILogger logger, UnscheduledSubscriptionCharge unscheduledSubscription, UnscheduledSubscriptionChargeResult result);

    /// <summary>
    /// Exception when charging
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="unscheduledSubscriptionID">The unscheduled subscription ID</param>
    /// <param name="ex">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "An exception occurred trying to charge unscheduled subscription {UnscheduledSubscriptionId}",
        SkipEnabledCheck = true
    )]
    public static partial void ExceptionCharge(this ILogger logger, Guid unscheduledSubscriptionID, Exception ex);

    /// <summary>
    /// Error invalid bulk
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="bulk">The bulk of subscriptions</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Invalid {Bulk}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidBulk(this ILogger logger, BulkUnscheduledSubscriptionCharge bulk);

    /// <summary>
    /// Trace bulk charge
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="bulk">The bulk</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Charging {Bulk} unscheduled subscription"
    )]
    public static partial void TraceBulkCharge(this ILogger logger, BulkUnscheduledSubscriptionCharge bulk);

    /// <summary>
    /// Error deserializing bulk charge response
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="content">The response content</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not deserialize response {Content} from http client to BulkId",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorDeserializeBulkCharge(this ILogger logger, string content);

    /// <summary>
    /// Info bulk charged
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="bulk">The bulk</param>
    /// <param name="bulkId">The bulk ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Charged {Bulk}, with {BulkId}",
        SkipEnabledCheck = true
    )]
    public static partial void InfoBulkCharged(this ILogger logger, BulkUnscheduledSubscriptionCharge bulk, BulkId bulkId);

    /// <summary>
    /// Exception when charging bulk
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="bulk">The bulk</param>
    /// <param name="ex">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "An exception occurred trying to charge {Bulk} unscheduled subscriptions",
        SkipEnabledCheck = true
    )]
    public static partial void ExceptionBulkCharge(this ILogger logger, BulkUnscheduledSubscriptionCharge bulk, Exception ex);

    /// <summary>
    /// Error invalid bulk charge or external reference or notifications
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="bulk">The bulk</param>
    /// <param name="externalBulkChargeID">The external bulk charge ID</param>
    /// <param name="notifications">The webhook notifications</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Invalid {Bulk} or missing external {ExternalBulkChargeId} or notifications {Notifications}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidBulkCharge(this ILogger logger, IList<ChargeUnscheduledSubscription> bulk, string? externalBulkChargeID, Notification? notifications);

    /// <summary>
    /// Trace bulk charge
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="bulk">The bulk</param>
    /// <param name="externalBulkChargeID">The external bulk charge ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Charging {Bulk} unscheduled subscription {ExternalBulkChargeId}"
    )]
    public static partial void TraceBulkCharge(this ILogger logger, IList<ChargeUnscheduledSubscription> bulk, string externalBulkChargeID);

    /// <summary>
    /// Exception when charging bulk
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="bulk">The bulk</param>
    /// <param name="externalBulkChargeID">The external bulk charge ID</param>
    /// <param name="ex">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "An exception occurred trying to charge {Bulk} unscheduled subscription {ExternalBulkChargeId}",
        SkipEnabledCheck = true
    )]
    public static partial void ExceptionBulkCharge(this ILogger logger, IList<ChargeUnscheduledSubscription> bulk, string externalBulkChargeID, Exception ex);

    /// <summary>
    /// Trace retrieval
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="bulkID">The bulk ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Retrieving bulk {BulkId}"
    )]
    public static partial void TraceRetrieveBulk(this ILogger logger, Guid bulkID);

    /// <summary>
    /// Error deserializing retrieval response
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="response">The response</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not deserialize {Response} to paginated unscheduled subscription process",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorDeserializeRetrieveBulk(this ILogger logger, string response);

    /// <summary>
    /// Info retrieved bulk
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="page">The paginated set of bulk</param>
    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Retrieved bulk unscheduled subscriptions: {Page}",
        SkipEnabledCheck = true
    )]
    public static partial void InfoRetrieveBulk(this ILogger logger, PageResult<UnscheduledSubscriptionProcessStatus> page);

    /// <summary>
    /// Exception retrieving bulk
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="bulkID">The bulk ID</param>
    /// <param name="ex">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "An exception occurred trying to retrieve {BulkId}",
        SkipEnabledCheck = true
    )]
    public static partial void ExceptionRetrieveBulk(this ILogger logger, Guid bulkID, Exception ex);

    /// <summary>
    /// Trace retrieve by skip and take
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="bulkID">The bulk ID</param>
    /// <param name="skip">The skip amount</param>
    /// <param name="take">The take amount</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Retrieving bulk {BulkId}, {Skip} and {Take}"
    )]
    public static partial void TraceRetrieveBulkSkipTake(this ILogger logger, Guid bulkID, int skip, int take);

    /// <summary>
    /// Trace retrieve by page size and page number
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
    public static partial void TraceRetrieveBulkSizeNumber(this ILogger logger, Guid bulkID, int pageSize, int pageNumber);

    /// <summary>
    /// Error invalid bulk or external verification ID
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="subscriptions">The subscriptions</param>
    /// <param name="externalVerificationID">The external verification ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Invalid {Subscriptions} or missing external {ExternalVerificationId}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidVerifyBulk(this ILogger logger, IList<UnscheduledSubscriptionInfo> subscriptions, string externalVerificationID);

    /// <summary>
    /// Trace verifying bulk
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="unscheduledSubscriptions">The unscheduled subscriptions</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Verifying {UnscheduledSubscriptions}"
    )]
    public static partial void TraceVerifyBulk(this ILogger logger, IList<UnscheduledSubscriptionInfo> unscheduledSubscriptions);

    /// <summary>
    /// Error deserializing verification response
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="response">The response</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not deserialize {Response} to BulkId",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorDeserializeVerifyBulk(this ILogger logger, string response);

    /// <summary>
    /// Info verifying bulk
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="bulkID">The bulk ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Retrieved bulk id: {BulkId}",
        SkipEnabledCheck = true
    )]
    public static partial void InfoVerifyBulk(this ILogger logger, BulkId bulkID);

    /// <summary>
    /// Exception verifying bulk
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="unscheduledSubscriptions">The unscheduled subscriptions</param>
    /// <param name="ex">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "An exception occurred trying to verify {UnscheduledSubscriptions}",
        SkipEnabledCheck = true
    )]
    public static partial void ExceptionVerifyBulk(this ILogger logger, IList<UnscheduledSubscriptionInfo> unscheduledSubscriptions, Exception ex);

    /// <summary>
    /// Error invalid bulk ID or parameters
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="bulkID">The bulk ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Invalid {BulkId}, or skip, take, pageNumber or pageSize parameters. Must be non-negative",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidRetrieveBulkVerification(this ILogger logger, Guid bulkID);

    /// <summary>
    /// Trace retrieval of bulk verification
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="bulkID">The bulk ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Retrieving page verifications for {BulkId}"
    )]
    public static partial void TraceRetrieveBulkVerification(this ILogger logger, Guid bulkID);

    /// <summary>
    /// Exception when retrieving bulk verification
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="bulkID">The bulk ID</param>
    /// <param name="ex">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "An exception occurred trying to retrieve bulk verifications for {BulkId}",
        SkipEnabledCheck = true
    )]
    public static partial void ExceptionRetrieveBulkVerifications(this ILogger logger, Guid bulkID, Exception ex);

    /// <summary>
    /// Error invalid bulk ID or parameters when retrieving bulk
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="bulkID">The bulk ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Invalid {BulkId} or paging parameters, must have values greater than zero",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidRetrieveBulk(this ILogger logger, Guid bulkID);
}
