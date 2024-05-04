using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

/// <summary>
/// The refund initiated payload
/// </summary>
public record RefundInitiatedData : WebhookData
{
    /// <summary>
    /// A unique identifier of this refund.
    /// </summary>
    [Required]
    [JsonPropertyName("refundId")]
    public Guid RefundId { get; init; }

    /// <summary>
    /// The charge identifier.
    /// </summary>
    [Required]
    [JsonPropertyName("chargeId")]
    public Guid ChargeId { get; init; }

    /// <summary>
    /// The amount of the refund.
    /// </summary>
    [Required]
    [JsonPropertyName("amount")]
    public WebhookAmount Amount { get; init; } = new();
}