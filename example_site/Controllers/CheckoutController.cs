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
        var payment = await client.CreatePaymentAsync(PaymentRequestHelper.MinimalOrderExample(basket.Item, basket.Quantity), Integration.EmbeddedCheckout, cts);
        var vm = new CheckoutViewModel
        {
            CheckoutKey = client.CheckoutKey,
            PaymentID = payment.PaymentId.ToString("N")
        };
        return View(vm);
    }
}
