using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;

/// <summary>
/// Unscheduled subscription
/// </summary>
public record ChargeUnscheduledSubscription : UnscheduledSubscriptionInfo
{
    /// <summary>
    /// Specifies an order associated with a payment. An order must contain at least one order item. The amount of the order must match the sum of the specified order items.
    /// </summary>
    [Required]
    [JsonPropertyName("order")]
    public Order Order { get; init; } = new();
}
