using System;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Payments;

public record BulkId
{
    [JsonPropertyName("bulkId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid Id { get; init; }
}