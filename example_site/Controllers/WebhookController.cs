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

    [SolidNetsEasyPaymentCreated("custom/{number:int}/route", Name = "CustomPaymentCreated")]
    public ActionResult DuplicatePaymentCreated([FromBody] PaymentCreated payment, int number)
    {
        Logger.LogInformation("The header: {@Headers}", Request.Headers);
        Logger.LogInformation("The authorization header: {Authorization}", Request.Headers.Authorization!);
        Logger.LogInformation("The data: {@PaymentCreated} and {Number}", payment, number);
        return Ok();
    }

    /// <inheritdoc />
    [SolidNetsEasyChargeCreated("nets/my_charge")]
    public override ActionResult NetsChargeCreated([FromBody] ChargeCreated charge, CancellationToken cancellationToken)
    {
        return base.NetsChargeCreated(charge, cancellationToken);
    }
}