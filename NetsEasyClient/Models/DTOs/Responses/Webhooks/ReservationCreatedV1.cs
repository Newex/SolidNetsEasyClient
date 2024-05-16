using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// A web hook payment reservation created event DTO
/// </summary>
public record ReservationCreatedV1 : Webhook<ReservationCreatedDataV1>
{
    /// <summary>
    /// The data associated with this event
    /// </summary>
    [Required]
    [JsonPropertyName("data")]
    public override ReservationCreatedDataV1 Data { get; init; } = new();
}
