using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// A web hook payment reservation created event DTO
/// </summary>
public record ReservationCreatedV2 : Webhook<ReservationCreatedDataV2>
{
    /// <summary>
    /// The merchant Id
    /// </summary>
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
    public override ReservationCreatedDataV2 Data { get; init; } = new();
}
