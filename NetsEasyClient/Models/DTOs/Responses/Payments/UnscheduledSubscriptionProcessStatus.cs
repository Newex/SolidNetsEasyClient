using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Payments;

/// <summary>
/// The status of a bulk unscheduled subscription charge process
/// </summary>
public record UnscheduledSubscriptionProcessStatus : BaseProcessStatus
{
    /// <summary>
    /// The unscheduled subscription identifier (a UUID) returned from the Retrieve bulk unscheduled subscription charges method.
    /// </summary>
    [Required]
    [JsonPropertyName("unscheduledSubscriptionId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid UnscheduledSubscriptionId { get; init; }
}
