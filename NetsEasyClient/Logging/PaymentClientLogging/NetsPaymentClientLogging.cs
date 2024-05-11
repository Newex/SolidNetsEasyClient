using System;
using Microsoft.Extensions.Logging;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;

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
        Message = "Refunded id: {ChargeId} containing: {Charge}, refund result is: {RefundResult}"
    )
    ]
    public static partial void LogInfoRefundResult(this ILogger logger, Guid chargeId, CancelOrder charge, RefundResult refundResult);

    [LoggerMessage(
        EventId = LogEventIDs.Errors.Error,
        Level = LogLevel.Error,
        Message = "Could not refund {ChargeId} containing {Charge}. Get response: {Response}"
    )
    ]
    public static partial void LogErrorRefundCharge(this ILogger logger, Guid chargeId, CancelOrder charge, string response);
}
