using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// Converter for <see cref="DateTimeOffset"/>
/// </summary>
public class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    /// <inheritdoc />
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var json = reader.GetString();
        var isDateTime = DateTimeOffset.TryParse(json, out var dateTime);
        if (!isDateTime)
        {
            throw new NotSupportedException("Cannot convert value to DateTime object");
        }

        return dateTime;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
