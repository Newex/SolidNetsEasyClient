using System;
using System.Threading;
using System.Threading.Tasks;
using ExampleSite.Models;
using Microsoft.AspNetCore.Mvc;
using SolidNetsEasyClient.Clients;

namespace ExampleSite.Controllers;

public class CheckoutController : Controller
{
    private readonly PaymentClient client;

    public CheckoutController(PaymentClient client)
    {
        this.client = client;
    }

    [HttpPost("/checkout")]
    public async Task<ActionResult> Index(CancellationToken cts)
    {
        var payment = await client.CreatePaymentAsync(PaymentRequestHelper.MinimalOrderExample(new()), cts);
        var vm = new CheckoutViewModel
        {
            PaymentID = payment.PaymentId,
            CheckoutKey = client.CheckoutKey
        };

        return View(vm);
    }
}
