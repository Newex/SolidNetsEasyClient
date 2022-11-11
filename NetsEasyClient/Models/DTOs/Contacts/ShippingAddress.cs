using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Contacts;

/// <summary>
/// The address of a customer (private or business)
/// </summary>
public record ShippingAddress
{
    /// <summary>
    /// The primary address line
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("addressLine1")]
    public string? AddressLine1 { get; init; }

    /// <summary>
    /// The additional address line
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("addressLine2")]
    public string? AddressLine2 { get; init; }

    /// <summary>
    /// The postal code
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("postalCode")]
    public string? PostalCode { get; init; }

    /// <summary>
    /// The city
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("city")]
    public string? City { get; init; }

    /// <summary>
    /// The country
    /// </summary>
    /// <remarks>
    /// A three-letter country code (ISO 3166-1), for example GBR
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("country")]
    public string? Country { get; init; }
}
