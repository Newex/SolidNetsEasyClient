using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

/// <summary>
/// The consumer reservation details
/// </summary>
public record ReservationCreatedConsumer
{
    /// <summary>
    /// The consumer IP
    /// </summary>
    [JsonPropertyName("ip")]
    public string IP { get; init; } = string.Empty;
}