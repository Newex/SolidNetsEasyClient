using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;

/// <summary>
/// The amount
/// </summary>
public record WebhookAmount
{
    /// <summary>
    /// The amount, for example 100_00
    /// </summary>
    [Required]
    [JsonPropertyName("amount")]
    public int Amount { get; init; }

    /// <summary>
    /// The currency, for example 'SEK'.
    /// </summary>
    [Required]
    [JsonPropertyName("currency")]
    public Currency Currency { get; init; }
}
