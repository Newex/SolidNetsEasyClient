using System;
using Microsoft.AspNetCore.Mvc;
using SolidNetsEasyClient.Clients;

namespace ExampleSite.Controllers;

public class PaymentController : Controller
{
    private readonly PaymentClient paymentClient;

    public PaymentController(PaymentClient paymentClient)
    {
        this.paymentClient = paymentClient;
    }

    [HttpGet("/pay")]
    public IActionResult Pay()
    {
        return View();
    }
}
