using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

/// <summary>
/// Refund completed event payload data
/// </summary>
public record RefundCompletedData : WebhookData
{
    /// <summary>
    /// A unique identifier of this refund.
    /// </summary>
    [Required]
    [JsonPropertyName("refundId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid RefundId { get; init; }

    /// <summary>
    /// The amount of the refund.
    /// </summary>
    [Required]
    [JsonPropertyName("amount")]
    public WebhookAmount Amount { get; init; } = new();

    /// <summary>
    /// Invoice details
    /// </summary>
    [JsonPropertyName("invoiceDetails")]
    public WebhookInvoiceDetails InvoiceDetails { get; init; } = new();
}