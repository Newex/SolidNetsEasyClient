using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Contacts;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;

/// <summary>
/// The bulk subscription response, for a bulk charge operation.
/// </summary>
public record BulkSubscriptionResult
{
    /// <summary>
    /// The bulk charge identifier (a UUID). This identifier can be used when retrieving all charges associated with a bulk charge operation.
    /// </summary>
    [JsonPropertyName("bulkId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    [Required]
    public Guid BulkId { get; init; }

    /// <summary>
    /// An international phone number.
    /// </summary>
    [JsonPropertyName("phoneNumber")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PhoneNumber? PhoneNumber { get; init; }
}