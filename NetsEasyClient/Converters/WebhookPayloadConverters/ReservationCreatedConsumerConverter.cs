using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Converters.WebhookPayloadConverters;

/// <summary>
/// Custom json converter
/// </summary>
public class ReservationCreatedConsumerConverter : JsonConverter<ReservationCreatedConsumer>
{
    /// <inheritdoc />
    public override ReservationCreatedConsumer? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonToken = reader.TokenType;
        if (jsonToken == JsonTokenType.Null || jsonToken == JsonTokenType.None)
        {
            return null;
        }

        string? ip = null;
        var propertyName = "";
        var parsing = true;

        while (parsing)
        {
            parsing = reader.Read();
            jsonToken = reader.TokenType;
            switch (jsonToken)
            {
                case JsonTokenType.PropertyName:
                    propertyName = reader.GetString()!;
                    break;
                case JsonTokenType.String:
                    var text = reader.GetString()!;
                    if (propertyName.Equals("ip")) ip = text;
                    break;
                case JsonTokenType.EndObject:
                    parsing = false;
                    break;
            }
        }

        if (ip is null)
        {
            throw new JsonException("Missing property for deserialization of ReservationCreatedConsumer.");
        }

        return new()
        {
            IP = ip
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ReservationCreatedConsumer value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("ip", value.IP);
        writer.WriteEndObject();
    }
}
