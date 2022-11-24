using ExampleSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExampleSite.Controllers;

public class ProductController : Controller
{
    public IActionResult Index()
    {
        var basket = new BasketViewModel();
        return View(basket);
    }
}
