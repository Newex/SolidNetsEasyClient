using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// The payment.refund.completed event is triggered when a refund has successfully been completed.
/// </summary>
public record RefundCompleted : Webhook<RefundCompletedData>
{
    /// <summary>
    /// The data associated with this event
    /// </summary>
    [Required]
    [JsonPropertyName("data")]
    public override RefundCompletedData Data { get; init; } = new();
}
