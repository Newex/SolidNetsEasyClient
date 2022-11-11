using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

/// <summary>
/// The refund initiated payload
/// </summary>
public record RefundInitiatedData
{
    /// <summary>
    /// The payment identifier
    /// </summary>
    [Required]
    [JsonConverter(typeof(GuidTypeConverter))]
    [JsonPropertyName("paymentId")]
    public Guid PaymentId { get; init; }

    /// <summary>
    /// A unique identifier of this refund.
    /// </summary>
    [Required]
    [JsonConverter(typeof(GuidTypeConverter))]
    [JsonPropertyName("refundId")]
    public Guid RefundId { get; init; }

    /// <summary>
    /// The charge identifier.
    /// </summary>
    [Required]
    [JsonConverter(typeof(GuidTypeConverter))]
    [JsonPropertyName("chargeId")]
    public Guid ChargeId { get; init; }

    /// <summary>
    /// The amount of the refund.
    /// </summary>
    [Required]
    [JsonPropertyName("amount")]
    public WebhookAmount Amount { get; init; } = new();
}