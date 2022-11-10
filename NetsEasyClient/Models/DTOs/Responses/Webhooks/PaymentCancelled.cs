using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// The webhook DTO for the when a payment has been cancelled
/// </summary>
/// <remarks>
/// The payment.cancel.created event is triggered when a reservation has been canceled.
/// </remarks>
public record PaymentCancelled : Webhook<PaymentCancelledData> { }

/// <summary>
/// The data associated with this event.
/// </summary>
public record PaymentCancelledData
{
    /// <summary>
    /// The payment identifier
    /// </summary>
    [Required]
    [JsonConverter(typeof(GuidTypeConverter))]
    [JsonPropertyName("paymentId")]
    public Guid PaymentId { get; init; }

    /// <summary>
    /// The cancellation id
    /// </summary>
    [Required]
    [JsonConverter(typeof(GuidTypeConverter))]
    [JsonPropertyName("cancelId")]
    public Guid CancelId { get; init; }

    /// <summary>
    /// The list of order items that are associated with the canceled payment. Contains at least one order item.
    /// </summary>
    [Required]
    [JsonPropertyName("orderItems")]
    public IList<Item> OrderItems { get; init; } = new List<Item>();

    /// <summary>
    /// The amount of the charge.
    /// </summary>
    [Required]
    [JsonPropertyName("amount")]
    public WebhookAmount Amount { get; init; } = new();
}