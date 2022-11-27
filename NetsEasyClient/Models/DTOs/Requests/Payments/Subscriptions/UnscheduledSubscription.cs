using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;

/// <summary>
/// Unscheduled subscription
/// </summary>
public record UnscheduledSubscription
{
    /// <summary>
    /// The subscription identifier (a UUID) returned from the Retrieve payment method.
    /// </summary>
    [JsonPropertyName("unscheduledSubscriptionId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid? UnscheduledSubscriptionId { get; init; }

    /// <summary>
    /// An external reference to identify a set of imported subscriptions. This parameter is only used if your subscriptions have been imported from a payment platform other than Nets Easy.
    /// </summary>
    [JsonPropertyName("externalReference")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ExternalReference { get; init; }

    /// <summary>
    /// Specifies an order associated with a payment. An order must contain at least one order item. The amount of the order must match the sum of the specified order items.
    /// </summary>
    [Required]
    [JsonPropertyName("order")]
    public Order Order { get; init; } = new();
}
