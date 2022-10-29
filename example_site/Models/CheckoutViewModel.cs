using System;

namespace ExampleSite.Models;

public record class CheckoutViewModel
{
    public Guid PaymentID { get; init; }
    public string CheckoutKey { get; init; } = string.Empty;
}
