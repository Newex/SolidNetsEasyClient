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
public class PaymentCancellationFailedDataConverter : JsonConverter<PaymentCancellationFailedData>
{
    /// <inheritdoc />
    public override PaymentCancellationFailedData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonToken = reader.TokenType;
        if (jsonToken == JsonTokenType.Null || jsonToken == JsonTokenType.None)
        {
            return null;
        }

        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookError))) is not JsonConverter<WebhookError> webhookErrorConverter)
        {
            webhookErrorConverter = new WebhookErrorConverter();
        }
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(IList<Item>))) is not JsonConverter<IList<Item>> orderItemsConverter)
        {
            orderItemsConverter = new OrderItemsConverter();
        }
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookAmount))) is not JsonConverter<WebhookAmount> webhookAmountConverter)
        {
            webhookAmountConverter = new WebhookAmountConverter();
        }

        Guid? paymentId = null;
        WebhookError? error = null;
        Guid? cancelId = null;
        IList<Item> orderItems = [];
        WebhookAmount? amount = null;

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
                    var text = reader.GetString()!;
                    if (propertyName.Equals("paymentId")) paymentId = new Guid(text);
                    else if (propertyName.Equals("cancelId")) cancelId = new Guid(text);
                    break;
                case JsonTokenType.StartObject:
                    if (propertyName.Equals("error"))
                    {
                        error = webhookErrorConverter.Read(ref reader, typeof(WebhookError), options);
                    }
                    else if (propertyName.Equals("amount"))
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
                    parsing = false;
                    break;
            }
        }

        if (!paymentId.HasValue || error is null || !cancelId.HasValue || orderItems.Count == 0 || amount is null)
        {
            throw new JsonException("Missing properties, cannot deserialize PaymentCancellationFailed object");
        }

        return new()
        {
            PaymentId = paymentId.GetValueOrDefault(),
            CancelId = cancelId.GetValueOrDefault(),
            Error = error,
            Amount = amount,
            OrderItems = orderItems
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, PaymentCancellationFailedData value, JsonSerializerOptions options)
    {
        var omitNull = options.DefaultIgnoreCondition.HasFlag(JsonIgnoreCondition.WhenWritingNull);
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookError))) is not JsonConverter<WebhookError> webhookErrorConverter)
        {
            webhookErrorConverter = new WebhookErrorConverter();
        }
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(IList<Item>))) is not JsonConverter<IList<Item>> orderItemsConverter)
        {
            orderItemsConverter = new OrderItemsConverter();
        }
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookAmount))) is not JsonConverter<WebhookAmount> webhookAmountConverter)
        {
            webhookAmountConverter = new WebhookAmountConverter();
        }

        writer.WriteStartObject();
        writer.WriteString("paymentId", value.PaymentId.ToString("N"));
        writer.WriteString("cancelId", value.CancelId.ToString("N"));

        writer.WritePropertyName("error");
        webhookErrorConverter.Write(writer, value.Error, options);

        writer.WritePropertyName("orderItems");
        orderItemsConverter.Write(writer, value.OrderItems, options);

        writer.WritePropertyName("amount");
        webhookAmountConverter.Write(writer, value.Amount, options);

        writer.WriteEndObject();
    }
}
