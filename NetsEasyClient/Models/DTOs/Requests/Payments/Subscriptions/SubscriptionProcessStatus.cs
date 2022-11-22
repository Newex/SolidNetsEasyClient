using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;

/// <summary>
/// The status of a bulk subscription charge process
/// </summary>
public record SubscriptionProcessStatus : BaseSubscription
{
    /// <summary>
    /// The subscription identifier (a UUID) returned from the Retrieve payment method.
    /// </summary>
    [Required]
    [JsonPropertyName("subscriptionId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public new Guid SubscriptionId { get; init; }

    /// <summary>
    /// The payment identifier.
    /// </summary>
    [JsonPropertyName("paymentId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid? PaymentId { get; init; }

    /// <summary>
    /// The charge identifier (a UUID) returned from the Charge payment method.
    /// </summary>
    [JsonPropertyName("chargeId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid? ChargeId { get; init; }

    /// <summary>
    /// The current processing status of the subscription. Possible values are: 'Pending', 'Succeeded', and 'Failed'.
    /// </summary>
    [Required]
    [JsonPropertyName("status")]
    public SubscriptionStatus Status { get; init; }

    /// <summary>
    /// The error message
    /// </summary>
    [JsonPropertyName("message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; init; }

    /// <summary>
    /// The error code
    /// </summary>
    [JsonPropertyName("code")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Code { get; init; }

    /// <summary>
    /// The error source
    /// </summary>
    [JsonPropertyName("source")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Source { get; init; }
}
