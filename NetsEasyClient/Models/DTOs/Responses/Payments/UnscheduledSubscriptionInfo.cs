using System;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Payments;

/// <summary>
/// An Unscheduled subscription info
/// </summary>
public record UnscheduledSubscriptionInfo
{
    /// <summary>
    /// The unscheduled subscription identifier (a UUID)
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("unscheduledSubscriptionId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid? UnscheduledSubscriptionId { get; init; }

    /// <summary>
    /// An external reference to identify a set of imported subscriptions. This
    /// parameter is only used if your subscriptions have been imported from a
    /// payment platform other than Nets Easy.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("externalReference")]
    public string? ExternalReference { get; init; }
}
