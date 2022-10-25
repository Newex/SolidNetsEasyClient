using System;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models;

/// <summary>
/// Defines the duration and interval when creating or updating a subscription
/// </summary>
public record Subscription
{
    /// <summary>
    /// The identifier of the subscription to be updated. If omitted, a new subscription will be created
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("subscriptionId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid? SubscriptionId { get; init; }

    /// <summary>
    /// The date and time when the subscription expires. It is not possible to charge this subscription after this date
    /// </summary>
    /// <remarks>
    /// he field has three components: date, time, and time zone (offset from GMT). For example: 2021-07-02T12:00:00.0000+02:00
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("endDate")]
    public DateTimeOffset? EndDate { get; init; }

    /// <summary>
    /// Defines the minimum number of days between each recurring charge. This interval commences from either the day the subscription was created or the most recent subscription charge, whichever is later
    /// </summary>
    /// <remarks>
    /// An interval value of 0 means that there are no payment interval restrictions
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("interval")]
    public int? Interval { get; init; }
}
