using System;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

/// <summary>
/// The reservation payload
/// </summary>
public record ReservationCreatedDataV1 : WebhookData
{
    /// <summary>
    /// The reservation card details
    /// </summary>
    [JsonPropertyName("cardDetails")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ReservationCreatedCardDetails? CardDetails { get; init; }

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
    /// The consumer details
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("consumer")]
    public ReservationCreatedConsumer Consumer { get; init; } = new();

    /// <summary>
    /// The reservation reference
    /// </summary>
    [JsonPropertyName("reservationReference")]
    public string ReservationReference { get; init; } = string.Empty;

    /// <summary>
    /// The reserve id
    /// </summary>
    [JsonPropertyName("reserveId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid ReserveId { get; init; }

    /// <summary>
    /// The reservation amount
    /// </summary>
    [JsonPropertyName("amount")]
    public WebhookAmount Amount { get; init; } = new();
}
