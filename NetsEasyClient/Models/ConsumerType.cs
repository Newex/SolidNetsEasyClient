using System.Collections.Generic;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models;

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
    public string? Default { get; init; }

    /// <summary>
    /// The array of consumer types that should be supported on the checkout page. Allowed values are: 'B2B' and 'B2C'
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("supportedTypes")]
    public IEnumerable<ConsumerEnumType>? SupportedTypes { get; init; }
}

/// <summary>
/// An enumeration of consumer types
/// </summary>
[JsonConverter(typeof(ConsumerEnumTypeConverter))]
public enum ConsumerEnumType
{
    /// <summary>
    /// A business-2-consumer relation
    /// </summary>
    B2C,

    /// <summary>
    /// A business-2-business relation
    /// </summary>
    B2B
}
