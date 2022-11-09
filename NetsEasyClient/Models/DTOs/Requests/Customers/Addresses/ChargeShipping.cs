using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Customers.Addresses;

/// <summary>
/// The shipping information when finalizing charge
/// </summary>
public record ChargeShipping
{
    /// <summary>
    /// The tracking number
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("trackingNumber")]
    public string? TrackingNumber { get; init; }

    /// <summary>
    /// The provider
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("provider")]
    public string? Provider { get; init; }
}
