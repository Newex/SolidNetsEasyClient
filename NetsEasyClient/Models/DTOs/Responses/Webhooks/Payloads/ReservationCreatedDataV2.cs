using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

/// <summary>
/// The reservation payload
/// </summary>
public record ReservationCreatedDataV2
{
    /// <summary>
    /// The payment identifier
    /// </summary>
    [Required]
    [JsonConverter(typeof(GuidTypeConverter))]
    [JsonPropertyName("paymentId")]
    public Guid PaymentId { get; init; }

    /// <summary>
    /// The payment method
    /// </summary>
    [JsonPropertyName("paymentMethod")]
    public PaymentMethodEnum PaymentMethod { get; init; }

    /// <summary>
    /// The payment type
    /// </summary>
    [JsonPropertyName("paymentType")]
    public PaymentTypeEnum PaymentType { get; init; }

    /// <summary>
    /// The reservation amount
    /// </summary>
    [JsonPropertyName("amount")]
    public WebhookAmount Amount { get; init; } = new();

    /// <summary>
    /// The complete order associated with the payment.
    /// </summary>
    /// <remarks>
    /// Specification says this is required. Actual response and given examples exclude this property
    /// </remarks>
    [JsonPropertyName("order")]
    public WebhookOrder? Order { get; init; }
}
