using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;

namespace SolidNetsEasyClient.Converters.WebhookPayloadConverters;

/// <summary>
/// Webhook order payload json converter
/// </summary>
public class WebhookOrderConverter : JsonConverter<WebhookOrder>
{
    /// <inheritdoc />
    public override WebhookOrder? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonToken = reader.TokenType;
        if (jsonToken == JsonTokenType.Null || jsonToken == JsonTokenType.None)
        {
            return null;
        }

        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(IList<Item>))) is not JsonConverter<IList<Item>> orderItemsConverter)
        {
            throw new JsonException("Must register OrderItemsConverter.");
        }
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookAmount))) is not JsonConverter<WebhookAmount> webhookAmountConverter)
        {
            throw new JsonException("Must register WebhookAmountConverter.");
        }

        // properties
        WebhookAmount? amount = null;
        string? reference = null;
        List<Item> orderItems = [];

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
                    if (propertyName.Equals("reference")) reference = text;
                    break;
                case JsonTokenType.StartObject:
                    if (propertyName.Equals("amount"))
                    {
                        amount = webhookAmountConverter.Read(ref reader, typeof(WebhookAmount), options);
                    }
                    break;
                case JsonTokenType.StartArray:
                    if (propertyName.Equals("orderItems"))
                    {
                        orderItems = orderItemsConverter.Read(ref reader, typeof(IList<Item>), options)?.ToList() ?? [];

                        // Stop if amount and reference have been parsed
                        parsing = !(amount is not null && reference is not null);
                    }
                    break;
            }
        }

        if (reference is null || amount is null)
        {
            throw new JsonException("Missing properties to deserialize WebhookOrder");
        }

        return new()
        {
            Reference = reference,
            Amount = amount,
            OrderItems = orderItems
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, WebhookOrder value, JsonSerializerOptions options)
    {
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(IList<Item>))) is not JsonConverter<IList<Item>> orderItemsConverter)
        {
            throw new JsonException("Must register OrderItemsConverter.");
        }
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookAmount))) is not JsonConverter<WebhookAmount> webhookAmountConverter)
        {
            throw new JsonException("Must register WebhookAmountConverter.");
        }

        writer.WriteStartObject();

        writer.WriteString("reference", value.Reference);

        writer.WritePropertyName("amount");
        webhookAmountConverter.Write(writer, value.Amount, options);

        writer.WritePropertyName("orderItems");
        orderItemsConverter.Write(writer, value.OrderItems, options);

        writer.WriteEndObject();
    }
}
