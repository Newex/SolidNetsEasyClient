using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.Status;

/// <summary>
/// Retrieve refund status
/// </summary>
public record RetrieveRefund
{
    /// <summary>
    /// The refund Identifier
    /// </summary>
    [Required]
    [JsonPropertyName("refundId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid RefundId { get; init; }

    /// <summary>
    /// The amount
    /// </summary>
    [Required]
    [JsonPropertyName("amount")]
    public int Amount { get; init; }

    /// <summary>
    /// Information about an publicly accessible invoice
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("amount")]
    public RefundInvoiceDetail? InvoiceDetails { get; init; }
}

/// <summary>
/// Invoice for a refund
/// </summary>
public record RefundInvoiceDetail
{
    /// <summary>
    /// The URL of an invoice that is publicly accessible
    /// </summary>
    [JsonPropertyName("link")]
    public string? Link { get; init; }
}
