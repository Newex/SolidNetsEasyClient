using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolidNetsEasyClient.Helpers.WebhookAttributes;
using SolidNetsEasyClient.Logging;
using SolidNetsEasyClient.Logging.NetsWebhookControllerLogging;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Helpers.Controllers;

/// <summary>
/// Default Nets webhook controller. Override the Nets actions to handle the given webhook event.
/// </summary>
/// <remarks>
/// Default implementation only logs the headers, the event DTO and then returns a 200 OK.
/// </remarks>
public abstract class NetsWebhookController : Controller
{
    /// <summary>
    /// Get logger
    /// </summary>
    protected abstract ILogger Logger { get; init; }

    /// <summary>
    /// The webhook endpoint for the <see cref="EventName.PaymentCreated"/> event
    /// </summary>
    /// <param name="payment">The payment created details</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>On successful handling must return status 200 OK</returns>
    [SolidNetsEasyPaymentCreated("nets/payment/created")]
    public virtual ActionResult NetsPaymentCreated([FromBody] PaymentCreated payment, CancellationToken cancellationToken)
    {
        Logger.InfoHeader(Request.Headers);
        Logger.InfoAuthorizationHeader(Request.Headers.Authorization);
        Logger.InfoData<PaymentCreated, PaymentCreatedData>(payment);
        return Ok();
    }

    /// <summary>
    /// The webhook endpoint for the <see cref="EventName.ReservationCreatedV1"/> event
    /// </summary>
    /// <param name="reservation">The reservation created event details</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>On successful handling must return status 200 OK</returns>
    [SolidNetsEasyReservationCreatedV1("nets/reservationv1/created")]
    public virtual ActionResult NetsReservationCreatedV1([FromBody] ReservationCreatedV1 reservation, CancellationToken cancellationToken)
    {
        Logger.InfoHeader(Request.Headers);
        Logger.InfoAuthorizationHeader(Request.Headers.Authorization);
        Logger.InfoData<ReservationCreatedV1, ReservationCreatedDataV1>(reservation);
        return Ok();
    }

    /// <summary>
    /// The webhook endpoint for the <see cref="EventName.ReservationCreatedV2"/> event
    /// </summary>
    /// <param name="reservation">The reservation created event details</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>On successful handling must return status 200 OK</returns>
    [SolidNetsEasyReservationCreatedV2("nets/reservationv2/created")]
    public virtual ActionResult NetsReservationCreatedV2([FromBody] ReservationCreatedV2 reservation, CancellationToken cancellationToken)
    {
        Logger.InfoHeader(Request.Headers);
        Logger.InfoAuthorizationHeader(Request.Headers.Authorization);
        Logger.InfoData<ReservationCreatedV2, ReservationCreatedDataV2>(reservation);
        return Ok();
    }

    /// <summary>
    /// The webhook endpoint for the <see cref="EventName.ReservationFailed"/> event
    /// </summary>
    /// <param name="reservation">The reservation failed event details</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>On successful handling must return status 200 OK</returns>
    [SolidNetsEasyReservationFailed("nets/reservation/failed")]
    public virtual ActionResult NetsReservationFailed([FromBody] ReservationFailed reservation, CancellationToken cancellationToken)
    {
        Logger.InfoHeader(Request.Headers);
        Logger.InfoAuthorizationHeader(Request.Headers.Authorization);
        Logger.InfoData<ReservationFailed, ReservationFailedData>(reservation);
        return Ok();
    }

    /// <summary>
    /// The webhook endpoint for the <see cref="EventName.CheckoutCompleted"/> event
    /// </summary>
    /// <param name="checkout">The checkout completed event details</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>On successful handling must return status 200 OK</returns>
    [SolidNetsEasyCheckoutCompleted("nets/checkout/completed")]
    public virtual ActionResult NetsCheckoutCompleted([FromBody] CheckoutCompleted checkout, CancellationToken cancellationToken)
    {
        Logger.InfoHeader(Request.Headers);
        Logger.InfoAuthorizationHeader(Request.Headers.Authorization);
        Logger.InfoData<CheckoutCompleted, CheckoutCompletedData>(checkout);
        return Ok();
    }

    /// <summary>
    /// The webhook endpoint for the <see cref="EventName.ReservationFailed"/> event
    /// </summary>
    /// <param name="charge">The charge created event details</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>On successful handling must return status 200 OK</returns>
    [SolidNetsEasyChargeCreated("nets/charge/created")]
    public virtual ActionResult NetsChargeCreated([FromBody] ChargeCreated charge, CancellationToken cancellationToken)
    {
        Logger.InfoHeader(Request.Headers);
        Logger.InfoAuthorizationHeader(Request.Headers.Authorization);
        Logger.InfoData<ChargeCreated, ChargeData>(charge);
        return Ok();
    }

