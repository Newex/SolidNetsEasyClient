using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SolidNetsEasyClient.Filters;
using SolidNetsEasyClient.Helpers.Controllers;
using SolidNetsEasyClient.Helpers.WebhookAttributes;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

namespace ExampleSite.Controllers;

[Route("/webhook")]
[SolidNetsEasyIPFilter]
public sealed class WebhookController : NetsWebhookController
{
    public WebhookController(
        ILogger? logger = null
    )
    {
        Logger = logger ?? NullLogger.Instance;
    }

    protected override ILogger Logger { get; init; }

    /// <summary>
    /// Override one of the default hooks to implement custom logic handler
    /// </summary>
    /// <param name="charge">The charge webhook event</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>200 OK if handled event - note that other results (i.e. 201 or 404) will make Nets renotify for a total of 10 times.</returns>
    [SolidNetsEasyChargeCreated("nets/my_charge")]
    public override ActionResult NetsChargeCreated([FromBody] ChargeCreated charge, CancellationToken cancellationToken)
    {
        return base.NetsChargeCreated(charge, cancellationToken);
    }

    /// <summary>
    /// When duplicating a handler you must also set the Name!
    /// </summary>
    /// <param name="payment">The payment created event</param>
    /// <param name="number">A number</param>
    /// <returns>200 OK</returns>
    [SolidNetsEasyPaymentCreated("custom/{number:int}/route", Name = "CustomPaymentCreated")]
    public ActionResult DuplicatePaymentCreated([FromBody] PaymentCreated payment, int number)
    {
        Logger.LogInformation("The header: {@Headers}", Request.Headers);
        Logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        Logger.LogInformation("The data: {@PaymentCreated} and {Number}", payment, number);
        return Ok();
    }
}