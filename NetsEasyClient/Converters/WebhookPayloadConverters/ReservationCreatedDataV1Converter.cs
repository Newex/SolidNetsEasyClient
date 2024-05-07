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
public class ReservationCreatedDataV1Converter : JsonConverter<ReservationCreatedDataV1>
{
    /// <inheritdoc />
    public override ReservationCreatedDataV1? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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

        if (options.Converters.FirstOrDefault(x =>
            x.CanConvert(typeof(ReservationCreatedCardDetails)))
            is not JsonConverter<ReservationCreatedCardDetails> reservationCreatedConverter)
        {
            reservationCreatedConverter = new ReservationCreatedCardDetailsConverter();
        }
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(ReservationCreatedConsumer))) is not JsonConverter<ReservationCreatedConsumer> reservationCreatedConsumerConverter)
        {
            reservationCreatedConsumerConverter = new ReservationCreatedConsumerConverter();
        }
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookAmount))) is not JsonConverter<WebhookAmount> webhookAmountConverter)
        {
            webhookAmountConverter = new WebhookAmountConverter();
        }

        // Properties
        Guid? paymentId = null;
        ReservationCreatedCardDetails? cardDetails = null;
        PaymentMethodEnum? paymentMethod = null;
        PaymentTypeEnum? paymentType = null;
        ReservationCreatedConsumer? consumer = null;
        string? reservationReference = null;
        Guid? reserveId = null;
        WebhookAmount? amount = null;

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
                    else if (propertyName.Equals("reservationReference")) reservationReference = text;
                    else if (propertyName.Equals("reserveId")) reserveId = new Guid(text);
                    break;
                case JsonTokenType.StartObject:
                    if (propertyName.Equals("cardDetails"))
                    {
                        cardDetails = reservationCreatedConverter.Read(ref reader, typeof(ReservationCreatedCardDetails), options);
                    }
                    else if (propertyName.Equals("consumer"))
                    {
                        consumer = reservationCreatedConsumerConverter.Read(ref reader, typeof(ReservationCreatedConsumer), options);
                    }
                    else if (propertyName.Equals("amount"))
                    {
                        amount = webhookAmountConverter.Read(ref reader, typeof(WebhookAmount), options);
                    }
                    break;
                case JsonTokenType.EndObject:
                    parsing = false;
                    break;
            }
        }

        if (!paymentId.HasValue
            || cardDetails is null
            || !paymentMethod.HasValue
            || !paymentType.HasValue
            || consumer is null
            || reservationReference is null
            || !reserveId.HasValue
            || amount is null)
        {
            throw new JsonException("Missing properties, cannot deserialize to ReservationCreatedDataV1.");
        }

        return new()
        {
            PaymentId = paymentId.GetValueOrDefault(),
            CardDetails = cardDetails,
            PaymentMethod = paymentMethod.GetValueOrDefault(),
            PaymentType = paymentType.GetValueOrDefault(),
            Consumer = consumer,
            ReservationReference = reservationReference,
            ReserveId = reserveId.GetValueOrDefault(),
            Amount = amount,
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ReservationCreatedDataV1 value, JsonSerializerOptions options)
    {
        var omitNull = options.DefaultIgnoreCondition.HasFlag(JsonIgnoreCondition.WhenWritingNull);
        if (options.Converters.FirstOrDefault(x =>
            x.CanConvert(typeof(ReservationCreatedCardDetails)))
            is not JsonConverter<ReservationCreatedCardDetails> reservationCreatedConverter)
        {
            reservationCreatedConverter = new ReservationCreatedCardDetailsConverter();
        }
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(ReservationCreatedConsumer))) is not JsonConverter<ReservationCreatedConsumer> reservationCreatedConsumerConverter)
        {
            reservationCreatedConsumerConverter = new ReservationCreatedConsumerConverter();
        }
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookAmount))) is not JsonConverter<WebhookAmount> webhookAmountConverter)
        {
            webhookAmountConverter = new WebhookAmountConverter();
        }

        writer.WriteStartObject();
        writer.WriteString("paymentId", value.PaymentId.ToString("N"));

        if (value.CardDetails is not null)
        {
            writer.WritePropertyName("cardDetails");
            reservationCreatedConverter.Write(writer, value.CardDetails, options);
        }
        else if (!omitNull)
        {
            writer.WriteNull("cardDetails");
        }

        writer.WriteString("paymentMethod", value.PaymentMethod.ToString());
        writer.WriteString("paymentType", PaymentTypeHelper.GetName(value.PaymentType));

        writer.WritePropertyName("consumer");
        reservationCreatedConsumerConverter.Write(writer, value.Consumer, options);

        writer.WriteString("reservationReference", value.ReservationReference);
        writer.WriteString("reserveId", value.ReserveId.ToString("N"));

        writer.WritePropertyName("amount");
        webhookAmountConverter.Write(writer, value.Amount, options);

        writer.WriteEndObject();
    }
}
