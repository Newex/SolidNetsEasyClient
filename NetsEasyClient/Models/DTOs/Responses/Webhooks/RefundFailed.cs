using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// The payment.refund.failed event is triggered when a refund attempt has failed.
/// </summary>
public record RefundFailed : Webhook<RefundFailedData>
{
    /// <summary>
    /// The data associated with this event
    /// </summary>
    [Required]
    [JsonPropertyName("data")]
    public override RefundFailedData Data { get; init; } = new();
}