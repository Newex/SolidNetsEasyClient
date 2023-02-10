using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SolidNetsEasyClient.Filters;
using SolidNetsEasyClient.Helpers.WebhookAttributes;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

namespace ExampleSite.Controllers;

[SolidNetsEasyIPFilter(WhitelistIPs = "::1")]
[Route("/webhook")]
public class WebhookController : Controller
{
    private readonly ILogger<WebhookController> logger;

    public WebhookController(
        ILogger<WebhookController>? logger = null
    )
    {
        this.logger = logger ?? NullLogger<WebhookController>.Instance;
    }

    [SolidNetsEasyPaymentCreated("nets/payment/created")]
    public ActionResult PaymentCreated([FromBody] PaymentCreated payment)
    {
        logger.LogInformation("The header: {@Headers}", Request.Headers);
        logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        logger.LogInformation("The data: {@Payment}", payment);
        return Ok();
    }

    [SolidNetsEasyReservationCreatedV1("nets/reservationv1/created")]
    public ActionResult ReservationCreatedV1([FromBody] ReservationCreatedV1 reservation)
    {
        logger.LogInformation("The header: {@Headers}", Request.Headers);
        logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        logger.LogInformation("The data: {@ReservationV1}", reservation);
        return Ok();
    }

    [SolidNetsEasyReservationCreatedV2("nets/reservationv2/created")]
    public ActionResult ReservationCreatedV2([FromBody] ReservationCreatedV2 reservation)
    {
        logger.LogInformation("The header: {@Headers}", Request.Headers);
        logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        logger.LogInformation("The data: {@ReservationV2}", reservation);
        return Ok();
    }

    [SolidNetsEasyReservationFailed("nets/reservation/failed")]
    public ActionResult ReservationFailed([FromBody] ReservationFailed reservation)
    {
        logger.LogInformation("The header: {@Headers}", Request.Headers);
        logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        logger.LogInformation("The data: {@ReservationFailed}", reservation);
        return Ok();
    }

    [SolidNetsEasyCheckoutCompleted("nets/checkout/completed")]
    public ActionResult CheckoutCompleted([FromBody] CheckoutCompleted checkout)
    {
        logger.LogInformation("The header: {@Headers}", Request.Headers);
        logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        logger.LogInformation("The data: {@CheckoutCompleted}", checkout);
        return Ok();
    }

    [SolidNetsEasyChargeCreated("nets/charge/created")]
    public ActionResult ChargeCreated([FromBody] ChargeCreated charge)
    {
        logger.LogInformation("The header: {@Headers}", Request.Headers);
        logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        logger.LogInformation("The data: {@ChargeCreated}", charge);
        return Ok();
    }

    [SolidNetsEasyPaymentCancelled("nets/payment/cancelled")]
    public ActionResult PaymentCancelled([FromBody] PaymentCancelled payment)
    {
        logger.LogInformation("The header: {@Headers}", Request.Headers);
        logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        logger.LogInformation("The data: {@PaymentCancelled}", payment);
        return Ok();
    }

    [SolidNetsEasyPaymentCancellationFailed("nets/payment/cancelled/failed")]
    public ActionResult PaymentCancelled([FromBody] PaymentCancellationFailed payment)
    {
        logger.LogInformation("The header: {@Headers}", Request.Headers);
        logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        logger.LogInformation("The data: {@PaymentCancellationFailed}", payment);
        return Ok();
    }

    [SolidNetsEasyChargeFailed("nets/charge/failed")]
    public ActionResult ChargeFailed([FromBody] ChargeFailed charge)
    {
        logger.LogInformation("The header: {@Headers}", Request.Headers);
        logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        logger.LogInformation("The data: {@ChargeFailed}", charge);
        return Ok();
    }

    [SolidNetsEasyRefundInitiated("nets/refund/initiated")]
    public ActionResult RefundInitiated([FromBody] RefundInitiated refund)
    {
        logger.LogInformation("The header: {@Headers}", Request.Headers);
        logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        logger.LogInformation("The data: {@RefundInitiated}", refund);
        return Ok();
    }

    [SolidNetsEasyRefundCompleted("nets/refund/completed")]
    public ActionResult RefundCompleted([FromBody] RefundCompleted refund)
    {
        logger.LogInformation("The header: {@Headers}", Request.Headers);
        logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        logger.LogInformation("The data: {@RefundCompleted}", refund);
        return Ok();
    }

    [SolidNetsEasyRefundFailed("nets/refund/failed")]
    public ActionResult RefundFailed([FromBody] RefundFailed refund)
    {
        logger.LogInformation("The header: {@Headers}", Request.Headers);
        logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        logger.LogInformation("The data: {@RefundFailed}", refund);
        return Ok();
    }

    // Must set the name. Each route must have a unique name! This route also needs a parameter: number
    [SolidNetsEasyPaymentCreated("custom/{number:int}/route", Name = "CustomPaymentCreated")]
    public ActionResult DuplicatePaymentCreated([FromBody] PaymentCreated payment, int number)
    {
        logger.LogInformation("The header: {@Headers}", Request.Headers);
        logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        logger.LogInformation("The data: {@PaymentCreated} and {Number}", payment, number);
        return Ok();
    }
}
