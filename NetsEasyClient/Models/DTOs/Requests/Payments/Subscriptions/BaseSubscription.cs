using System;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;

/// <summary>
/// A base subscription with a subscription id and an ewternal reference
/// </summary>
public record BaseSubscription
{
    /// <summary>
    /// The subscription identifier (a UUID) returned from the Retrieve payment method.
    /// </summary>
    [JsonPropertyName("subscriptionId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(GuidTypeConverter))]
    public virtual Guid? SubscriptionId { get; init; }

    /// <summary>
    /// An external reference to identify a set of imported subscriptions. This parameter is only used if your subscriptions have been imported from a payment platform other than Nets Easy.
    /// </summary>
    [JsonPropertyName("externalReference")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public virtual string? ExternalReference { get; init; }
}
