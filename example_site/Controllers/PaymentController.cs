using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SolidNetsEasyClient.Clients;
using SolidNetsEasyClient.Models;
using SolidNetsEasyClient.Models.Requests;

namespace ExampleSite.Controllers;

public class PaymentController : Controller
{
    private readonly PaymentClient paymentClient;

    public PaymentController(PaymentClient paymentClient)
    {
        this.paymentClient = paymentClient;
    }

    [HttpGet("/pay")]
    public async Task<ActionResult> Pay(CancellationToken cts)
    {
        var payment = await paymentClient.CreatePaymentAsync(MinimalPaymentExample, cts);
        return View();
    }

    public static PaymentRequest MinimalPaymentExample => new()
    {
        Order = new Order
        {
            Currency = "DKK",
            Items = new List<Item>
                {
                    new()
                    {
                        Name = "Base service",
                        Quantity = 12,
                        Unit = "month",
                        UnitPrice = 80000,
                        Reference = Guid.NewGuid().ToString()
                    }
                },
            Reference = Guid.NewGuid().ToString()
        },
        Checkout = new()
        {
            Url = "https://my.checkout.url",
            TermsUrl = "https://my.terms.url",
            ReturnUrl = "https://return.to.me"
        },
    };
}
