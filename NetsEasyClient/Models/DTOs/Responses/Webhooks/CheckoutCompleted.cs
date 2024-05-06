using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// A web hook DTO for the checkout completed event
/// </summary>
public record CheckoutCompleted : Webhook<CheckoutCompletedData>
{

    /// <summary>
    /// The data associated with this event
    /// </summary>
    [Required]
    [JsonPropertyName("data")]
    public override CheckoutCompletedData Data { get; init; } = new();
}
