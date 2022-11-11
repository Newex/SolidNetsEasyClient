using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// The payment.charge.created.v2 event is triggered when the customer has successfully been charged, partially or fully. A use case can be to get notified for successful subscription or unscheduled charges.
/// </summary>
public record ChargeCreated : Webhook<ChargedData>
{
    /// <summary>
    /// The merchant number.
    /// </summary>
    /// <remarks>
    /// The Nets specification has 2 different merchant ids, which has been mapped to the same value in this library
    /// </remarks>
    [Required]
    [JsonPropertyName("merchantNumber")]
    public int MerchantNumber
    {
        get { return MerchantId; }
        init { MerchantId = value; }
    }
}