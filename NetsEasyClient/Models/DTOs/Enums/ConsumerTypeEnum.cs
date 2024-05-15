using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Enums;

/// <summary>
/// An enumeration of consumer types
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ConsumerTypeEnum>))]
public enum ConsumerTypeEnum
{
    /// <summary>
    /// A business-2-consumer relation
    /// </summary>
    B2C = 0,

    /// <summary>
    /// A business-2-business relation
    /// </summary>
    B2B = 1
}
