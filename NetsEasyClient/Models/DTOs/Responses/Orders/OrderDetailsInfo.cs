using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Orders;

/// <summary>
/// The order details information
/// </summary>
public record OrderDetailsInfo
{
    /// <summary>
    /// The total amount of the order, for example 10000
    /// </summary>
    [Required]
    [JsonPropertyName("amount")]
    public int Amount { get; init; }

    /// <summary>
    /// The currency of the payment, for example 'SEK'
    /// </summary>
    [Required]
    [JsonPropertyName("currency")]
    public string Currency { get; init; } = string.Empty;

    /// <summary>
    /// The reference to recognize this order. Usually a number sequence provided when creating or updating the payment
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("reference")]
    public string? Reference { get; init; }
}
