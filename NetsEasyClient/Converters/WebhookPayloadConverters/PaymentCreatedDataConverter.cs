using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Converters.WebhookPayloadConverters;

/// <summary>
/// Payment created webhook payload json converter
/// </summary>
public class PaymentCreatedDataConverter : JsonConverter<PaymentCreatedData>
{
    /// <inheritdoc />
    public override PaymentCreatedData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonToken = reader.TokenType;
        if (jsonToken == JsonTokenType.Null || jsonToken == JsonTokenType.None)
        {
            return null;
        }

        // Converters
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookOrder))) is not JsonConverter<WebhookOrder> webhookOrderConverter)
        {
            throw new JsonException("Must register WebhookOrderConverter.");
        }

        // Properties
        Guid? paymentId = null;
        WebhookOrder? webhookOrder = null;

        var propertyName = "";

        while (reader.Read())
        {
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
                    if (propertyName.Equals("order"))
                    {
                        webhookOrder = webhookOrderConverter.Read(ref reader, typeof(WebhookOrder), options);
                    }
                    break;
            }
        }

        if (!paymentId.HasValue || webhookOrder is null)
        {
            throw new JsonException("Missing properties to deserialize object to PaymentCreatedData");
        }

        return new()
        {
            PaymentId = paymentId.GetValueOrDefault(),
            Order = webhookOrder
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, PaymentCreatedData value, JsonSerializerOptions options)
    {
        var orderWriter = new WebhookOrderConverter();

        writer.WriteStartObject();

        writer.WriteString("paymentId", value.PaymentId.ToString("N"));
        writer.WritePropertyName("order");
        orderWriter.Write(writer, value.Order, options);

        writer.WriteEndObject();
    }
}
