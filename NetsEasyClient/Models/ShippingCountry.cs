using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models;

/// <summary>
/// A shipping country
/// </summary>
public record ShippingCountry
{
    /// <summary>
    /// A three-letter country code (ISO 3166-1), for example GBR
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; init; }
}
