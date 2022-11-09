using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// Converter for <see cref="EventNames"/>
/// </summary>
public class EventNameConverter : JsonConverter<EventNames>
{
    /// <inheritdoc />
    public override EventNames? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var eventName = reader.GetString();
        if (eventName is null)
        {
            return null;
        }

        return new(eventName);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, EventNames value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
