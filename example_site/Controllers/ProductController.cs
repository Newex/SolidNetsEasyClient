using ExampleSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExampleSite.Controllers;

public class ProductController : Controller
{
    [HttpGet("/products")]
    public IActionResult Index()
    {
        var basket = new BasketViewModel();
        return View(basket);
    }
}
