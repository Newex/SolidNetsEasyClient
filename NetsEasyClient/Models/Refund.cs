using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models;

/// <summary>
/// A refund
/// </summary>
public record Refund
{
    /// <summary>
    /// The amount to refund
    /// </summary>
    [Required]
    [JsonPropertyName("amount")]
    public int Amount { get; init; }

    /// <summary>
    /// The order items
    /// </summary>
    /// <remarks>
    /// Required to supply the order items on a partial refund, which items are being refunded
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("amount")]
    public IEnumerable<Item>? OrderItems { get; init; }
}
