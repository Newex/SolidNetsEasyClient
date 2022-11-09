using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Payments;

/// <summary>
/// Details of an existing charge
/// </summary>
public record ChargeDetailsInfo
{
    /// <summary>
    /// The charge identifier (a UUID)
    /// </summary>
    [Required]
    [JsonPropertyName("chargeId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid ChargeId { get; init; }

    /// <summary>
    /// The charged amount
    /// </summary>
    [Required]
    [JsonPropertyName("chargeId")]
    public int Amount { get; init; }

    /// <summary>
    /// Information about an publicly accessible invoice
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("chargeId")]
    public Invoice? InvoiceDetails { get; init; }
}
