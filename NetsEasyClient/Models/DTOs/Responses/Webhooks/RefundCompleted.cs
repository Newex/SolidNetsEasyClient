using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// The payment.refund.completed event is triggered when a refund has successfully been completed.
/// </summary>
public record RefundCompleted : Webhook<RefundCompletedData> { }

/// <summary>
/// Refund completed event payload data
/// </summary>
public record RefundCompletedData
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