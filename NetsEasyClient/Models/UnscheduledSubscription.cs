using System;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models;

/// <summary>
/// Defines the payment as one that should initiate or update an unscheduled card on file agreement
/// </summary>
public record UnscheduledSubscription
{
    /// <summary>
    /// A flag indicating if a new unscheduled card on file agreement should be created. Can be omitted when updating an existing unscheduled card on file agreement.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("create")]
    public bool? Create { get; init; }

    /// <summary>
    /// The identifier of the unscheduled card on file agreement to be updated. If omitted, a new unscheduled card on file agreement will be created
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("unscheduledSubscriptionId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid? UnscheduledSubscriptionId { get; init; }
}
