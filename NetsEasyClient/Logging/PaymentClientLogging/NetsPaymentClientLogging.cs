using System;
using Microsoft.Extensions.Logging;
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
}
