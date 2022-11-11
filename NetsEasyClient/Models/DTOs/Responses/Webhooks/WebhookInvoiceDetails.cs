using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// Invoice details
/// </summary>
public record WebhookInvoiceDetails
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
