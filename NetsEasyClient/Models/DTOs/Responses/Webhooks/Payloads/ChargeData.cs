using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

/// <summary>
/// The data associated with this event.
/// </summary>
public record ChargeData : IWebhookData
{
    /// <summary>
    /// The payment identifier.
    /// </summary>
    [Required]
    [JsonConverter(typeof(GuidTypeConverter))]
    [JsonPropertyName("paymentId")]
    public Guid PaymentId { get; init; }

    /// <summary>
    /// The subscription identifier.
    /// </summary>
    [JsonConverter(typeof(GuidTypeConverter))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("subscriptionId")]
    public Guid? SubscriptionId { get; init; }

    /// <summary>
    /// The charge identifier.
    /// </summary>
    [JsonConverter(typeof(GuidTypeConverter))]
    [JsonPropertyName("chargeId")]
    public Guid ChargeId { get; init; }

    /// <summary>
    /// The list of order items that are associated with the charge. Contains at least one order item.
    /// </summary>
    [Required]
    [JsonPropertyName("orderItems")]
    public IList<Item> OrderItems { get; init; } = new List<Item>();

    /// <summary>
    /// The payment method, for example 'Visa' or 'Mastercard'.
    /// </summary>
    [JsonPropertyName("paymentMethod")]
    public PaymentMethodEnum PaymentMethod { get; init; }

    /// <summary>
    /// The type of payment. Possible values are: 'CARD', 'INVOICE', 'A2A', 'INSTALLMENT', 'WALLET', and 'PREPAID-INVOICE'.
    /// </summary>
    [JsonPropertyName("paymentType")]
    public PaymentTypeEnum PaymentType { get; init; }

    /// <summary>
    /// The amount of the charge.
    /// </summary>
    [JsonPropertyName("amount")]
    public WebhookAmount Amount { get; init; } = new();
}
