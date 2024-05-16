using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// The webhook DTO for when a payment has failed to be cancelled
/// </summary>
/// <remarks>
/// The payment.cancel.failed event is triggered when a cancellation of a reservation has failed.
/// </remarks>
public record PaymentCancellationFailed : Webhook<PaymentCancellationFailedData>
{
    /// <summary>
    /// The data associated with this event
    /// </summary>
    [Required]
    [JsonPropertyName("data")]
    public override PaymentCancellationFailedData Data { get; init; } = new();
}