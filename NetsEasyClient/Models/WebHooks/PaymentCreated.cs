using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.WebHooks;

/// <summary>
/// The <see cref="EventNames.Payment.PaymentCreated"/> event is triggered when a new payment is created. This happens when the customer hits the "Pay" button on the checkout page.
/// </summary>
public record PaymentCreated : Webhook<CreatedData> { }

/// <summary>
/// The data associated with this event.
/// </summary>
public record CreatedData
{
    /// <summary>
    /// The payment identifier.
    /// </summary>
    [JsonConverter(typeof(GuidTypeConverter))]
    [JsonPropertyName("paymentId")]
    public Guid PaymentId { get; init; }

    /// <summary>
    /// The complete order associated with the payment.
    /// </summary>
    [Required]
    [JsonPropertyName("order")]
    public CreatedOrder Order { get; init; } = new();
}

/// <summary>
/// The created order details
/// </summary>
public record CreatedOrder
{
    /// <summary>
    /// The amount of the charge.
    /// </summary>
    [JsonPropertyName("amount")]
    public WebhookAmount Amount { get; init; } = new();

    /// <summary>
    /// A reference to recognize this order. Usually a number sequence (order number).
    /// </summary>
    [JsonPropertyName("reference")]
    public string Reference { get; init; } = string.Empty;

    /// <summary>
    /// The list of order items that are associated with the charge. Contains at least one order item.
    /// </summary>
    [Required]
    [JsonPropertyName("orderItems")]
    public IList<Item> OrderItems { get; init; } = new List<Item>();
}