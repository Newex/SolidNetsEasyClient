using System.Threading;
using System.Threading.Tasks;
using ExampleSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SolidNetsEasyClient.Builder;
using SolidNetsEasyClient.Clients;
using SolidNetsEasyClient.Encryption;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace ExampleSite.Controllers;

public class CheckoutController : Controller
{
    private readonly PaymentClient client;
    private readonly string webhookUrl;
    private readonly string signingKey;

    public CheckoutController(PaymentClient client, IOptions<MyOptions> options)
    {
        this.client = client;
        webhookUrl = options.Value.WebhookCallbackUrl;
        signingKey = options.Value.MySigningKey;
    }

    [HttpPost("/checkout")]
    public async Task<ActionResult> Index(BasketViewModel basket, CancellationToken cts)
    {
        var order = PaymentRequestHelper.MinimalOrderExample(basket.Item, basket.Quantity);
        var paymentBuilder = NetsPaymentBuilder
            .CreatePayment(order)
            .EmbedCheckoutOnMyPage()
            .WithPrivateCustomer(
                customerId: "jdoe",
                firstName: "John",
                lastName: "Doe",
                email: "john@doe.com",
                new()
                {
                    Number = "54545454",
                    Prefix = "+45"
                },
                retypeCostumerData: false
            )
            .ChargePaymentOnCreation(false)
            .SubscribeToEvent(EventName.ChargeCreated, webhookUrl, order.SignOrder(signingKey))
            .SubscribeToEvent(EventName.ChargeFailed, webhookUrl, order.SignOrder(signingKey))
            .SubscribeToEvent(EventName.CheckoutCompleted, webhookUrl, order.SignOrder(signingKey))
            .SubscribeToEvent(EventName.PaymentCreated, webhookUrl, order.SignOrder(signingKey))
            .SubscribeToEvent(EventName.RefundCompleted, webhookUrl, order.SignOrder(signingKey))
            .SubscribeToEvent(EventName.RefundFailed, webhookUrl, order.SignOrder(signingKey))
            .SubscribeToEvent(EventName.RefundInitiated, webhookUrl, order.SignOrder(signingKey))
            .SubscribeToEvent(EventName.ReservationCancellationFailed, webhookUrl, order.SignOrder(signingKey))
            .SubscribeToEvent(EventName.ReservationCancelled, webhookUrl, order.SignOrder(signingKey))
            .SubscribeToEvent(EventName.ReservationCreatedV1, webhookUrl, order.SignOrder(signingKey))
            .SubscribeToEvent(EventName.ReservationCreatedV2, webhookUrl, order.SignOrder(signingKey))
            .SubscribeToEvent(EventName.ReservationFailed, webhookUrl, order.SignOrder(signingKey));

        var payment = await client.CreatePaymentAsync(paymentBuilder.BuildPaymentRequest(), cts);
        var vm = new CheckoutViewModel
        {
            CheckoutKey = client.CheckoutKey,
            PaymentID = payment.PaymentId.ToString("N")
        };
        return View(vm);
    }
}
