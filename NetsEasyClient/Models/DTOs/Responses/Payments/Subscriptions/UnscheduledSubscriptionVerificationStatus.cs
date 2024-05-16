using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Payments.Subscriptions;

/// <summary>
/// Unscheduled subscription verification status
/// </summary>
public record UnscheduledSubscriptionVerificationStatus
{
    /// <summary>
    /// The identifier of the unscheduled subscription (a UUID).
    /// </summary>
    [Required]
    [JsonPropertyName("unscheduledSubscriptionId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid UnscheduledSubscriptionId { get; init; }

    /// <summary>
    /// An external reference to identify a set of imported unscheduled 
    /// subscriptions. This parameter is only used if your unscheduled 
    /// subscriptions have been imported from a payment platform other than 
    /// Checkout.
    /// </summary>
    [JsonPropertyName("externalReference")]
    public string? ExternalReference { get; init; }

    /// <summary>
    /// The current processing status of the unscheduled subscription. Possible 
    /// values are: 'Pending', 'Succeeded', and 'Failed'.
    /// </summary>
    [Required]
    [JsonPropertyName("status")]
    public SubscriptionStatus Status { get; init; }

    /// <summary>
    /// The payment identifier (a UUID).
    /// </summary>
    [JsonPropertyName("paymentId")]
    public Guid? PaymentId { get; init; }
}
