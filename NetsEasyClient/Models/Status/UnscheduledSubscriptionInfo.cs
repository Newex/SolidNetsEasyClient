using System;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.Status;

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
}
