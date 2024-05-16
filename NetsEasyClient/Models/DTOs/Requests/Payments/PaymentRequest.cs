using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Requests.Webhooks;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Payments;

/// <summary>
/// A payment request object
/// </summary>
public record PaymentRequest
{
    /// <summary>
    /// Specifies an order associated with a payment. An order must contain at least one order item. The amount of the order must match the sum of the specified order items
    /// </summary>
    [Required]
    [JsonPropertyName("order")]
    public required Order Order { get; init; }

    /// <summary>
    /// Defines the behavior and style of the checkout page
    /// </summary>
    [Required]
    [JsonPropertyName("checkout")]
    public required Checkout Checkout { get; init; }

    /// <summary>
    /// The merchant number. Use this header only if you are a Nets partner and initiating the checkout with your partner keys. If you are using the integration keys for your webshop, there is no need to specify this header
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("merchantNumber")]
    public string? MerchantNumber { get; init; }

    /// <summary>
    /// Notifications allow you to subscribe to status updates for a payment
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("notifications")]
    public Notification? Notifications { get; init; }

    /// <summary>
    /// Defines the duration and interval when creating or updating a subscription
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("subscription")]
    public Subscription? Subscription { get; init; }

    /// <summary>
    /// Defines the payment as one that should initiate or update an unscheduled card on file agreement
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("unscheduledSubscription")]
    public UnscheduledSubscription? UnscheduledSubscription { get; init; }

    /// <summary>
    /// Specifies payment methods configuration to be used for this payment, ignored if empty or null
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("paymentMethodsConfiguration")]
    public IEnumerable<PaymentMethodConfiguration>? PaymentMethodsConfiguration { get; init; }

    /// <summary>
    /// The payment methods
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("paymentMethods")]
    public IEnumerable<PaymentMethod>? PaymentMethods { get; init; }

    /// <summary>
    /// Merchant payment reference.
    /// The maximum length is 36 characters.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("myReference")]
    public string? MyReference { get; init; }
}
