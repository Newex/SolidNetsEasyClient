using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Orders;

/// <summary>
/// Specifies an order associated with a payment
/// </summary>
public record Order
{
    /// <summary>
    /// A list of order items, at least one must be specified
    /// </summary>
    [Required]
    [JsonPropertyName("items")]
    public IEnumerable<Item> Items { get; init; } = Enumerable.Empty<Item>();

    /// <summary>
    /// The total amount of the order including VAT, if any.
    /// </summary>
    /// <remarks>
    /// Sum of all <see cref="Item.GrossTotalAmount"/>
    /// </remarks>
    [Required]
    [JsonPropertyName("amount")]
    public int Amount => Items.Sum(i => i.GrossTotalAmount);

    /// <summary>
    /// The currency of the payment
    /// </summary>
    /// <remarks>
    /// Example: 'SEK'
    /// </remarks>
    [Required]
    [JsonPropertyName("currency")]
    public string Currency { get; init; } = string.Empty;

    /// <summary>
    /// A reference to recognize this order
    /// </summary>
    /// <remarks>
    /// Usually a number sequence (order number)
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("reference")]
    public string? Reference { get; init; }
}
