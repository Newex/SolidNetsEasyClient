using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// Converter for <see cref="ConsumerType"/>
/// </summary>
public class ConsumerEnumTypeConverter : JsonConverter<ConsumerEnumType>
{
    /// <inheritdoc />
    public override ConsumerEnumType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var text = reader.GetString();
        var isConsumerType = Enum.TryParse<ConsumerEnumType>(text, ignoreCase: true, out var consumerType);
        if (!isConsumerType)
        {
            throw new NotSupportedException("Cannot convert value to ConsumerType of either B2B or B2C");
        }

        return consumerType;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ConsumerEnumType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
