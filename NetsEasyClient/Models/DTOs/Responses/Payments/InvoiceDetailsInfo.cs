using System;
using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Payments;

/// <summary>
/// Invoice detail information
/// </summary>
public record InvoiceDetailsInfo
{
    /// <summary>
    /// The invoice number
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("invoiceNumber")]
    public string? InvoiceNumber { get; init; }

    /// <summary>
    /// The reference number
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ocr")]
    public string? OCR { get; init; }

    /// <summary>
    /// The PDF link
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("pdfLink")]
    public string? PDFLink { get; init; }

    /// <summary>
    /// The invoice due date
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("dueDate")]
    public DateTimeOffset? DueDate { get; init; }
}
