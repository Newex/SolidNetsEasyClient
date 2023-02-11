using System.Threading;
using System.Threading.Tasks;
using ExampleSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SolidNetsEasyClient.Builder;
using SolidNetsEasyClient.Clients;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace ExampleSite.Controllers;

public class CheckoutController : Controller
{
    private readonly PaymentClient client;
    private readonly NetsPaymentFactory paymentFactory;
    private readonly NetsNotificationFactory notificationFactory;
    private readonly ILogger<CheckoutController> logger;

    public CheckoutController(
        PaymentClient client,
        NetsPaymentFactory paymentFactory,
        NetsNotificationFactory notificationFactory,
        ILogger<CheckoutController>? logger = null)
    {
        this.client = client;
        this.paymentFactory = paymentFactory;
        this.notificationFactory = notificationFactory;
        this.logger = logger ?? NullLogger<CheckoutController>.Instance;
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
            .SubscribeToEvent(EventName.PaymentCreated)
            .SubscribeToEvent(EventName.ReservationCreatedV1)
            .SubscribeToEvent(EventName.ReservationCreatedV2)
            .SubscribeToEvent(EventName.ReservationFailed)
            .SubscribeToEvent(EventName.CheckoutCompleted)
            .SubscribeToEvent(EventName.ChargeCreated)
            .SubscribeToEvent(EventName.ChargeFailed)
            .SubscribeToEvent(EventName.RefundInitiated)
            .SubscribeToEvent(EventName.RefundFailed)
            .SubscribeToEvent(EventName.RefundCompleted)
            .SubscribeToEvent(EventName.PaymentCancelled)
            .SubscribeToEvent(EventName.PaymentCancellationFailed)
            .BuildPaymentRequest();

        var notifications = notificationFactory.CreateNotificationBuilder()
            .AddNotificationForSingleEvent(EventName.PaymentCreated, order, routeName: "CustomPaymentCreated", routeValues: new { number = 42 })
            .Build();
        logger.LogTrace("Notifications unused: {Notifications}", notifications);

        var payment = await client.CreatePaymentAsync(paymentRequest, cts);
        var vm = new CheckoutViewModel
        {
            CheckoutKey = client.CheckoutKey,
            PaymentID = payment.PaymentId.ToString("N")
        };
        return View(vm);
    }
}
