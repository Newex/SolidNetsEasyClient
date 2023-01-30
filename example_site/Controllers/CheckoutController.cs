using System.Threading;
using System.Threading.Tasks;
using ExampleSite.Models;
using Microsoft.AspNetCore.Mvc;
using SolidNetsEasyClient.Builder;
using SolidNetsEasyClient.Clients;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace ExampleSite.Controllers;

public class CheckoutController : Controller
{
    private readonly PaymentClient client;
    private readonly NetsPaymentFactory paymentFactory;

    public CheckoutController(PaymentClient client, NetsPaymentFactory paymentFactory)
    {
        this.client = client;
        this.paymentFactory = paymentFactory;
    }

    [HttpPost("/checkout")]
    public async Task<ActionResult> Index(BasketViewModel basket, CancellationToken cts)
    {
        var order = PaymentRequestHelper.MinimalOrderExample(basket.Item, basket.Quantity);

        var paymentBuilder = paymentFactory.CreatePaymentBuilder(order);
        var paymentRequest = paymentBuilder
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
            .SubscribeToEvent(EventName.PaymentCreated, Url)
            .SubscribeToEvent(EventName.ReservationCreatedV1, Url)
            .SubscribeToEvent(EventName.ReservationCreatedV2, Url)
            .SubscribeToEvent(EventName.ReservationFailed, Url)
            .SubscribeToEvent(EventName.CheckoutCompleted, Url)
            .SubscribeToEvent(EventName.ChargeCreated, Url)
            .SubscribeToEvent(EventName.ChargeFailed, Url)
            .SubscribeToEvent(EventName.RefundInitiated, Url)
            .SubscribeToEvent(EventName.RefundFailed, Url)
            .SubscribeToEvent(EventName.RefundCompleted, Url)
            .SubscribeToEvent(EventName.PaymentCancelled, Url)
            .SubscribeToEvent(EventName.PaymentCancellationFailed, Url)
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
