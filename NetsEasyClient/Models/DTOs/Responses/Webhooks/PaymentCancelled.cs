using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// The webhook DTO for the when a payment has been cancelled
/// </summary>
/// <remarks>
/// The payment.cancel.created event is triggered when a reservation has been canceled.
/// </remarks>
public record PaymentCancelled : Webhook<PaymentCancelledData>
{
    /// <summary>
    /// The data associated with this event
    /// </summary>
    [Required]
    [JsonPropertyName("data")]
    public override PaymentCancelledData Data { get; init; } = new();
}
