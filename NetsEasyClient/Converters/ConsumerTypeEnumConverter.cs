using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// Converter for <see cref="ConsumerTypeEnum"/>
/// </summary>
public class ConsumerTypeEnumConverter : JsonConverter<ConsumerTypeEnum>
{
    /// <inheritdoc />
    public override ConsumerTypeEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var text = reader.GetString();
        var isConsumerType = Enum.TryParse<ConsumerTypeEnum>(text, ignoreCase: true, out var consumerType);
        if (!isConsumerType)
        {
            throw new NotSupportedException("Cannot convert value to ConsumerType of either B2B or B2C");
        }

        return consumerType;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ConsumerTypeEnum value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
