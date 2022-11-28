using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Payments;

/// <summary>
/// The result of a successfull unscheduled subscription charge
/// </summary>
public record UnscheduledSubscriptionChargeResult
{
    /// <summary>
    /// The payment identifier of the new payment object created when charging for the unscheduled subscription.
    /// </summary>
    [Required]
    [JsonPropertyName("paymentId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid PaymentId { get; init; }

    /// <summary>
    /// A unique identifier of the charge.
    /// </summary>
    [Required]
    [JsonPropertyName("chargeId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid ChargeId { get; init; }
}
