using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Converters.WebhookPayloadConverters;

/// <summary>
/// Custom json converter
/// </summary>
public class ReservationCreatedDataV2Converter : JsonConverter<ReservationCreatedDataV2>
{
    /// <inheritdoc />
    public override ReservationCreatedDataV2? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookOrder))) is not JsonConverter<WebhookOrder> webhookOrderConverter)
        {
            webhookOrderConverter = new WebhookOrderConverter();
        }

        Guid? paymentId = null;
        PaymentMethodEnum? paymentMethod = null;
        PaymentTypeEnum? paymentType = null;
        WebhookAmount? amount = null;
        WebhookOrder? order = null;

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
                    else if (propertyName.Equals("paymentMethod")) paymentMethod = Enum.Parse<PaymentMethodEnum>(text);
                    else if (propertyName.Equals("paymentType")) paymentType = PaymentTypeHelper.Convert(text);
                    break;
                case JsonTokenType.StartObject:
                    if (propertyName.Equals("amount"))
                    {
                        amount = webhookAmountConverter.Read(ref reader, typeof(WebhookAmount), options);
                    }
                    else if (propertyName.Equals("order"))
                    {
                        order = webhookOrderConverter.Read(ref reader, typeof(WebhookOrder), options);
                    }
                    break;
                case JsonTokenType.EndObject:
                    parsing = false;
                    break;
            }
        }

        if (!paymentId.HasValue || !paymentMethod.HasValue || !paymentType.HasValue || amount is null)
        {
            throw new JsonException("Cannot deserialize to ReservationCreatedDataV2. Missing properties.");
        }

        return new()
        {
            PaymentId = paymentId.GetValueOrDefault(),
            PaymentMethod = paymentMethod.GetValueOrDefault(),
            PaymentType = paymentType.GetValueOrDefault(),
            Amount = amount,
            Order = order
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ReservationCreatedDataV2 value, JsonSerializerOptions options)
    {
        var omitNull = options.DefaultIgnoreCondition.HasFlag(JsonIgnoreCondition.WhenWritingNull);
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookAmount))) is not JsonConverter<WebhookAmount> webhookAmountConverter)
        {
            webhookAmountConverter = new WebhookAmountConverter();
        }
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookOrder))) is not JsonConverter<WebhookOrder> webhookOrderConverter)
        {
            webhookOrderConverter = new WebhookOrderConverter();
        }

        writer.WriteStartObject();
        writer.WriteString("paymentId", value.PaymentId.ToString("N"));
        writer.WriteString("paymentMethod", value.PaymentMethod.ToString());
        writer.WriteString("paymentType", PaymentTypeHelper.GetName(value.PaymentType));

        writer.WritePropertyName("amount");
        webhookAmountConverter.Write(writer, value.Amount, options);

        if (value.Order is not null)
        {
            writer.WritePropertyName("order");
            webhookOrderConverter.Write(writer, value.Order, options);
        }
        else if (!omitNull)
        {
            writer.WriteNull("order");
        }

        writer.WriteEndObject();
    }
}
