using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Orders;

/// <summary>
/// The order cancellation request
/// </summary>
/// <remarks>
/// Only full cancels are allowed. The amount must match the total amount of the
/// order.
/// Once a payment has been charged, it cannot be cancelled.
/// A cancelled payment cannot change status.
/// Nexi Group will not charge a fee for a canceled payment.
/// </remarks>
public record CancelOrder
{
    /// <summary>
    /// The amount to be canceled.
    /// Must be higher than 0.
    /// </summary>
    [JsonPropertyName("amount")]
    public required int Amount { get; init; }

    /// <summary>
    /// The order items to be canceled. Note! Since only full cancels are
    /// currently supported, you need to provide all order items or completely
    /// avoid specifying any order items.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("orderItems")]
    public IList<Item>? OrderItems { get; init; }
}
