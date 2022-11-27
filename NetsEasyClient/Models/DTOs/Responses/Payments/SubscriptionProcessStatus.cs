using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Payments;

/// <summary>
/// The status of a bulk subscription charge process
/// </summary>
public record SubscriptionProcessStatus : BaseProcessStatus
{
    /// <summary>
    /// The subscription identifier (a UUID) returned from the Retrieve payment method.
    /// </summary>
    [Required]
    [JsonPropertyName("subscriptionId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid SubscriptionId { get; init; }

    /// <inheritdoc />
    public override Guid GetId() => SubscriptionId;
}
