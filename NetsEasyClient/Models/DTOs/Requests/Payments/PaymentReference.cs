using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Payments;

/// <summary>
/// Updates myReference field on payment. The myReference can be used if you 
/// want to create a myReference ID that can be used in your own accounting 
/// system to keep track of the actions connected to the payment. 
/// </summary>
public record PaymentReference
{
    /// <summary>
    /// Merchant payment reference. 
    /// The maximum length is 36 characters.
    /// </summary>
    [JsonPropertyName("myReference")]
    public string? MyReference { get; init; }
}
