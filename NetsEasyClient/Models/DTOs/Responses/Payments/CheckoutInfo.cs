using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Payments;

/// <summary>
/// The checkout information
/// </summary>
public record CheckoutInfo
{
    /// <summary>
    /// The URL to the hosted or embedded checkout page
    /// </summary>
    [Required]
    [JsonPropertyName("url")]
    public string Url { get; init; } = string.Empty;

    /// <summary>
    /// The URL to the page responsible for handling a canceled checkout
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("cancelUrl")]
    public string? CancelUrl { get; init; }
}
