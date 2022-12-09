using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Requests.Webhooks;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;

/// <summary>
/// Bulk unscheduled subscriptions to be charged
/// </summary>
public record BulkUnscheduledSubscriptionCharge
{
    /// <summary>
    /// A string that uniquely identifies the bulk charge operation. Use this property for enabling safe retries. Must be between 1 and 64 characters.
    /// </summary>
    [JsonPropertyName("externalBulkChargeId")]
    public string ExternalBulkChargeId { get; init; } = string.Empty;

    /// <summary>
    /// Notifications allow you to subscribe to status updates for a payment.
    /// </summary>
    [JsonPropertyName("notifications")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Notification? Notifications { get; init; }

    /// <summary>
    /// The array of unscheduled subscriptions that should be charged. Each item in the array should define either a subscriptionId or an externalReference, but not both.
    /// </summary>
    [Required]
    [JsonPropertyName("unscheduledSubscriptions")]
    public IList<ChargeUnscheduledSubscription> UnscheduledSubscriptions { get; init; } = Enumerable.Empty<ChargeUnscheduledSubscription>().ToList();
}