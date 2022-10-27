using System;
using Microsoft.AspNetCore.Mvc;

namespace ExampleSite.Controllers;

public class CheckoutController : Controller
{
    [HttpPost("/checkout")]
    public ActionResult Index()
    {
        return View();
    }
}
