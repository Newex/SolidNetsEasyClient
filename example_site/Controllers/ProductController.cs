using System;
using ExampleSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExampleSite.Controllers;

public class ProductController : Controller
{
    [HttpGet("/products")]
    public IActionResult Index()
    {
        var cola = new ProductCola();
        return View(cola);
    }
}
