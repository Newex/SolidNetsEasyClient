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
            .AsUnscheduledSubscription(true)
            .AsRegularSubscription(interval: 0, years: 5, onTheEndOfTheMonth: true, onMidnight: true)
            .SubscribeToEvent(EventName.ChargeCreated, webhookUrl)
            .SubscribeToEvent(EventName.ChargeFailed, webhookUrl)
            .SubscribeToEvent(EventName.CheckoutCompleted, webhookUrl)
            .SubscribeToEvent(EventName.PaymentCreated, webhookUrl)
            .SubscribeToEvent(EventName.RefundCompleted, webhookUrl)
            .SubscribeToEvent(EventName.RefundFailed, webhookUrl)
            .SubscribeToEvent(EventName.RefundInitiated, webhookUrl)
            .SubscribeToEvent(EventName.ReservationCancellationFailed, webhookUrl)
            .SubscribeToEvent(EventName.ReservationCancelled, webhookUrl)
            .SubscribeToEvent(EventName.ReservationCreatedV1, webhookUrl)
            .SubscribeToEvent(EventName.ReservationCreatedV2, webhookUrl)
            .SubscribeToEvent(EventName.ReservationFailed, webhookUrl);

        var payment = await client.CreatePaymentAsync(paymentBuilder.BuildPaymentRequest(), cts);
        var vm = new CheckoutViewModel
        {
            CheckoutKey = client.CheckoutKey,
            PaymentID = payment.PaymentId.ToString("N")
        };
        return View(vm);
    }
}
