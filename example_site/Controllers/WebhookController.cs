using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SolidNetsEasyClient.Filters;
using SolidNetsEasyClient.Helpers.WebhookAttributes;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

namespace ExampleSite.Controllers;

[SolidNetsEasyIPFilter(WhitelistIPs = "::1")]
public class WebhookController : Controller
{
    private readonly ILogger<WebhookController> logger;

    public WebhookController(
        ILogger<WebhookController>? logger = null
    )
    {
        this.logger = logger ?? NullLogger<WebhookController>.Instance;
    }

    [SolidNetsEasyPaymentCreated("/nets/payment/created")]
    public ActionResult PaymentCreated([FromBody] PaymentCreated payment)
    {
        logger.LogInformation("The header: {@Headers}", Request.Headers);
        logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        logger.LogInformation("The data: {@Payment}", payment);
        return NoContent();
    }

    [SolidNetsEasyReservationCreatedV1("/nets/reservationv1/created")]
    public ActionResult ReservationCreatedV1([FromBody] ReservationCreatedV1 reservation)
    {
        logger.LogInformation("The header: {@Headers}", Request.Headers);
        logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        logger.LogInformation("The data: {@ReservationV1}", reservation);
        return NoContent();
    }
}