    /// <summary>
    /// The webhook endpoint for the <see cref="EventName.PaymentCancelled"/> event
    /// </summary>
    /// <param name="payment">The payment cancelled event details</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>On successful handling must return status 200 OK</returns>
    [SolidNetsEasyPaymentCancelled("nets/payment/cancelled")]
    public virtual ActionResult NetsPaymentCancelled([FromBody] PaymentCancelled payment, CancellationToken cancellationToken)
    {
        Logger.InfoHeader(Request.Headers);
        Logger.InfoAuthorizationHeader(Request.Headers.Authorization);
        Logger.InfoData<PaymentCancelled, PaymentCancelledData>(payment);
        return Ok();
    }

    /// <summary>
    /// The webhook endpoint for the <see cref="EventName.PaymentCancellationFailed"/> event
    /// </summary>
    /// <param name="payment">The payment cancellation failed event details</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>On successful handling must return status 200 OK</returns>
    [SolidNetsEasyPaymentCancellationFailed("nets/payment/cancelled/failed")]
    public virtual ActionResult NetsPaymentCancellationFailed([FromBody] PaymentCancellationFailed payment, CancellationToken cancellationToken)
    {
        Logger.InfoHeader(Request.Headers);
        Logger.InfoAuthorizationHeader(Request.Headers.Authorization);
        Logger.InfoData<PaymentCancellationFailed, PaymentCancellationFailedData>(payment);
        return Ok();
    }

    /// <summary>
    /// The webhook endpoint for the <see cref="EventName.ChargeFailed"/> event
    /// </summary>
    /// <param name="charge">The charge failed event details</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>On successful handling must return status 200 OK</returns>
    [SolidNetsEasyChargeFailed("nets/charge/failed")]
    public virtual ActionResult NetsChargeFailed([FromBody] ChargeFailed charge, CancellationToken cancellationToken)
    {
        Logger.InfoHeader(Request.Headers);
        Logger.InfoAuthorizationHeader(Request.Headers.Authorization);
        Logger.InfoData<ChargeFailed, ChargeFailedData>(charge);
        return Ok();
    }

    /// <summary>
    /// The webhook endpoint for the <see cref="EventName.RefundInitiated"/> event
    /// </summary>
    /// <param name="refund">The refund initiated event details</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>On successful handling must return status 200 OK</returns>
    [SolidNetsEasyRefundInitiated("nets/refund/initiated")]
    public virtual ActionResult NetsRefundInitiated([FromBody] RefundInitiated refund, CancellationToken cancellationToken)
    {
        Logger.InfoHeader(Request.Headers);
        Logger.InfoAuthorizationHeader(Request.Headers.Authorization);
        Logger.InfoData<RefundInitiated, RefundInitiatedData>(refund);
        return Ok();
    }

    /// <summary>
    /// The webhook endpoint for the <see cref="EventName.RefundCompleted"/> event
    /// </summary>
    /// <param name="refund">The refund completed event details</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>On successful handling must return status 200 OK</returns>
    [SolidNetsEasyRefundCompleted("nets/refund/completed")]
    public virtual ActionResult NetsRefundCompleted([FromBody] RefundCompleted refund, CancellationToken cancellationToken)
    {
        Logger.InfoHeader(Request.Headers);
        Logger.InfoAuthorizationHeader(Request.Headers.Authorization);
        Logger.InfoData<RefundCompleted, RefundCompletedData>(refund);
        return Ok();
    }

    /// <summary>
    /// The webhook endpoint for the <see cref="EventName.RefundFailed"/> event
    /// </summary>
    /// <param name="refund">The refund failed event details</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>On successful handling must return status 200 OK</returns>
    [SolidNetsEasyRefundFailed("nets/refund/failed")]
    public virtual ActionResult NetsRefundFailed([FromBody] RefundFailed refund, CancellationToken cancellationToken)
    {
        Logger.InfoHeader(Request.Headers);
        Logger.InfoAuthorizationHeader(Request.Headers.Authorization);
        Logger.InfoData<RefundFailed, RefundFailedData>(refund);
        return Ok();
    }
}

file static class WebhookDataLogger<T, D>
        where T : Webhook<D>
        where D : IWebhookData, new()
{
    public static readonly Action<ILogger, T, Exception?> DataLogger = LoggerMessage.Define<T>(LogLevel.Information, LogEventIDs.Neutral.Info, "The data: {Data}", new LogDefineOptions
    {
        SkipEnabledCheck = true
    });
}

/// <summary>
/// Log webhook data extension
/// </summary>
public static class LogDataExtension
{
    /// <summary>
    /// Log webhook data as <see cref="LogLevel.Information"/>
    /// </summary>
    /// <typeparam name="T">The webhook type</typeparam>
    /// <typeparam name="D">The webhook data payload type</typeparam>
    /// <param name="logger">The logger</param>
    /// <param name="data">The webhook event</param>
    public static void InfoData<T, D>(this ILogger logger, T data)
        where T : Webhook<D>
        where D : IWebhookData, new()
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            WebhookDataLogger<T, D>.DataLogger(logger, data, null);
        }
    }
}