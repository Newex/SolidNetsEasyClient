using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// The payment.refund.failed event is triggered when a refund attempt has failed.
/// </summary>
public record RefundFailed : Webhook<RefundFailedData> { }

/// <summary>
/// The refund failed payload data
/// </summary>
public record RefundFailedData
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