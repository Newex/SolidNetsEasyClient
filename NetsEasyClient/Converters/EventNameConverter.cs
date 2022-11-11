using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// Converter for <see cref="EventName"/>
/// </summary>
public class EventNameConverter : JsonConverter<EventName>
{
    /// <inheritdoc />
    public override EventName Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var eventName = EventNameHelper.ToEventName(reader.GetString());
        if (eventName is null)
        {
            throw new NotSupportedException();
        }

        return eventName.Value;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, EventName value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToStringEventName());
    }
}
