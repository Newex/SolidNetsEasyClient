using System.Threading;
using System.Threading.Tasks;
using ExampleSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SolidNetsEasyClient.Builder;
using SolidNetsEasyClient.Clients;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Requests.Webhooks;

namespace ExampleSite.Controllers;

public class CheckoutController : Controller
{
    private readonly NetsPaymentClient client;
    private readonly NetsPaymentBuilder paymentBuilder;
    private readonly ILogger<CheckoutController> logger;

    public CheckoutController(
        NetsPaymentClient client,
        NetsPaymentBuilder paymentBuilder,
        ILogger<CheckoutController>? logger = null)
    {
        this.client = client;
        this.paymentBuilder = paymentBuilder;
        this.logger = logger ?? NullLogger<CheckoutController>.Instance;
    }

    [HttpPost("/checkout")]
    public async Task<ActionResult> Index(BasketViewModel basket, CancellationToken cts)
    {
        var order = PaymentRequestHelper.MinimalOrderExample(basket.Item, basket.Quantity);

        var paymentRequest = paymentBuilder.CreateSinglePayment(order)
                                    .AddWebhook("https://localhost/webhook/callback", EventName.PaymentCreated, "randomAuth123")
                                    .AddWebhook("https://localhost/webhook/callback", EventName.PaymentCancelled, "randomAuth123")
                                    .Build();
        var payment = await client.StartCheckoutPayment(paymentRequest, cts);

        if (payment is null)
        {
            return View("SomethingWentWrong");
        }

        var vm = new CheckoutViewModel
        {
            CheckoutKey = client.CheckoutKey ?? string.Empty,
            PaymentID = payment.PaymentId.ToString("N")
        };
        return View(vm);
    }
}
