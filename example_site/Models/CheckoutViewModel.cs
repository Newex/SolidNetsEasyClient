using System;

namespace ExampleSite.Models;

public record class CheckoutViewModel
{
    public string CheckoutKey { get; init; } = string.Empty;
}
