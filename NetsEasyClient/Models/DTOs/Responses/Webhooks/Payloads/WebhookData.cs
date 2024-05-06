using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

/// <summary>
/// A webhook data payload
/// </summary>
[JsonDerivedType(typeof(PaymentCreatedData))]
[JsonDerivedType(typeof(ChargeData))]
[JsonDerivedType(typeof(ChargeFailedData))]
[JsonDerivedType(typeof(CheckoutCompletedData))]
[JsonDerivedType(typeof(PaymentCancellationFailedData))]
[JsonDerivedType(typeof(PaymentCancelledData))]
[JsonDerivedType(typeof(RefundCompletedData))]
[JsonDerivedType(typeof(RefundFailedData))]
[JsonDerivedType(typeof(RefundInitiatedData))]
[JsonDerivedType(typeof(ReservationCreatedDataV1))]
[JsonDerivedType(typeof(ReservationCreatedDataV2))]
[JsonDerivedType(typeof(ReservationFailedData))]
public record WebhookData
{
    /// <summary>
    /// The payment identifier
    /// </summary>
    [Required]
    [JsonPropertyName("paymentId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid PaymentId { get; init; }
}