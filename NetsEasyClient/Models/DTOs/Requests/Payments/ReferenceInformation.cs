using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Payments;

/// <summary>
/// Reference information
/// </summary>
public record ReferenceInformation
{
    /// <summary>
    /// The checkout url
    /// The maximum length is 256 characters.
    /// </summary>
    [JsonPropertyName("checkoutUrl")]
    public required string CheckoutUrl { get; init; }

    /// <summary>
    /// The payment reference.
    /// The maximum length is 128 characters.
    /// </summary>
    [JsonPropertyName("reference")]
    public required string Reference { get; init; }
}
