using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolidNetsEasyClient.Helpers.WebhookAttributes;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

namespace SolidNetsEasyClient.Helpers.Controllers;

/// <summary>
/// Default Nets webhook controller
/// </summary>
/// <remarks>
/// Every action only logs the headers, the event DTO and then returns a 200 OK.
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
        Logger.LogInformation("The header: {@Headers}", Request.Headers);
        Logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        Logger.LogInformation("The data: {@Payment}", payment);
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
        Logger.LogInformation("The header: {@Headers}", Request.Headers);
        Logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        Logger.LogInformation("The data: {@ReservationV1}", reservation);
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
        Logger.LogInformation("The header: {@Headers}", Request.Headers);
        Logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        Logger.LogInformation("The data: {@ReservationV2}", reservation);
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
        Logger.LogInformation("The header: {@Headers}", Request.Headers);
        Logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        Logger.LogInformation("The data: {@ReservationFailed}", reservation);
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
        Logger.LogInformation("The header: {@Headers}", Request.Headers);
        Logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        Logger.LogInformation("The data: {@CheckoutCompleted}", checkout);
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
        Logger.LogInformation("The header: {@Headers}", Request.Headers);
        Logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        Logger.LogInformation("The data: {@ChargeCreated}", charge);
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
        Logger.LogInformation("The header: {@Headers}", Request.Headers);
        Logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        Logger.LogInformation("The data: {@PaymentCancelled}", payment);
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
        Logger.LogInformation("The header: {@Headers}", Request.Headers);
        Logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        Logger.LogInformation("The data: {@PaymentCancellationFailed}", payment);
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
        Logger.LogInformation("The header: {@Headers}", Request.Headers);
        Logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        Logger.LogInformation("The data: {@ChargeFailed}", charge);
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
        Logger.LogInformation("The header: {@Headers}", Request.Headers);
        Logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        Logger.LogInformation("The data: {@RefundInitiated}", refund);
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
        Logger.LogInformation("The header: {@Headers}", Request.Headers);
        Logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        Logger.LogInformation("The data: {@RefundCompleted}", refund);
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
        Logger.LogInformation("The header: {@Headers}", Request.Headers);
        Logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        Logger.LogInformation("The data: {@RefundFailed}", refund);
        return Ok();
    }
}
