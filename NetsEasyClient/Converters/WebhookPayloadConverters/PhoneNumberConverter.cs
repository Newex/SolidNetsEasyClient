using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Contacts;

namespace SolidNetsEasyClient.Converters.WebhookPayloadConverters;

/// <summary>
/// Custom json converter
/// </summary>
public class PhoneNumberConverter : JsonConverter<PhoneNumber>
{
    /// <inheritdoc />
    public override PhoneNumber? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonToken = reader.TokenType;
        if (jsonToken == JsonTokenType.Null || jsonToken == JsonTokenType.None)
        {
            return null;
        }

        string? prefix = null;
        string? number = null;

        var propertyName = "";
        var parsing = true;
        while (parsing)
        {
            parsing = reader.Read();
            switch (reader.TokenType)
            {
                case JsonTokenType.PropertyName:
                    propertyName = reader.GetString()!;
                    break;
                case JsonTokenType.String:
                    var text = reader.GetString();
                    if (propertyName.Equals("prefix")) prefix = text;
                    else if (propertyName.Equals("number")) number = text;
                    break;
                case JsonTokenType.EndObject:
                    parsing = false;
                    break;
            }
        }

        return new()
        {
            Prefix = prefix,
            Number = number
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, PhoneNumber value, JsonSerializerOptions options)
    {
        var omitNull = options.DefaultIgnoreCondition.HasFlag(JsonIgnoreCondition.WhenWritingNull);
        writer.WriteStartObject();

        if (value.Prefix is not null && !omitNull)
            writer.WriteString("prefix", value.Prefix);
        else if (value.Prefix is null && !omitNull)
            writer.WriteNull("prefix");

        if (value.Number is not null && !omitNull)
            writer.WriteString("number", value.Number);
        else if (value.Number is null && !omitNull)
            writer.WriteNull("number");

        writer.WriteEndObject();
    }
}
