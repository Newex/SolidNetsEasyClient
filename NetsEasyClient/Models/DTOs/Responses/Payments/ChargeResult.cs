using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Payments;

/// <summary>
/// A charge result
/// </summary>
public record ChargeResult
{
    /// <summary>
    /// The charge identifier
    /// </summary>
    [JsonPropertyName("chargeId")]
    [Required]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid ChargeId { get; init; }

    /// <summary>
    /// An invoice for the charge
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("invoice")]
    public Invoice? Invoice { get; init; }
}
