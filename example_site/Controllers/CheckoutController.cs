using System.Threading;
using System.Threading.Tasks;
using ExampleSite.Models;
using Microsoft.AspNetCore.Mvc;
using SolidNetsEasyClient.Clients;
using SolidNetsEasyClient.Models;

namespace ExampleSite.Controllers;

public class CheckoutController : Controller
{
    private readonly PaymentClient client;

    public CheckoutController(PaymentClient client)
    {
        this.client = client;
    }

    [HttpPost("/checkout")]
    public async Task<ActionResult> Index(BasketViewModel basket, CancellationToken cts)
    {
        var order = PaymentRequestHelper.MinimalOrderExample(basket.Item, basket.Quantity);
        var customer = new Consumer
        {
            Email = "john@doe.com",
            PhoneNumber = new()
            {
                Number = "54545454",
                Prefix = "+45"
            },
            PrivatePerson = new()
            {
                FirstName = "John",
                LastName = "Doe"
            },
            Reference = "jdoe",
            // ShippingAddress = new()
            // {
            //     AddressLine1 = "Rådhuspladsen 1",
            //     City = "København",
            //     Country = "DNK",
            //     PostalCode = "1599"
            // },
        };
        var payment = await client.CreatePaymentAsync(order, customer, cts);
        var vm = new CheckoutViewModel
        {
            CheckoutKey = client.CheckoutKey,
            PaymentID = payment.PaymentId.ToString("N")
        };
        return View(vm);
    }
}
