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
        return RedirectToAction("Pay", new
        {
            paymentId = payment.PaymentId.ToString("N"),
        });
    }

    [HttpGet("/checkout")]
    public ActionResult Pay([FromQuery] string paymentId)
    {
        var vm = new CheckoutViewModel
        {
            CheckoutKey = client.CheckoutKey,
        };

        return View(vm);
    }
}
