using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Converters.WebhookPayloadConverters;

/// <summary>
/// Custom json converter
/// </summary>
public class ReservationFailedDataConverter : JsonConverter<ReservationFailedData>
{
    /// <inheritdoc />
    public override ReservationFailedData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonToken = reader.TokenType;
        if (jsonToken == JsonTokenType.None)
        {
            // Try advance once
            reader.Read();
            jsonToken = reader.TokenType;
        }

        if (jsonToken == JsonTokenType.Null)
        {
            return null;
        }

        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookAmount))) is not JsonConverter<WebhookAmount> webhookAmountConverter)
        {
            webhookAmountConverter = new WebhookAmountConverter();
        }
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(IList<Item>))) is not JsonConverter<IList<Item>> orderItemsConverter)
        {
            orderItemsConverter = new OrderItemsConverter();
        }
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookError))) is not JsonConverter<WebhookError> webhookErrorConverter)
        {
            webhookErrorConverter = new WebhookErrorConverter();
        }

        Guid? paymentId = null;
        WebhookAmount? amount = null;
        IList<Item> orderItems = [];
        WebhookError? error = null;

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
                    if (propertyName.Equals("paymentId")) paymentId = new Guid(text);
                    break;
                case JsonTokenType.StartObject:
                    if (propertyName.Equals("amount"))
                    {
                        amount = webhookAmountConverter.Read(ref reader, typeof(WebhookAmount), options);
                    }
                    else if (propertyName.Equals("orderItems"))
                    {
                        orderItems = orderItemsConverter.Read(ref reader, typeof(IList<Item>), options) ?? [];
                    }
                    else if (propertyName.Equals("error"))
                    {
                        error = webhookErrorConverter.Read(ref reader, typeof(WebhookError), options);
                    }
                    break;
                case JsonTokenType.EndObject:
                    parsing = false;
                    break;
            }
        }

        if (!paymentId.HasValue || amount is null || orderItems.Count == 0 || error is null)
        {
            throw new JsonException("Missing properties. Cannot deserialize to ReservationFailedData.");
        }

        return new()
        {
            PaymentId = paymentId.GetValueOrDefault(),
            Amount = amount,
            OrderItems = orderItems,
            Error = error
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ReservationFailedData value, JsonSerializerOptions options)
    {
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookAmount))) is not JsonConverter<WebhookAmount> webhookAmountConverter)
        {
            webhookAmountConverter = new WebhookAmountConverter();
        }
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(IList<Item>))) is not JsonConverter<IList<Item>> orderItemsConverter)
        {
            orderItemsConverter = new OrderItemsConverter();
        }
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookError))) is not JsonConverter<WebhookError> webhookErrorConverter)
        {
            webhookErrorConverter = new WebhookErrorConverter();
        }

        writer.WriteStartObject();

        writer.WriteString("paymentId", value.PaymentId.ToString("N"));

        writer.WritePropertyName("amount");
        webhookAmountConverter.Write(writer, value.Amount, options);

        writer.WritePropertyName("orderItems");
        orderItemsConverter.Write(writer, value.OrderItems, options);

        writer.WritePropertyName("error");
        webhookErrorConverter.Write(writer, value.Error, options);

        writer.WriteEndObject();
    }
}
