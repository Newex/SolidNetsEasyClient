using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

/// <summary>
/// The refund failed payload data
/// </summary>
public record RefundFailedData : WebhookData
{
    /// <summary>
    /// A unique identifier of this refund.
    /// </summary>
    [Required]
    [JsonPropertyName("refundId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid RefundId { get; init; }

    /// <summary>
    /// Contains information about an error (client error or server error).
    /// </summary>
    [Required]
    [JsonPropertyName("error")]
    public WebhookError Error { get; init; } = new();

    /// <summary>
    /// The amount of the refund.
    /// </summary>
    [Required]
    [JsonPropertyName("amount")]
    public WebhookAmount Amount { get; init; } = new();

    /// <summary>
    /// The invoice details
    /// </summary>
    [JsonPropertyName("invoiceDetails")]
    public WebhookInvoiceDetails InvoiceDetails { get; init; } = new();
}