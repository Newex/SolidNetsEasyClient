using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// The payment.refund.initiated.v2 event is triggered when a refund has been initiated.
/// </summary>
public record RefundInitiated : Webhook<RefundInitiatedData>
{
    /// <summary>
    /// The merchant number.
    /// </summary>
    [Required]
    [JsonPropertyName("merchantNumber")]
    public int MerchantNumber
    {
        get => MerchantId;
        init => MerchantId = value;
    }

    /// <summary>
    /// The data associated with this event
    /// </summary>
    [Required]
    [JsonPropertyName("data")]
    public override RefundInitiatedData Data { get; init; } = new();
}
