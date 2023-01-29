using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SolidNetsEasyClient.Filters;
using SolidNetsEasyClient.Helpers.WebhookAttributes;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

namespace ExampleSite.Controllers;

public class WebhookController : Controller
{
    private readonly ILogger<WebhookController> logger;

    public WebhookController(
        ILogger<WebhookController>? logger = null
    )
    {
        this.logger = logger ?? NullLogger<WebhookController>.Instance;
    }

    [SolidNetsEasyIPFilter(WhitelistIPs = "::1")]
    [SolidNetsEasyPaymentCreated("/nets/created")]
    public ActionResult Post([FromBody] PaymentCreated payment)
    {
        logger.LogInformation("The header: {@Headers}", Request.Headers);
        logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        logger.LogInformation("The data: {@Payment}", payment);
        return NoContent();
    }
}
