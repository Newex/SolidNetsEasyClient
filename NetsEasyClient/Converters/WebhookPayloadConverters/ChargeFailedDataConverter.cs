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
public class ChargeFailedDataConverter : JsonConverter<ChargeFailedData>
{
    /// <inheritdoc />
    public override ChargeFailedData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookAmount))) is not JsonConverter<WebhookAmount> webhookAmountConverter)
        {
            webhookAmountConverter = new WebhookAmountConverter();
        }
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(IList<Item>))) is not JsonConverter<IList<Item>> orderItemsConverter)
        {
            orderItemsConverter = new OrderItemsConverter();
        }

        Guid? paymentId = null;
        Guid? chargeId = null;
        WebhookError? error = null;
        IList<Item> orderItems = [];
        Guid? reservationId = null;
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
                case JsonTokenType.String:
                    var text = reader.GetString()!;
                    if (propertyName.Equals("paymentId")) paymentId = new Guid(text);
                    else if (propertyName.Equals("chargeId")) chargeId = new Guid(text);
                    else if (propertyName.Equals("reservationId")) reservationId = new Guid(text);
                    break;
                case JsonTokenType.StartArray:
                    if (propertyName.Equals("orderItems"))
                    {
                        orderItems = orderItemsConverter.Read(ref reader, typeof(IList<Item>), options) ?? [];
                    }
                    break;
                case JsonTokenType.EndArray:
                    parsing = false;
                    break;
            }
        }

        if (!paymentId.HasValue || error is null || !chargeId.HasValue || orderItems.Count == 0 || !reservationId.HasValue || amount is null)
        {
            throw new JsonException("Missing properties cannot deserialize to charge failed data");
        }

        return new()
        {
            PaymentId = paymentId.GetValueOrDefault(),
            ChargeId = chargeId.GetValueOrDefault(),
            Amount = amount,
            Error = error,
            OrderItems = orderItems,
            ReservationId = reservationId.GetValueOrDefault(),
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ChargeFailedData value, JsonSerializerOptions options)
    {
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookError))) is not JsonConverter<WebhookError> webhookErrorConverter)
        {
            webhookErrorConverter = new WebhookErrorConverter();
        }
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookAmount))) is not JsonConverter<WebhookAmount> webhookAmountConverter)
        {
            webhookAmountConverter = new WebhookAmountConverter();
        }
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(IList<Item>))) is not JsonConverter<IList<Item>> orderItemsConverter)
        {
            orderItemsConverter = new OrderItemsConverter();
        }

        writer.WriteStartObject();

        writer.WritePropertyName("error");
        webhookErrorConverter.Write(writer, value.Error, options);

        writer.WriteString("paymentId", value.PaymentId.ToString("N"));
        writer.WriteString("chargeId", value.ChargeId.ToString("N"));

        writer.WritePropertyName("orderItems");
        orderItemsConverter.Write(writer, value.OrderItems, options);

        writer.WriteString("reservationId", value.ReservationId.ToString("N"));

        writer.WritePropertyName("amount");
        webhookAmountConverter.Write(writer, value.Amount, options);

        writer.WriteEndObject();
    }
}
