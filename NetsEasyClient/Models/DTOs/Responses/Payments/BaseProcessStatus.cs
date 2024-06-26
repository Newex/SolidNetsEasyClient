using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Payments;

/// <summary>
/// The process status of a subscription
/// </summary>
public abstract record BaseProcessStatus
{
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
    [JsonConverter(typeof(JsonStringEnumConverter<SubscriptionStatus>))]
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

    /// <summary>
    /// An external reference to identify a set of imported subscriptions. This parameter is only used if your subscriptions have been imported from a payment platform other than Nets Easy.
    /// </summary>
    [JsonPropertyName("externalReference")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public virtual string? ExternalReference { get; init; }
}
