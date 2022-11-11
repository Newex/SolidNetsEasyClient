using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

/// <summary>
/// The checkout completed address
/// </summary>
public record CheckoutCompletedAddress
{
    /// <summary>
    /// The first address line
    /// </summary>
    [JsonPropertyName("addressLine1")]
    public string AddressLine1 { get; init; } = string.Empty;

    /// <summary>
    /// The second address line
    /// </summary>
    [JsonPropertyName("addressLine2")]
    public string AddressLine2 { get; init; } = string.Empty;

    /// <summary>
    /// The city
    /// </summary>
    [JsonPropertyName("city")]
    public string City { get; init; } = string.Empty;

    /// <summary>
    /// The country
    /// </summary>
    [JsonPropertyName("country")]
    public string Country { get; init; } = string.Empty;

    /// <summary>
    /// The post code
    /// </summary>
    [JsonPropertyName("postcode")]
    public string PostCode { get; init; } = string.Empty;

    /// <summary>
    /// The receiver line
    /// </summary>
    [JsonPropertyName("receiverLine")]
    public string ReceiverLine { get; init; } = string.Empty;
}