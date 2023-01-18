using System;
using Microsoft.Extensions.Logging;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;

namespace SolidNetsEasyClient.Logging.PaymentClientLogging;

/// <summary>
/// Payment client logging extensions
/// </summary>
public static partial class LogExtensions
{
    /// <summary>
    /// Error invalid payment or API key
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="payment">The payment</param>
    /// <param name="apiKey">The API key</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Invalid {Payment} or {ApiKey}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidPaymentOrApiKey(this ILogger logger, PaymentRequest payment, string apiKey);

    /// <summary>
    /// Trace payment creation
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="payment">The payment</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Creating new {Payment}"
    )]
    public static partial void TracePaymentCreation(this ILogger logger, PaymentRequest payment);

    /// <summary>
    /// Trace raw response from NETS
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="response">The response</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Raw content: {Response}"
    )]
    public static partial void TraceRawResponse(this ILogger logger, string? response);

    /// <summary>
    /// Result of creating payment
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="payment">The payment</param>
    /// <param name="result">The result response</param>
    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Created {Payment} with a {Result}",
        SkipEnabledCheck = true
    )]
    public static partial void InfoCreatedPayment(this ILogger logger, PaymentRequest payment, PaymentResult result);

    /// <summary>
    /// Exception occurred when creating payment
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="payment">The payment</param>
    /// <param name="exception">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "An exception occurred trying to create a payment with {Payment}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorCreateException(this ILogger logger, PaymentRequest payment, Exception? exception);

    /// <summary>
    /// Error invalid payment ID
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="paymentID">The payment ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Invalid {PaymentID}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidPaymentID(this ILogger logger, Guid paymentID);

    /// <summary>
    /// Trace payment retrieval
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="paymentID">The payment ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Retrieving payment status for {PaymentID}"
    )]
    public static partial void TracePaymentRetrieval(this ILogger logger, Guid paymentID);

    /// <summary>
    /// Info retrieved payment
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="paymentStatus">The retrieved payment status</param>
    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Retrieved {PaymentStatus}",
        SkipEnabledCheck = true
    )]
    public static partial void InfoRetrievedPayment(this ILogger logger, PaymentStatus paymentStatus);

    /// <summary>
    /// Error retrieving payment
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="paymentID">The payment ID</param>
    /// <param name="ex">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "An exception occurred trying to retrieve payment status for {PaymentID}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorRetrievalException(this ILogger logger, Guid paymentID, Exception? ex);

    /// <summary>
    /// Error invalid update payment arguments
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="payment">The payment</param>
    /// <param name="checkoutUrl">The checkout url</param>
    /// <param name="reference">The reference</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Invalid {Payment} or {CheckoutUrl} or {Reference}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidPaymentReferenceUpdates(this ILogger logger, Payment payment, string checkoutUrl, string reference);

    /// <summary>
    /// Trace arguments when updating references
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="payment">The payment</param>
    /// <param name="checkoutUrl">The checkout url</param>
    /// <param name="reference">The reference</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Updating references for {Payment} to {CheckoutUrl} and {Reference}"
    )]
    public static partial void TraceUpdatingReferences(this ILogger logger, Payment payment, string checkoutUrl, string reference);

    /// <summary>
    /// Exception updating payment references
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="payment">The payment</param>
    /// <param name="checkoutUrl">The checkout url</param>
    /// <param name="reference">The reference</param>
    /// <param name="ex">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "An exception occurred trying to update payment references for {Payment} and {CheckoutUrl} and {Reference}",
        SkipEnabledCheck = true
    )]
    public static partial void ExceptionUpdatingReferences(this ILogger logger, Payment payment, string checkoutUrl, string reference, Exception ex);

    /// <summary>
    /// Error invalid order or payment ID
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="orderUpdate">The order updates</param>
    /// <param name="paymentID">The payment ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Invalid {OrderUpdate} or {PaymentID}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidOrderOrPayment(this ILogger logger, OrderUpdate orderUpdate, Guid paymentID);

    /// <summary>
    /// Trace payment updates
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="paymentID">The payment ID</param>
    /// <param name="orderUpdates">The order updates</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Updating order for {PaymentID} to {OrderUpdates}"
    )]
    public static partial void TracePaymentUpdate(this ILogger logger, Guid paymentID, OrderUpdate orderUpdates);

    /// <summary>
    /// Exception when updating order
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="paymentID">The payment ID</param>
    /// <param name="orderUpdates">The order updates</param>
    /// <param name="ex">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "An error occurred updating order items for {PaymentID} and {OrderUpdates}",
        SkipEnabledCheck = true
    )]
    public static partial void ExceptionUpdatingOrder(this ILogger logger, Guid paymentID, OrderUpdate orderUpdates, Exception ex);

    /// <summary>
    /// Error missing payment ID
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="paymentID">The payment ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Missing,
        Level = LogLevel.Error,
        Message = "Missing id: {PaymentID}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorMissingPaymentID(this ILogger logger, Guid paymentID);

    /// <summary>
    /// Trace payment termination
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="paymentID">The payment ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Terminating checkout for {PaymentID}"
    )]
    public static partial void TraceTermination(this ILogger logger, Guid paymentID);

    /// <summary>
    /// Exception when terminating payment
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="paymentID">The payment ID</param>
    /// <param name="ex">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "An error occurred terminating checkout for {PaymentID}",
        SkipEnabledCheck = true
    )]
    public static partial void ExceptionTerminatingPayment(this ILogger logger, Guid paymentID, Exception ex);

    /// <summary>
    /// Trace cancellation
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="paymentID">The payment ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Cancelling checkout for {PaymentID}"
    )]
    public static partial void TraceCancellation(this ILogger logger, Guid paymentID);

    /// <summary>
    /// Exception cancelling payment
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="paymentID">The payment ID</param>
    /// <param name="ex">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "An error occurred cancelling checkout for {PaymentID}",
        SkipEnabledCheck = true
    )]
    public static partial void ExceptionCancellingPayment(this ILogger logger, Guid paymentID, Exception ex);

    /// <summary>
    /// Error invalid payment or idempotency key
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="paymentID">The payment ID</param>
    /// <param name="idempotencyKey">The idempotency key</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Invalid {PaymentID} or {IdempotencyKey}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidPaymentOrIdempotencyKey(this ILogger logger, Guid paymentID, string? idempotencyKey);

    /// <summary>
    /// Trace charging payment
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="paymentID">The payment ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Charging payment for {PaymentID}"
    )]
    public static partial void TraceChargingPayment(this ILogger logger, Guid paymentID);

    /// <summary>
    /// Charge result
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="paymentID">The payment ID</param>
    /// <param name="charge">The charge</param>
    /// <param name="result">The result of charging</param>
    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Charged {PaymentID} with {Charge} and got {Result}",
        SkipEnabledCheck = true
    )]
    public static partial void InfoChargeResult(this ILogger logger, Guid paymentID, Charge charge, ChargeResult result);

    /// <summary>
    /// Exception occurred charging payment
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="paymentID">The payment ID</param>
    /// <param name="ex">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "An error occurred finalizing charge for {PaymentID}",
        SkipEnabledCheck = true
    )]
    public static partial void ExceptionChargingPayment(this ILogger logger, Guid paymentID, Exception ex);

    /// <summary>
    /// Error invalid charge ID
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="chargeID">The charge ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Invalid {ChargeID}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidChargeID(this ILogger logger, Guid chargeID);

    /// <summary>
    /// Trace charge retrieval
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="chargeID">The charge ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Retrieving charge for {ChargeID}"
    )]
    public static partial void TraceChargeRetrieval(this ILogger logger, Guid chargeID);

    /// <summary>
    /// Info charge details
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="chargeDetails">The charge details</param>
    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Retrieved charge: {ChargeDetails}",
        SkipEnabledCheck = true
    )]
    public static partial void InfoChargeDetails(this ILogger logger, ChargeDetailsInfo chargeDetails);

    /// <summary>
    /// Exception when retrieving charge details
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="chargeID">The charge ID</param>
    /// <param name="ex">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "An error occurred retrieving charge for {ChargeID}",
        SkipEnabledCheck = true
    )]
    public static partial void ExceptionRetrievingCharge(this ILogger logger, Guid chargeID, Exception ex);

    /// <summary>
    /// Error invalid charge ID or idempotency key
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="chargeID">The charge ID</param>
    /// <param name="idempotencyKey">The idempotency key</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Invalid {ChargeID} or {IdempotencyKey}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidChargeOrIdempotencyKey(this ILogger logger, Guid chargeID, string? idempotencyKey);

    /// <summary>
    /// Trace refunding
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="chargeID">The charge ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Refunding charge {ChargeID}"
    )]
    public static partial void TraceRefund(this ILogger logger, Guid chargeID);

    /// <summary>
    /// Info refund charge
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="chargeID">The charge ID</param>
    /// <param name="refund">The refund</param>
    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Refunded: {ChargeID} for {Refund}",
        SkipEnabledCheck = true
    )]
    public static partial void InfoRefundCharge(this ILogger logger, Guid chargeID, Refund refund);

    /// <summary>
    /// Exception refunding charge
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="chargeID">The charge ID</param>
    /// <param name="ex">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "An error occurred refunding charge for {ChargeID}",
        SkipEnabledCheck = true
    )]
    public static partial void ExceptionRefundCharge(this ILogger logger, Guid chargeID, Exception ex);

    /// <summary>
    /// Error invalid refund ID
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="refundID">The refund ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Invalid {RefundId}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidRefundID(this ILogger logger, Guid refundID);

    /// <summary>
    /// Trace refund
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="refundID">The refund ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Retrieving refund for {RefundId}"
    )]
    public static partial void TraceRetrievingRefund(this ILogger logger, Guid refundID);

    /// <summary>
    /// Info retrieve refund details
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="refund">The refund</param>
    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Refund result: {Refund}",
        SkipEnabledCheck = true
    )]
    public static partial void InfoRetrieveRefund(this ILogger logger, RetrieveRefund refund);

    /// <summary>
    /// Exception occurred retrieving refund
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="refundID">The refund ID</param>
    /// <param name="ex">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "An error occurred retrieving refund {RefundId}",
        SkipEnabledCheck = true
    )]
    public static partial void ExceptionRetrieveRefund(this ILogger logger, Guid refundID, Exception ex);

    /// <summary>
    /// Trace cancel pending refund
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="refundID">The refund ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Cancelling pending refund for {RefundId}"
    )]
    public static partial void TraceCancelPendingRefund(this ILogger logger, Guid refundID);

    /// <summary>
    /// Info cancelled pending refund
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="refundID">The refund ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Cancelled pending refund {RefundId}",
        SkipEnabledCheck = true
    )]
    public static partial void InfoCancelledPendingRefund(this ILogger logger, Guid refundID);

    /// <summary>
    /// Exception occurred cancelling pending refund
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="refundID">The refund ID</param>
    /// <param name="ex">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "An error occurred cancelling pending refund {RefundId}",
        SkipEnabledCheck = true
    )]
    public static partial void ExceptionCancellingRefund(this ILogger logger, Guid refundID, Exception ex);

    /// <summary>
    /// Trace updating payment references
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="paymentID">The payment ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Neutral.Info,
        Level = LogLevel.Trace,
        Message = "Updating my reference for {PaymentId}"
    )]
    public static partial void TraceUpdatePaymentReference(this ILogger logger, Guid paymentID);

    /// <summary>
    /// Info updated payment references
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="myReference">The merchant payment reference</param>
    /// <param name="paymentID">The payment ID</param>
    [LoggerMessage(
        EventId = LogEventIDs.Success.Correct,
        Level = LogLevel.Information,
        Message = "Updated my reference to {MyReference} for {PaymentId}",
        SkipEnabledCheck = true
    )]
    public static partial void InfoUpdatedReferences(this ILogger logger, string? myReference, Guid paymentID);

    /// <summary>
    /// Exception occurred updating payment references
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="myReference">The merchant payment reference</param>
    /// <param name="paymentID">The payment ID</param>
    /// <param name="ex">The exception</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "An error occurred updating my reference {MyReference} for {PaymentId}",
        SkipEnabledCheck = true
    )]
    public static partial void ExceptionUpdatingPaymentReferences(this ILogger logger, string? myReference, Guid paymentID, Exception ex);
}
