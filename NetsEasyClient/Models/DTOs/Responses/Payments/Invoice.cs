using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Payments;

/// <summary>
/// An invoice
/// </summary>
public record Invoice
{
    /// <summary>
    /// The invoice number
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("invoiceNumber")]
    public string? InvoiceNumber { get; init; }
}
