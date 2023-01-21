using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;

/// <summary>
/// Should define either a subscriptionId or an externalReference, but not both.
/// </summary>
public record SubscriptionCharge : BaseSubscription
{
    /// <summary>
    /// Specifies an order associated with a payment. An order must contain at least one order item. The amount of the order must match the sum of the specified order items.
    /// </summary>
    [Required]
    [JsonPropertyName("order")]
    public required Order Order { get; init; }
}
