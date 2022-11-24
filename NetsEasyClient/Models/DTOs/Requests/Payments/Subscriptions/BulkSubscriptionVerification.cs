using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;

/// <summary>
/// A bulk verification of subscriptions
/// </summary>
public record BulkSubscriptionVerification
{
    /// <summary>
    /// A string that uniquely identifies the verification operation. Use this property for enabling safe retries. Must be between 1 and 64 characters.
    /// </summary>
    [JsonPropertyName("externalBulkVerificationId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ExternalBulkVerificationId { get; init; }

    /// <summary>
    /// The set of subscriptions that should be verified. Each item in the array should define either a subscriptioId or an externalReference, but not both.
    /// </summary>
    [JsonPropertyName("subscriptions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IList<BaseSubscription>? Subscriptions { get; init; }
}