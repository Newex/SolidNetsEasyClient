using System.Collections.Generic;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Customers;

/// <summary>
/// The type of consumer
/// </summary>
public record ConsumerType
{
    /// <summary>
    /// The checkout form defaults to this consumer type when first loaded
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("default")]
    [JsonConverter(typeof(ConsumerTypeEnumConverter))]
    public ConsumerTypeEnum? Default { get; init; }

    /// <summary>
    /// The array of consumer types that should be supported on the checkout page. Allowed values are: 'B2B' and 'B2C'
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("supportedTypes")]
    public IEnumerable<ConsumerTypeEnum>? SupportedTypes { get; init; }
}