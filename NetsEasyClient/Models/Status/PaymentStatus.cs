using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.Status;

/// <summary>
/// Status of a payment request
/// </summary>
public record PaymentStatus
{
    /// <summary>
    /// The payment object
    /// </summary>
    [JsonPropertyName("payment")]
    public Payment Payment { get; init; } = new();
}
/// <summary>
/// The payment object
/// </summary>
public record Payment
{
    /// <summary>
    /// The payment result identifier (a UUID)
    /// </summary>
    [Required]
    [JsonPropertyName("paymentId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid PaymentId { get; init; }

    /// <summary>
    /// Summarizes the reserved, charged, refunded, and canceled amounts associated with a payment
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("summary")]
    public Summary? Summary { get; init; }

    /// <summary>
    /// The consumer information
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("consumer")]
    public ConsumerInfo? Consumer { get; init; }

    /// <summary>
    /// The payment details information
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("paymentDetails")]
    public PaymentDetailsInfo? PaymentDetails { get; init; }

    /// <summary>
    /// The order details information
    /// </summary>
    [Required]
    [JsonPropertyName("orderDetails")]
    public OrderDetailsInfo OrderDetails { get; init; } = new();

    /// <summary>
    /// The checkout information
    /// </summary>
    [Required]
    [JsonPropertyName("checkout")]
    public CheckoutInfo Checkout { get; init; } = new();

    /// <summary>
    /// The date and time when the payment was initiated
    /// </summary>
    [JsonPropertyName("created")]
    [JsonConverter(typeof(DateTimeOffsetConverter))]
    public DateTimeOffset Created { get; init; }

    /// <summary>
    /// An array of all the refunds associated with this payment
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("refunds")]
    public IEnumerable<RefundInfo>? Refunds { get; init; }

    /// <summary>
    /// An array of all the charges associated with this payment
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("charges")]
    public IEnumerable<ChargeStatus>? Charges { get; init; }

    /// <summary>
    /// The date and time of termination. Only present if the payment has been terminated
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("terminated")]
    public DateTimeOffset? Terminated { get; init; }

    /// <summary>
    /// The subscription identifier
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("subscription")]
    public SubscriptionInfo? Subscription { get; init; }

    /// <summary>
    /// The unscheduled subscription identifier
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("unscheduledSubscription")]
    public UnscheduledSubscriptionInfo? UnscheduledSubscription { get; init; }

    /// <summary>
    /// Merchant payment reference
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("myReference")]
    public string? MyReference { get; init; }
}
