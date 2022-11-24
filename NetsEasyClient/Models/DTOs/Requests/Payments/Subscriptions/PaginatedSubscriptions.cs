using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;

/// <summary>
/// A page of subscription statusses
/// </summary>
public record PaginatedSubscriptions
{
    /// <summary>
    /// A page of subscription statusses
    /// </summary>
    [JsonPropertyName("page")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IList<SubscriptionProcessStatus>? Page { get; init; }

    /// <summary>
    /// Indicates whether there are more subscriptions beyond the requested range.
    /// </summary>
    [JsonPropertyName("more")]
    public bool More { get; init; }

    /// <summary>
    /// Indicates whether the operation has completed or is still processing subscriptions. Possible values are 'Done' and 'Processing'.
    /// </summary>
    [JsonPropertyName("status")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BulkStatus? Status { get; init; }

    /// <summary>
    /// The payment identifier (a UUID).
    /// </summary>
    [JsonPropertyName("paymentId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? PaymentId { get; init; }
}