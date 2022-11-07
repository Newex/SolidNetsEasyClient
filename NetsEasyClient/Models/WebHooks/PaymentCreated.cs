using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.WebHooks;

/// <summary>
/// The <see cref="EventNames.Payment.PaymentCreated"/> event is triggered when a new payment is created. This happens when the customer hits the "Pay" button on the checkout page.
/// </summary>
public record PaymentCreated
{
    /// <summary>
    /// A unique identifier of this event. You can use this identifier to detect whether this event is new or has already been handled by you.
    /// </summary>
    [JsonConverter(typeof(GuidTypeConverter))]
    [JsonPropertyName("id")]
    [Required]
    public Guid Id { get; init; }

    /// <summary>
    /// The merchant number.
    /// </summary>
    [Required]
    [JsonPropertyName("merchantId")]
    public int MerchantId { get; init; }

    /// <summary>
    /// The time at which the event occurred formatted according to RFC339, for example 2021-03-23T15:30:55.23Z.
    /// </summary>
    [Required]
    [JsonPropertyName("timestamp")]
    [JsonConverter(typeof(DateTimeOffsetConverter))]
    public DateTimeOffset Timestamp { get; init; }

    /// <summary>
    /// The name of the event, in this case <see cref="EventNames.Payment.PaymentCreated"/>
    /// </summary>
    [Required]
    [JsonPropertyName("event")]
    public EventNames Event { get; init; } = EventNames.Payment.PaymentCreated;

    /// <summary>
    /// The data associated with this event.
    /// </summary>
    [Required]
    [JsonPropertyName("data")]
    public CreatedData Data { get; init; } = new();
}

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
    public CreatedAmount Amount { get; init; } = new();

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

/// <summary>
/// The amount of the charge.
/// </summary>
public record CreatedAmount
{
    /// <summary>
    /// The amount, for example 10000
    /// </summary>
    [Required]
    [JsonPropertyName("amount")]
    public int Amount { get; init; }

    /// <summary>
    /// The currency, for example 'SEK'.
    /// </summary>
    [Required]
    [JsonPropertyName("currency")]
    public string Currency { get; init; } = string.Empty;
}