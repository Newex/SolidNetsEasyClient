using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// <see cref="DateTimeOffset"/> converter
/// </summary>
public class NullableDateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
{
    /// <inheritdoc />
    public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var json = reader.GetString();
        if (json is null)
        {
            return null;
        }
        var dateTime = DateTimeOffset.Parse(json);
        return dateTime;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
    {
        if (value is not null)
            writer.WriteStringValue(value.Value);
    }
}
