using System;

namespace ExampleSite.Models;

public record BasketViewModel
{
    public ProductCola Item { get; set; } = new();
    public int Quantity { get; set; }
}
