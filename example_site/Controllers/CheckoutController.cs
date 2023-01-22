using System.Threading;
using System.Threading.Tasks;
using ExampleSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SolidNetsEasyClient.Builder;
using SolidNetsEasyClient.Clients;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace ExampleSite.Controllers;

public class CheckoutController : Controller
{
    private readonly PaymentClient client;
    private readonly string webhookUrl;

    public CheckoutController(PaymentClient client, IOptions<MyOptions> options)
    {
        this.client = client;
        webhookUrl = options.Value.WebhookCallbackUrl;
    }

    [HttpPost("/checkout")]
    public async Task<ActionResult> Index(BasketViewModel basket, CancellationToken cts)
    {
        var order = PaymentRequestHelper.MinimalOrderExample(basket.Item, basket.Quantity);
        const string authorizationHeader = "Abc123DE";
        var paymentRequest = NetsPaymentBuilder
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
            .AsSinglePayment()
            .SubscribeToEvent(EventName.ChargeCreated, webhookUrl, authorizationHeader)
            .SubscribeToEvent(EventName.ChargeFailed, webhookUrl, authorizationHeader)
            .SubscribeToEvent(EventName.CheckoutCompleted, webhookUrl, authorizationHeader)
            .SubscribeToEvent(EventName.PaymentCreated, webhookUrl, authorizationHeader)
            .SubscribeToEvent(EventName.RefundCompleted, webhookUrl, authorizationHeader)
            .SubscribeToEvent(EventName.RefundFailed, webhookUrl, authorizationHeader)
            .SubscribeToEvent(EventName.RefundInitiated, webhookUrl, authorizationHeader)
            .SubscribeToEvent(EventName.ReservationCancellationFailed, webhookUrl, authorizationHeader)
            .SubscribeToEvent(EventName.ReservationCancelled, webhookUrl, authorizationHeader)
            .SubscribeToEvent(EventName.ReservationCreatedV1, webhookUrl, authorizationHeader)
            .SubscribeToEvent(EventName.ReservationCreatedV2, webhookUrl, authorizationHeader)
            .SubscribeToEvent(EventName.ReservationFailed, webhookUrl, authorizationHeader)
            .BuildPaymentRequest();

        var payment = await client.CreatePaymentAsync(paymentRequest, cts);
        var vm = new CheckoutViewModel
        {
            CheckoutKey = client.CheckoutKey,
            PaymentID = payment.PaymentId.ToString("N")
        };
        return View(vm);
    }
}
