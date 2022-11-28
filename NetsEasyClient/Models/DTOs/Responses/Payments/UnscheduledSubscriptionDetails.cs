using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Payments;

/// <summary>
/// Information about an unscheduled subscription
/// </summary>
public record UnscheduledSubscriptionDetails
{
    /// <summary>
    /// The unscheduled subscription identifier.
    /// </summary>
    [Required]
    [JsonPropertyName("unscheduledSubscriptionId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid UnscheduledSubscriptionId { get; init; }

    /// <summary>
    /// The payment details
    /// </summary>
    [Required]
    [JsonPropertyName("paymentDetails")]
    public PaymentDetails PaymentDetails { get; init; } = new();
}
