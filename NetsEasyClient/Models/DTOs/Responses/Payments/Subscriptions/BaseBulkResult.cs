using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Payments.Subscriptions;

/// <summary>
/// The bulk result, containing the bulk id.
/// </summary>
public record BaseBulkResult
{
    /// <summary>
    /// The bulk charge identifier (a UUID). This identifier can be used when retrieving all charges associated with a bulk charge operation.
    /// </summary>
    [JsonPropertyName("bulkId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    [Required]
    public Guid BulkId { get; init; }
}
