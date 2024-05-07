using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Converters.WebhookPayloadConverters;

/// <summary>
/// Custom converter
/// </summary>
public class CheckoutCompletedDataConverter : JsonConverter<CheckoutCompletedData>
{
    /// <inheritdoc />
    public override CheckoutCompletedData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonToken = reader.TokenType;
        if (jsonToken == JsonTokenType.Null || jsonToken == JsonTokenType.None)
        {
            return null;
        }

        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookOrder))) is not JsonConverter<WebhookOrder> webhookOrderConverter)
        {
            webhookOrderConverter = new WebhookOrderConverter();
        }
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(CheckoutCompletedConsumer))) is not JsonConverter<CheckoutCompletedConsumer> checkoutCompletedConsumerConverter)
        {
            checkoutCompletedConsumerConverter = new CheckoutCompletedConsumerConverter();
        }

        Guid? paymentId = null;
        WebhookOrder? order = null;
        CheckoutCompletedConsumer? consumer = null;
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
                    break;
                case JsonTokenType.StartObject:
                    if (propertyName.Equals("order"))
                    {
                        order = webhookOrderConverter.Read(ref reader, typeof(WebhookOrder), options);
                    }
                    else if (propertyName.Equals("consumer"))
                    {
                        consumer = checkoutCompletedConsumerConverter.Read(ref reader, typeof(CheckoutCompletedConsumer), options);
                    }
                    break;
                case JsonTokenType.EndObject:
                    if (!paymentId.HasValue)
                    {
                        // This is the {... data: { }} EndObject
                        continue;
                    }

                    parsing = false;
                    break;
            }
        }

        if (!paymentId.HasValue || order is null)
        {
            throw new JsonException("Could not parse CheckoutCompletedData. Missing properties.");
        }

        return new()
        {
            PaymentId = paymentId.GetValueOrDefault(),
            Order = order,
            Consumer = consumer,
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, CheckoutCompletedData value, JsonSerializerOptions options)
    {
        var omitNull = options.DefaultIgnoreCondition.HasFlag(JsonIgnoreCondition.WhenWritingNull);
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookOrder))) is not JsonConverter<WebhookOrder> webhookOrderConverter)
        {
            webhookOrderConverter = new WebhookOrderConverter();
        }
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(CheckoutCompletedConsumer))) is not JsonConverter<CheckoutCompletedConsumer> checkoutCompletedConsumerConverter)
        {
            checkoutCompletedConsumerConverter = new CheckoutCompletedConsumerConverter();
        }

        writer.WriteStartObject();
        writer.WriteString("paymentId", value.PaymentId.ToString("N"));

        writer.WritePropertyName("order");
        webhookOrderConverter.Write(writer, value.Order, options);

        if (!omitNull && value.Consumer is not null)
        {
            writer.WritePropertyName("consumer");
            checkoutCompletedConsumerConverter.Write(writer, value.Consumer, options);
        }
        else if (!omitNull)
        {
            writer.WriteNull("consumer");
        }

        writer.WriteEndObject();
    }
}
