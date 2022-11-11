using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

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
    public RefundCompletedInvoiceDetails InvoiceDetails { get; init; } = new();
}

/// <summary>
/// Invoice details
/// </summary>
public record RefundCompletedInvoiceDetails
{
    /// <summary>
    /// The type of distribution, for example 'Email'.
    /// </summary>
    [Required]
    [JsonPropertyName("distributionType")]
    public string DistributionType { get; init; } = string.Empty;

    /// <summary>
    /// The due date of the invoice.
    /// </summary>
    [Required]
    [JsonPropertyName("invoiceDueDate")]
    [JsonConverter(typeof(InvoiceDateConverter))]
    public DateOnly InvoiceDueDate { get; init; }

    /// <summary>
    /// The invoice number.
    /// </summary>
    [Required]
    [JsonPropertyName("invoiceNumber")]
    public string InvoiceNumber { get; init; } = string.Empty;
}
