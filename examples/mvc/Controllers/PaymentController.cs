using Microsoft.AspNetCore.Mvc;

namespace ExampleSite.Controllers;

public class PaymentController : Controller
{
    [HttpGet("/pay")]
    public ActionResult Pay()
    {
        return View();
    }
}