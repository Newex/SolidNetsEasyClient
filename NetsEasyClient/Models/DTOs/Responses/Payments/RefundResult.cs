using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Payments;

/// <summary>
/// A refund
/// </summary>
public record RefundResult
{
    /// <summary>
    /// The refund Identifier
    /// </summary>
    [Required]
    [JsonPropertyName("refundId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid RefundId { get; init; }
}
