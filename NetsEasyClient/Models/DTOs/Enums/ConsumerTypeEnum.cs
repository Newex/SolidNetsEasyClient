using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Enums;

/// <summary>
/// An enumeration of consumer types
/// </summary>
[JsonConverter(typeof(ConsumerTypeEnumConverter))]
public enum ConsumerTypeEnum
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
