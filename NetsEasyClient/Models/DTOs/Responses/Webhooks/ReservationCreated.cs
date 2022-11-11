using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// A web hook payment reservation created event DTO
/// </summary>
public record ReservationCreated : Webhook<ReservationCreatedData>
{
    /// <summary>
    /// The merchant Id
    /// </summary>
    [JsonPropertyName("merchantNumber")]
    public int MerchantNumber
    {
        get { return MerchantId; }
        init { MerchantId = value; }
    }
}
