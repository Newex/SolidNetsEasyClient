using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments.Subscriptions;

namespace SolidNetsEasyClient.Logging.PaymentClientLogging;

internal static partial class NetsPaymentClientLogging
{
    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Created payment request: {Request} Payment response: {Response}")]
    public static partial void LogPaymentRequestSuccess(this ILogger logger, PaymentRequest request, PaymentResult response);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Unexpected response"
    )
    ]
    public static partial void LogUnexpectedResponse(this ILogger logger);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Unexpected response: {Response}"
    )
    ]
    public static partial void LogUnexpectedResponse(this ILogger logger, string response);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not create payment for {Request}. Response: {Response}"
    )
    ]
    public static partial void LogPaymentRequestError(this ILogger logger, PaymentRequest request, string response);

    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Got response for {PaymentId} with {PaymentStatus}."
    )
    ]
    public static partial void LogInfoPaymentDetails(this ILogger logger, Guid paymentId, PaymentStatus paymentStatus);

    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Updated {PaymentId} with order {OrderUpdates}."
    )
    ]
    public static partial void LogInfoOrderUpdated(this ILogger logger, Guid paymentId, OrderUpdate orderUpdates);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not update {PaymentId} with {OrderUpdates}. Response reason: {Response}"
    )
    ]
    public static partial void LogErrorOrderUpdate(this ILogger logger, Guid paymentId, OrderUpdate orderUpdates, string response);

    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Updated {PaymentId} with new references, {ReferenceInformation}."
    )
    ]
    public static partial void LogInfoReferenceInformation(this ILogger logger, Guid paymentId, ReferenceInformation referenceInformation);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not update {PaymentId} with {References}. Response reason: {Response}"
    )
    ]
    public static partial void LogErrorReferenceInformation(this ILogger logger, Guid paymentId, ReferenceInformation references, string response);

    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Payment {PaymentId} terminated before checkout completed."
    )
    ]
    public static partial void LogInfoTerminatePayment(this ILogger logger, Guid paymentId);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not terminate {PaymentId} due to {Response}."
    )
    ]
    public static partial void LogErrorTerminatePayment(this ILogger logger, Guid paymentId, string response);

    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Order with {PaymentId} and {OrderDetails} has been canceled."
    )
    ]
    public static partial void LogInfoOrderCanceled(this ILogger logger, Guid paymentId, CancelOrder orderDetails);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not cancel {PaymentId} with {OrderDetails} due to {Response}."
    )
    ]
    public static partial void LogErrorOrderCanceled(this ILogger logger, Guid paymentId, CancelOrder orderDetails, string response);

    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Payment {PaymentId} charged with {Charge} and got {Result}."
    )
    ]
    public static partial void LogInfoCharge(this ILogger logger, Guid paymentId, Charge charge, ChargeResult result);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not charge {PaymentId} with {Charge}, got {Response}."
    )
    ]
    public static partial void LogErrorCharge(this ILogger logger, Guid paymentId, Charge charge, string response);

    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Charge retrieved {Charge}"
    )
    ]
    public static partial void LogInfoChargeRetrieved(this ILogger logger, ChargeDetailsInfo charge);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not retrieve charge {ChargeId} response: {Response}."
    )
    ]
    public static partial void LogErrorChargeRetrieval(this ILogger logger, Guid chargeId, string response);

    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Refunded id: {Id} containing: {Order}, refund result is: {RefundResult}"
    )
    ]
    public static partial void LogInfoRefundResult(this ILogger logger, Guid id, CancelOrder order, RefundResult refundResult);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not refund {Id} containing {Order}. Get response: {Response}"
    )
    ]
    public static partial void LogErrorRefund(this ILogger logger, Guid id, CancelOrder order, string response);

    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Retrieved refund details for {RefundId} and got {Response}"
    )
    ]
    public static partial void LogInfoRefundDetails(this ILogger logger, Guid refundId, RetrieveRefund response);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not retrieve info about {RefundId}, got {StatusCode} {Response}"
    )
    ]
    public static partial void LogErrorRetrieveRefund(this ILogger logger, Guid refundId, int statusCode, string response);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not cancel pending refund {RefundId}. Got response: {Response}"
    )
    ]
    public static partial void LogErrorCancelPendingRefund(this ILogger logger, Guid refundId, string response);

    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Canceled pending refund: {RefundId}"
    )
    ]
    public static partial void LogInfoCanceledPendingRefund(this ILogger logger, Guid refundId);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not update MyReference {MyReference} for payment id: {PaymentId}. Got response: {Response}."
    )
    ]
    public static partial void LogErrorUpdateMyReference(this ILogger logger, string myReference, Guid paymentId, string response);

    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Updated {PaymentId} with {MyReference}."
    )
    ]
    public static partial void LogInfoUpdatedMyReference(this ILogger logger, Guid paymentId, PaymentReference myReference);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not retrieve subscription {SubscriptionId}."
    )
    ]
    public static partial void LogErrorRetrieveSubscription(this ILogger logger, Guid subscriptionId);

    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Retrieved {SubscriptionId} with {Subscription}"
    )
    ]
    public static partial void LogInfoRetrieveSubscription(this ILogger logger, Guid subscriptionId, SubscriptionDetails subscription);

    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Retrieved by external reference id: {ExternalReference} and got {Subscription}"
    )
    ]
    public static partial void LogInfoRetrieveSubscription(this ILogger logger, string externalReference, SubscriptionDetails subscription);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not retrieve subscription by external reference {ExternalReference}."
    )
    ]
    public static partial void LogErrorRetrieveSubscription(this ILogger logger, string externalReference);

    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Charged subscriptions {BulkCharges} and got succes {BulkResult}"
    )
    ]
    public static partial void LogInfoBulkCharge(this ILogger logger, BulkCharge bulkCharges, BulkSubscriptionResult bulkResult);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not charge subscription in bulk {BulkCharges} got {Response}"
    )
    ]
    public static partial void LogErrorBulkCharge(this ILogger logger, BulkCharge bulkCharges, string response);

    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Retrieved bulk id {BulkId} subscription page: {Page}"
    )
    ]
    public static partial void LogInfoRetrieveBulkCharge(this ILogger logger, Guid bulkId, PageResult<SubscriptionProcessStatus> page);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not retrieve bulk charge {BulkId}. Response: {Response}"
    )
    ]
    public static partial void LogErrorRetrieveBulkCharge(this ILogger logger, Guid bulkId, string response);

    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Started verification for {Subscriptions} with {ExternalBulkVerificationId} and got {BulkResult}"
    )
    ]
    public static partial void LogInfoVerifyBulk(this ILogger logger, IList<SubscriptionCharge> subscriptions, string externalBulkVerificationId, BulkSubscriptionResult bulkResult);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not verify bulk {ExternalBulkVerificationId} containing {Subscriptions}, got response {Response}."
    )
    ]
    public static partial void LogErrorVerifyBulk(this ILogger logger, string externalBulkVerificationId, IList<SubscriptionCharge> subscriptions, string response);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Invalid subscription {Subscription}"
    )
    ]
    public static partial void LogErrorInvalidSubscription(this ILogger logger, SubscriptionCharge subscription);

    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Got verification for {BulkId} and it is {Page}."
    )
    ]
    public static partial void LogInfoRetrieveBulkVerification(this ILogger logger, Guid bulkId, PageResult<SubscriptionVerificationStatus> page);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not retrieve verification for {BulkId}. Got response: {Response}."
    )
    ]
    public static partial void LogErrorRetrieveBulkVerification(this ILogger logger, Guid bulkId, string response);

    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Got unscheduled subscription with id {Id} and it is {UnscheduledSubscription}."
    )
    ]
    public static partial void LogInfoRetrieveUnscheduledSubscription(this ILogger logger, Guid id, UnscheduledSubscriptionDetails unscheduledSubscription);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not retrieve unscheduled subscription with {Id}. Got response: {Response}"
    )
    ]
    public static partial void LogErrorRetrieveUnscheduledSubscription(this ILogger logger, Guid id, string response);

    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Got unscheduled subscription with external reference {ExternalReference} and it is {UnscheduledSubscription}."
    )
    ]
    public static partial void LogInfoRetrieveUnscheduledSubscription(this ILogger logger, string externalReference, UnscheduledSubscriptionDetails unscheduledSubscription);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not retrieve unscheduled subscription external reference {ExternalReference}. Got response: {Response}"
    )
    ]
    public static partial void LogErrorRetrieveUnscheduledSubscription(this ILogger logger, string externalReference, string response);

    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Charged unscheduled subscription with {Id} and {Charge}. Got {Result}"
    )
    ]
    public static partial void LogInfoChargeUnscheduledSubscription(this ILogger logger, Guid id, UnscheduledSubscriptionCharge charge, UnscheduledSubscriptionChargeResult result);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not charge unscheduled subscription with {Id} and {Charge}. Got {Response}."
    )
    ]
    public static partial void LogErrorChargeUnscheduledSubscription(this ILogger logger, Guid id, UnscheduledSubscriptionCharge charge, string response);

    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Bulk charged unscheduled subscriptions {Subscriptions} with {ExternalBulkChargeId}. Got bulk id: {BulkId}."
    )
    ]
    public static partial void LogInfoBulkChargeUnscheduledSubscriptions(this ILogger logger, IList<ChargeUnscheduledSubscription> subscriptions, string externalBulkChargeId, Guid bulkId);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not bulk charge id {ExternalBulkChargeId} the {Subscriptions}. Got response: {Response}."
    )
    ]
    public static partial void LogErrorBulkChargeUnscheduledSubscriptions(this ILogger logger, string externalBulkChargeId, IList<ChargeUnscheduledSubscription> subscriptions, string response);

    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Retrieved page of unscheduled subscriptions for {BulkId} got {PageStatusSubscriptions}"
    )
    ]
    public static partial void LogInfoRetrieveBulkUnscheduledCharges(this ILogger logger, Guid bulkId, PageResult<UnscheduledSubscriptionProcessStatus> pageStatusSubscriptions);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not retrieve bulk unscheduled charges for {BulkId}. Got response: {Response}."
    )
    ]
    public static partial void LogErrorRetrieveBulkUnscheduledCharges(this ILogger logger, Guid bulkId, string response);

    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Bulk id: {BulkId} verification status of {Subscriptions}."
    )
    ]
    public static partial void LogInfoRetrieveBulkVerificationsForUnscheduledSubscriptions(this ILogger logger, Guid bulkId, PageResult<UnscheduledSubscriptionVerificationStatus> subscriptions);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not retrieve page of verification status for unscheduled subscriptions with {BulkId}. Got response: {Response}."
    )
    ]
    public static partial void LogErrorRetrieveBulkVerificationsForUnscheduledSubscriptions(this ILogger logger, Guid bulkId, string response);
}
