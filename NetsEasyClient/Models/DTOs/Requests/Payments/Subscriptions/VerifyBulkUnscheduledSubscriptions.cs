using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;

/// <summary>
/// Bulk request to verify cards for unscheduled subscriptions
/// </summary>
public record VerifyBulkUnscheduledSubscriptions
{
    /// <summary>
    /// A string that uniquely identifies the verification operation. Use this property for enabling safe retries. Must be between 1 and 64 characters.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("externalBulkVerificationId")]
    public string? ExternalBulkVerificationId { get; init; }

    /// <summary>
    /// The set of unscheduled subscriptions that should be verified. Each item in the array should define either a unscheduledSubscriptionId or an externalReference, but not both.
    /// </summary>
    [JsonPropertyName("unscheduledSubscriptions")]
    public IList<UnscheduledSubscriptionInfo> UnscheduledSubscriptions { get; init; } = Enumerable.Empty<UnscheduledSubscriptionInfo>().ToList();
}