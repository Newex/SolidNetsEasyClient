using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;

namespace SolidNetsEasyClient.Converters.WebhookPayloadConverters;

/// <summary>
/// Custom webhook amount json converter
/// </summary>
public class WebhookAmountConverter : JsonConverter<WebhookAmount>
{
    /// <inheritdoc />
    public override WebhookAmount? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonToken = reader.TokenType;
        if (jsonToken == JsonTokenType.Null || jsonToken == JsonTokenType.None)
        {
            return null;
        }

        // properties
        int? amount = null;
        string? currencyText = null;
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
                case JsonTokenType.Number:
                    var number = reader.GetInt32();
                    if (propertyName.Equals("amount")) amount = number;
                    break;
                case JsonTokenType.String:
                    var text = reader.GetString()!;
                    if (propertyName.Equals("currency")) currencyText = text;
                    break;
                case JsonTokenType.EndObject:
                    if (!amount.HasValue || currencyText is null)
                    {
                        throw new JsonException("Missing properties to deserialize to WebhookAmount");
                    }

                    parsing = false;
                    break;
            }
        }

        var hasCurrency = Enum.TryParse<Currency>(currencyText, out var currency);
        if (!amount.HasValue || currencyText is null || !hasCurrency)
        {
            throw new JsonException("Missing properties to deserialize to WebhookAmount");
        }

        return new()
        {
            Amount = amount.GetValueOrDefault(),
            Currency = currency
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, WebhookAmount value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("amount", value.Amount);
        writer.WriteString("currency", value.Currency.ToString().ToUpperInvariant());
        writer.WriteEndObject();
    }
}
