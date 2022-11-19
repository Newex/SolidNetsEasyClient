using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Requests.Webhooks;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Payments;

public record BulkCharge
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("externalBulkChargeId")]
    public string? ExternalBulkChargeId { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("notifications")]
    public Notification? Notifications { get; init; }

    [JsonPropertyName("subscriptions")]
    public IList<SubscriptionCharge> Subscriptions { get; init; } = Enumerable.Empty<SubscriptionCharge>().ToList();
}