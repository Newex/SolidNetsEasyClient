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
/// Payment cancelled webhook payload json converter
/// </summary>
public class PaymentCancelledDataConverter : JsonConverter<PaymentCancelledData>
{
    /// <inheritdoc />
    public override PaymentCancelledData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonToken = reader.TokenType;
        if (jsonToken == JsonTokenType.Null || jsonToken == JsonTokenType.None)
        {
            return null;
        }

        // Converters
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(IList<Item>))) is not JsonConverter<IList<Item>> orderItemsConverter)
        {
            orderItemsConverter = new OrderItemsConverter();
        }
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookAmount))) is not JsonConverter<WebhookAmount> webhookAmountConverter)
        {
            webhookAmountConverter = new WebhookAmountConverter();
        }

        // Properties
        Guid? paymentId = null;
        Guid? cancelId = null;
        IList<Item> orderItems = [];
        WebhookAmount? amount = null;

        var parsing = true;
        var propertyName = "";
        while (parsing)
        {
            parsing = reader.Read();
            switch (reader.TokenType)
            {
                case JsonTokenType.PropertyName:
                    propertyName = reader.GetString()!;
                    break;
                case JsonTokenType.String:
                    var text = reader.GetString()!;
                    if (propertyName.Equals("cancelId")) cancelId = new Guid(text);
                    else if (propertyName.Equals("paymentId")) paymentId = new Guid(text);
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
                        orderItems = orderItemsConverter.Read(ref reader, typeof(IList<Item>), options) ?? [];
                    }
                    break;
                case JsonTokenType.EndObject:
                    if (cancelId.HasValue && paymentId.HasValue && orderItems.Count > 0 && amount is not null)
                    {
                        parsing = false;
                    }
                    break;
            }
        }

        if (!cancelId.HasValue || !paymentId.HasValue || orderItems.Count == 0 || amount is null)
        {
            throw new JsonException("Cannot serialize PaymentCancelledData. Missing properties.");
        }

        return new()
        {
            CancelId = cancelId.GetValueOrDefault(),
            Amount = amount,
            OrderItems = orderItems,
            PaymentId = paymentId.GetValueOrDefault(),
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, PaymentCancelledData value, JsonSerializerOptions options)
    {
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(IList<Item>))) is not JsonConverter<IList<Item>> orderItemsConverter)
        {
            // Fallback to custom converter
            orderItemsConverter = new OrderItemsConverter();
        }

        writer.WriteStartObject();

        writer.WriteString("paymentId", value.PaymentId.ToString("N"));
        writer.WriteString("cancelId", value.CancelId.ToString("N"));

        writer.WritePropertyName("orderItems");
        writer.WriteStartArray();
        orderItemsConverter.Write(writer, value.OrderItems, options);
        writer.WriteEndArray();


        writer.WriteEndObject();
    }
}
