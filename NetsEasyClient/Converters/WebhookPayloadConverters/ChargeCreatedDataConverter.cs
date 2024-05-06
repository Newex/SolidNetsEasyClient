using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Converters.WebhookPayloadConverters;

/// <summary>
/// Custom converter for charge data webhook payload
/// </summary>
public class ChargeCreatedDataConverter : JsonConverter<ChargeData>
{
    /// <inheritdoc />
    public override ChargeData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
        Guid? subscriptionId = null;
        Guid? chargeId = null;
        IList<Item> orderItems = [];
        PaymentMethodEnum? paymentMethod = null;
        PaymentTypeEnum? paymentType = null;
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
                    if (propertyName.Equals("paymentId")) paymentId = new Guid(text);
                    else if (propertyName.Equals("subscriptionId")) subscriptionId = new Guid(text);
                    else if (propertyName.Equals("chargeId")) chargeId = new Guid(text);
                    else if (propertyName.Equals("paymentMethod")) paymentMethod = Enum.Parse<PaymentMethodEnum>(text);
                    else if (propertyName.Equals("paymentType")) paymentType = PaymentTypeHelper.Convert(text);
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
            }
        }

        if (!chargeId.HasValue
            || orderItems.Count == 0
            || !paymentId.HasValue
            || amount is null
            || !paymentMethod.HasValue
            || !paymentType.HasValue)
        {
            throw new JsonException("Could not deserialize to ChargeData. Missing properties.");
        }

        return new()
        {
            PaymentId = paymentId.GetValueOrDefault(),
            ChargeId = chargeId.GetValueOrDefault(),
            OrderItems = orderItems,
            Amount = amount,
            PaymentMethod = paymentMethod.GetValueOrDefault(),
            PaymentType = paymentType.GetValueOrDefault(),
            SubscriptionId = subscriptionId
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ChargeData value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString("paymentId", value.PaymentId.ToString("N"));
        if (value.SubscriptionId.HasValue)
        {
            if (!options.DefaultIgnoreCondition.HasFlag(JsonIgnoreCondition.WhenWritingNull))
            {
                writer.WriteString("subscriptionId", value.SubscriptionId.GetValueOrDefault().ToString("N"));
            }
        }
        else
        {
            if (options.DefaultIgnoreCondition.HasFlag(JsonIgnoreCondition.WhenWritingNull))
            {
                writer.WriteNull("subscriptionId");
            }
        }
        writer.WriteString("chargeId", value.ChargeId.ToString("N"));
        writer.WriteString("paymentMethod", value.PaymentMethod.ToString());
        writer.WriteString("paymentType", value.PaymentType.ToString().ToUpperInvariant());

        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(IList<Item>))) is not JsonConverter<IList<Item>> orderItemsConverter)
        {
            // Fallback to custom converter
            orderItemsConverter = new OrderItemsConverter();
        }
        writer.WritePropertyName("orderItems");
        orderItemsConverter.Write(writer, value.OrderItems, options);

        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookAmount))) is not JsonConverter<WebhookAmount> webhookAmountConverter)
        {
            webhookAmountConverter = new WebhookAmountConverter();
        }
        writer.WritePropertyName("amount");
        webhookAmountConverter.Write(writer, value.Amount, options);

        writer.WriteEndObject();
    }
}
