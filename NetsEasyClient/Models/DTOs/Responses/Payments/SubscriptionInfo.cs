using System;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Payments;

/// <summary>
/// A subscription information
/// </summary>
public record SubscriptionInfo
{
    /// <summary>
    /// The subscription identifier (a UUID)
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("id")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid? Id { get; init; }
}
