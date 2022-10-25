using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models;

/// <summary>
/// Order updates
/// </summary>
public record OrderUpdate
{
    /// <summary>
    /// The amount, for example 10000
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("amount")]
    public int? Amount { get; init; }

    /// <summary>
    /// The array of order items
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("items")]
    public IEnumerable<Item>? Items { get; init; }

    /// <summary>
    /// The shipping
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("shipping")]
    public OrderShippingUpdate? Shipping { get; init; }

    /// <summary>
    /// Specifies an array of invoice fees added to the total price when invoice is used as the payment method
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("paymentMethods")]
    public IEnumerable<PaymentMethod>? PaymentMethods { get; init; }
}
