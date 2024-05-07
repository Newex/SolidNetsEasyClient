using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;

namespace SolidNetsEasyClient.Converters.WebhookPayloadConverters;

/// <summary>
/// Custom json converter
/// </summary>
public class WebhookErrorConverter : JsonConverter<WebhookError>
{
    /// <inheritdoc />
    public override WebhookError? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonToken = reader.TokenType;
        if (jsonToken == JsonTokenType.Null || jsonToken == JsonTokenType.None)
        {
            return null;
        }

        string? message = null;
        string? code = null;
        string? source = null;

        var propertyName = "";
        var parsing = true;
        while (parsing)
        {
            parsing = reader.Read();
            var token = reader.TokenType;
            switch (token)
            {
                case JsonTokenType.PropertyName:
                    propertyName = reader.GetString()!;
                    break;
                case JsonTokenType.String:
                    if (propertyName.Equals("message")) message = reader.GetString();
                    else if (propertyName.Equals("code")) code = reader.GetString();
                    else if (propertyName.Equals("source")) source = reader.GetString();
                    break;
                case JsonTokenType.EndObject:
                    parsing = false;
                    break;
            }
        }

        return new()
        {
            Message = message,
            Code = code,
            Source = source
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, WebhookError value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString("message", value.Message);
        writer.WriteString("code", value.Code);
        writer.WriteString("source", value.Source);

        writer.WriteEndObject();
    }
}
