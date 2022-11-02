using System;

namespace ExampleSite.Models;

public record class CheckoutViewModel
{
    public string CheckoutKey { get; init; } = string.Empty;
    public string PaymentID { get; set; } = string.Empty;
}
