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
public record ReservationCreatedData
{
    /// <summary>
    /// The payment identifier
    /// </summary>
    [Required]
    [JsonConverter(typeof(GuidTypeConverter))]
    [JsonPropertyName("paymentId")]
    public Guid PaymentId { get; init; }

    /// <summary>
    /// The complete order associated with the payment.
    /// </summary>
    [Required]
    [JsonPropertyName("order")]
    public WebhookOrder Order { get; init; } = new();

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

    /*
        Payment V1
    */

    /// <summary>
    /// The reservation card details
    /// </summary>
    [JsonPropertyName("cardDetails")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ReservationCreatedCardDetails? V1CardDetails { get; init; }

    /// <summary>
    /// The consumer details
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("consumer")]
    public ReservationCreatedConsumer? V1Consumer { get; init; }

    /// <summary>
    /// The reservation reference
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("reservationReference")]
    public string? V1ReservationReference { get; init; }

    /// <summary>
    /// The reserve id
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(GuidTypeConverter))]
    [JsonPropertyName("reserveId")]
    public Guid? V1ReserveId { get; init; }
}
