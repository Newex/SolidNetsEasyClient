using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Requests.Webhooks;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;

/// <summary>
/// Bulk charge result
/// </summary>
public record BulkCharge
{
    /// <summary>
    /// A string that uniquely identifies the bulk charge operation. Use this property for enabling safe retries. Must be between 1 and 64 characters.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("externalBulkChargeId")]
    public string? ExternalBulkChargeId { get; init; }

    /// <summary>
    /// Notifications allow you to subscribe to status updates for a payment.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("notifications")]
    public Notification? Notifications { get; init; }

    /// <summary>
    /// The array of subscriptions that should be charged. Each item in the array should define either a subscriptionId or an externalReference, but not both.
    /// </summary>
    [JsonPropertyName("subscriptions")]
    public IList<SubscriptionCharge> Subscriptions { get; init; } = [];
}