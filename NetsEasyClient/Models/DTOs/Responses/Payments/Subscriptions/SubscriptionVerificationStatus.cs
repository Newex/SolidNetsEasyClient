using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Payments.Subscriptions;

/// <summary>
/// The current status of the subscription verification.
/// </summary>
public record SubscriptionVerificationStatus
{
    /// <summary>
    /// The identifier of the subscription (a UUID).
    /// </summary>
    [Required]
    [JsonPropertyName("subscriptionId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid SubscriptionId { get; init; }

    /// <summary>
    /// An external reference to identify a set of imported subscriptions. This 
    /// parameter is only used if your subscriptions have been imported from a 
    /// payment platform other than Checkout.
    /// </summary>
    [JsonPropertyName("externalReference")]
    public string? ExternalReference { get; init; }

    /// <summary>
    /// The current processing status of the subscription. Possible values are: 
    /// 'Pending', 'Succeeded', and 'Failed'.
    /// </summary>
    [Required]
    [JsonPropertyName("status")]
    public SubscriptionStatus Status { get; init; }

    /// <summary>
    /// The payment identifier (a UUID).
    /// </summary>
    [JsonPropertyName("paymentId")]
    [JsonConverter(typeof(NullableGuidTypeConverter))]
    public Guid? PaymentId { get; init; }
}
