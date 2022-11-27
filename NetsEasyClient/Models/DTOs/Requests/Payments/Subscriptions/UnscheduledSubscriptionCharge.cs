using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Requests.Webhooks;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;

/// <summary>
/// An unscheduled subscription charge request
/// </summary>
public record UnscheduledSubscriptionCharge
{
    /// <summary>
    /// Specifies an order associated with a payment. An order must contain at least one order item. The amount of the order must match the sum of the specified order items.
    /// </summary>
    [Required]
    [JsonPropertyName("order")]
    public Order Order { get; init; } = new();

    /// <summary>
    /// Notifications allow you to subscribe to status updates for a payment.
    /// </summary>
    [JsonPropertyName("notifications")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Notification? Notifications { get; init; }
}