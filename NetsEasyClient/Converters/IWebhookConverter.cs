using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters.WebhookPayloadConverters;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// Webhook converter
/// </summary>
public class IWebhookConverter : JsonConverter<IWebhook<WebhookData>>
{
    /// <inheritdoc />
    public override IWebhook<WebhookData>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonToken = reader.TokenType;
        if (jsonToken == JsonTokenType.Null || jsonToken == JsonTokenType.None)
        {
            return null;
        }

        Guid? id = null;
        int? merchantId = null;
        DateTimeOffset? timestamp = null;
        EventName? eventName = null;
        WebhookData? data = null;
        var dataBuilder = new StringBuilder();

        var propertyName = "";
        while (reader.Read())
        {
            jsonToken = reader.TokenType;
            switch (jsonToken)
            {
                case JsonTokenType.PropertyName:
                    propertyName = reader.GetString()!;
                    break;
                case JsonTokenType.Number:
                    var number = reader.GetInt32();
                    if (propertyName.Equals("merchantId")) merchantId = number;
                    else if (propertyName.Equals("merchantNumber")) merchantId = number;
                    break;
                case JsonTokenType.String:
                    var text = reader.GetString()!;
                    if (propertyName.Equals("timestamp")) timestamp = DateTimeOffset.Parse(text);
                    else if (propertyName.Equals("event"))
                    {
                        eventName = EventNameHelper.ToEventName(text);
                    }
                    else if (propertyName.Equals("id"))
                    {
                        id = new Guid(text);
                    }
                    break;
                case JsonTokenType.StartObject:
                    if (propertyName.Equals("data"))
                    {
                        if (eventName is null)
                        {
                            var doc = JsonDocument.ParseValue(ref reader);
                            dataBuilder.Append(doc.RootElement.GetRawText());
                            continue;
                        }

                        // Parse data depending on event name
                        data = GetWebhookData(ref reader, eventName.GetValueOrDefault(), options);
                    }
                    break;
            }
        }

        if (data is null && dataBuilder.Length > 0 && eventName.HasValue)
        {
            var stringReader = new Utf8JsonReader(Encoding.UTF8.GetBytes(dataBuilder.ToString()));

            // Advance once
            stringReader.Read();
            data = GetWebhookData(ref stringReader, eventName.GetValueOrDefault(), options);
        }

        // Done reading! All properties must have values
        if (!id.HasValue || !merchantId.HasValue || !timestamp.HasValue || !eventName.HasValue || data is null)
        {
            throw new JsonException("Could not parse all properties");
        }

        return eventName switch
        {
            EventName.PaymentCreated => new PaymentCreated
            {
                Id = id.GetValueOrDefault(),
                Data = (PaymentCreatedData)data!,
                Event = eventName.GetValueOrDefault(),
                MerchantId = merchantId.GetValueOrDefault(),
                Timestamp = timestamp.GetValueOrDefault()
            },
            EventName.PaymentCancelled => new PaymentCancelled
            {
                Id = id.GetValueOrDefault(),
                Data = (PaymentCancelledData)data!,
                Event = eventName.GetValueOrDefault(),
                MerchantId = merchantId.GetValueOrDefault(),
                Timestamp = timestamp.GetValueOrDefault(),
            },
            EventName.ChargeCreated => new ChargeCreated
            {
                Id = id.GetValueOrDefault(),
                Data = (ChargeData)data!,
                Event = eventName.GetValueOrDefault(),
                MerchantId = merchantId.GetValueOrDefault(),
                Timestamp = timestamp.GetValueOrDefault(),
            },
            EventName.ChargeFailed => new ChargeFailed
            {
                Id = id.GetValueOrDefault(),
                Data = (ChargeFailedData)data!,
                Event = eventName.GetValueOrDefault(),
                MerchantId = merchantId.GetValueOrDefault(),
                Timestamp = timestamp.GetValueOrDefault(),
            },
            EventName.CheckoutCompleted => new CheckoutCompleted
            {
                Id = id.GetValueOrDefault(),
                Data = (CheckoutCompletedData)data!,
                Event = eventName.GetValueOrDefault(),
                MerchantId = merchantId.GetValueOrDefault(),
                Timestamp = timestamp.GetValueOrDefault(),
            },
            EventName.PaymentCancellationFailed => new PaymentCancellationFailed
            {
                Id = id.GetValueOrDefault(),
                Data = (PaymentCancellationFailedData)data!,
                Event = eventName.GetValueOrDefault(),
                MerchantId = merchantId.GetValueOrDefault(),
                Timestamp = timestamp.GetValueOrDefault(),
            },
            EventName.RefundCompleted => new RefundCompleted
            {
                Id = id.GetValueOrDefault(),
                Data = (RefundCompletedData)data!,
                Event = eventName.GetValueOrDefault(),
                MerchantId = merchantId.GetValueOrDefault(),
                Timestamp = timestamp.GetValueOrDefault(),
            },
            EventName.RefundFailed => new RefundFailed
            {
                Id = id.GetValueOrDefault(),
                Data = (RefundFailedData)data!,
                Event = eventName.GetValueOrDefault(),
                MerchantId = merchantId.GetValueOrDefault(),
                Timestamp = timestamp.GetValueOrDefault(),
            },
            EventName.RefundInitiated => new RefundInitiated
            {
                Id = id.GetValueOrDefault(),
                Data = (RefundInitiatedData)data!,
                Event = eventName.GetValueOrDefault(),
                MerchantId = merchantId.GetValueOrDefault(),
                Timestamp = timestamp.GetValueOrDefault(),
            },
            EventName.ReservationCreatedV1 => new ReservationCreatedV1
            {
                Id = id.GetValueOrDefault(),
                Data = (ReservationCreatedDataV1)data!,
                Event = eventName.GetValueOrDefault(),
                MerchantId = merchantId.GetValueOrDefault(),
                Timestamp = timestamp.GetValueOrDefault(),
            },
            EventName.ReservationCreatedV2 => new ReservationCreatedV2
            {
                Id = id.GetValueOrDefault(),
                Data = (ReservationCreatedDataV2)data!,
                Event = eventName.GetValueOrDefault(),
                MerchantId = merchantId.GetValueOrDefault(),
                Timestamp = timestamp.GetValueOrDefault(),
            },
            EventName.ReservationFailed => new ReservationFailed
            {
                Id = id.GetValueOrDefault(),
                Data = (ReservationFailedData)data!,
                Event = eventName.GetValueOrDefault(),
                MerchantId = merchantId.GetValueOrDefault(),
                Timestamp = timestamp.GetValueOrDefault(),
            },
            _ => throw new NotSupportedException("event not supported")
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, IWebhook<WebhookData> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        // Write id without dashes
        writer.WriteString("id", value.Id.ToString("N"));
        writer.WriteNumber("merchantId", value.MerchantId);
        writer.WriteString("timestamp", value.Timestamp);
        writer.WriteString("event", EventNameHelper.ToStringEventName(value.Event));

        writer.WritePropertyName("data");

        switch (value.Event)
        {
            case EventName.PaymentCreated:
                if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(PaymentCreatedData))) is not JsonConverter<PaymentCreatedData> paymentCreatedDataConverter)
                {
                    paymentCreatedDataConverter = new PaymentCreatedDataConverter();
                }
                paymentCreatedDataConverter.Write(writer, value.Data as PaymentCreatedData ?? new(), options);
                break;
            case EventName.PaymentCancelled:
                if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(PaymentCancelledData))) is not JsonConverter<PaymentCancelledData> paymentCancelledDataConverter)
                {
                    paymentCancelledDataConverter = new PaymentCancelledDataConverter();
                }
                paymentCancelledDataConverter.Write(writer, value.Data as PaymentCancelledData ?? new(), options);
                break;
            case EventName.ChargeCreated:
                if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(ChargeData))) is not JsonConverter<ChargeData> chargeDataConverter)
                {
                    chargeDataConverter = new ChargeCreatedDataConverter();
                }
                chargeDataConverter.Write(writer, value.Data as ChargeData ?? new(), options);
                break;
            case EventName.ChargeFailed:
                if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(ChargeFailedData))) is not JsonConverter<ChargeFailedData> chargeFailedConverter)
                {
                    chargeFailedConverter = new ChargeFailedDataConverter();
                }
                chargeFailedConverter.Write(writer, value.Data as ChargeFailedData ?? new(), options);
                break;
            case EventName.CheckoutCompleted:
                if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(CheckoutCompletedData))) is not JsonConverter<CheckoutCompletedData> checkoutCompletedConverter)
                {
                    checkoutCompletedConverter = new CheckoutCompletedDataConverter();
                }
                checkoutCompletedConverter.Write(writer, value.Data as CheckoutCompletedData ?? new(), options);
                break;
            case EventName.PaymentCancellationFailed:
                if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(PaymentCancellationFailedData))) is not JsonConverter<PaymentCancellationFailedData> paymentCancellationFailedConverter)
                {
                    paymentCancellationFailedConverter = new PaymentCancellationFailedDataConverter();
                }
                paymentCancellationFailedConverter.Write(writer, value.Data as PaymentCancellationFailedData ?? new(), options);
                break;
            case EventName.RefundCompleted:
                if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(RefundCompletedData))) is not JsonConverter<RefundCompletedData> refundCompletedDataConverter)
                {
                    refundCompletedDataConverter = new RefundCompletedDataConverter();
                }
                refundCompletedDataConverter.Write(writer, value.Data as RefundCompletedData ?? new(), options);
                break;
            case EventName.RefundFailed:
                if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(RefundFailedData))) is not JsonConverter<RefundFailedData> refundFailedConverter)
                {
                    refundFailedConverter = new RefundFailedDataConverter();
                }
                refundFailedConverter.Write(writer, value.Data as RefundFailedData ?? new(), options);
                break;
            case EventName.RefundInitiated:
                if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(RefundInitiatedData))) is not JsonConverter<RefundInitiatedData> refundInitiatedConverter)
                {
                    refundInitiatedConverter = new RefundInitiatedDataConverter();
                }
                refundInitiatedConverter.Write(writer, value.Data as RefundInitiatedData ?? new(), options);
                break;
            case EventName.ReservationCreatedV1:
                if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(ReservationCreatedDataV1))) is not JsonConverter<ReservationCreatedDataV1> reservationCreatedConverter)
                {
                    reservationCreatedConverter = new ReservationCreatedDataV1Converter();
                }
                reservationCreatedConverter.Write(writer, value.Data as ReservationCreatedDataV1 ?? new(), options);
                break;
            case EventName.ReservationCreatedV2:
                if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(ReservationCreatedDataV2))) is not JsonConverter<ReservationCreatedDataV2> reservationCreated2Converter)
                {
                    reservationCreated2Converter = new ReservationCreatedDataV2Converter();
                }
                reservationCreated2Converter.Write(writer, value.Data as ReservationCreatedDataV2 ?? new(), options);
                break;
            case EventName.ReservationFailed:
                break;
        }

        writer.WriteEndObject();
    }

    private static WebhookData? GetWebhookData(ref Utf8JsonReader reader, EventName eventName, JsonSerializerOptions options)
    {
        return eventName switch
        {
            EventName.PaymentCreated => GetPaymentCreatedData(ref reader, options),
            EventName.PaymentCancelled => GetPaymentCancelledData(ref reader, options),
            EventName.ChargeCreated => GetChargeData(ref reader, options),
            EventName.ChargeFailed => GetChargeFailedData(ref reader, options),
            EventName.CheckoutCompleted => GetCheckoutCompletedData(ref reader, options),
            EventName.PaymentCancellationFailed => GetPaymentCancellationFailedData(ref reader, options),
            EventName.RefundCompleted => GetRefundCompletedData(ref reader, options),
            EventName.RefundFailed => GetRefundFailedData(ref reader, options),
            EventName.RefundInitiated => GetRefundInitiatedData(ref reader, options),
            EventName.ReservationCreatedV1 => GetReservationCreatedDataV1(ref reader, options),
            EventName.ReservationCreatedV2 => GetReservationCreatedDataV2(ref reader, options),
            EventName.ReservationFailed => throw new NotImplementedException(),
            _ => throw new NotSupportedException("event not supported")
        };
    }

    private static PaymentCreatedData? GetPaymentCreatedData(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(PaymentCreatedData))) is not JsonConverter<PaymentCreatedData> paymentCreatedDataConverter)
        {
            paymentCreatedDataConverter = new PaymentCreatedDataConverter();
        }

        return paymentCreatedDataConverter.Read(ref reader, typeof(PaymentCreatedData), options);
    }

    private static PaymentCancelledData? GetPaymentCancelledData(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(PaymentCancelledData))) is not JsonConverter<PaymentCancelledData> paymentCancelledDataConverter)
        {
            paymentCancelledDataConverter = new PaymentCancelledDataConverter();
        }

        return paymentCancelledDataConverter.Read(ref reader, typeof(PaymentCreatedData), options);
    }

    private static ChargeData? GetChargeData(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(ChargeData))) is not JsonConverter<ChargeData> chargeDataConverter)
        {
            chargeDataConverter = new ChargeCreatedDataConverter();
        }

        return chargeDataConverter.Read(ref reader, typeof(ChargeData), options);
    }

    private static ChargeFailedData? GetChargeFailedData(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(ChargeFailedData))) is not JsonConverter<ChargeFailedData> chargeFailedConverter)
        {
            chargeFailedConverter = new ChargeFailedDataConverter();
        }

        return chargeFailedConverter.Read(ref reader, typeof(ChargeFailedData), options);
    }

    private static CheckoutCompletedData? GetCheckoutCompletedData(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(CheckoutCompletedData))) is not JsonConverter<CheckoutCompletedData> checkoutCompletedConverter)
        {
            checkoutCompletedConverter = new CheckoutCompletedDataConverter();
        }

        return checkoutCompletedConverter.Read(ref reader, typeof(CheckoutCompletedData), options);
    }

    private static PaymentCancellationFailedData? GetPaymentCancellationFailedData(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(PaymentCancellationFailedData))) is not JsonConverter<PaymentCancellationFailedData> converter)
        {
            converter = new PaymentCancellationFailedDataConverter();
        }

        return converter.Read(ref reader, typeof(PaymentCancellationFailedData), options);
    }

    private static RefundCompletedData? GetRefundCompletedData(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(RefundCompletedData))) is not JsonConverter<RefundCompletedData> converter)
        {
            converter = new RefundCompletedDataConverter();
        }

        return converter.Read(ref reader, typeof(RefundCompletedData), options);
    }

    private static RefundFailedData? GetRefundFailedData(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(RefundFailedData))) is not JsonConverter<RefundFailedData> converter)
        {
            converter = new RefundFailedDataConverter();
        }

        return converter.Read(ref reader, typeof(RefundFailedData), options);
    }

    private static RefundInitiatedData? GetRefundInitiatedData(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(RefundInitiatedData))) is not JsonConverter<RefundInitiatedData> converter)
        {
            converter = new RefundInitiatedDataConverter();
        }

        return converter.Read(ref reader, typeof(RefundInitiatedData), options);
    }
    private static ReservationCreatedDataV1? GetReservationCreatedDataV1(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(ReservationCreatedDataV1))) is not JsonConverter<ReservationCreatedDataV1> converter)
        {
            converter = new ReservationCreatedDataV1Converter();
        }

        return converter.Read(ref reader, typeof(ReservationCreatedDataV1), options);
    }

    private static ReservationCreatedDataV2? GetReservationCreatedDataV2(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(ReservationCreatedDataV2))) is not JsonConverter<ReservationCreatedDataV2> converter)
        {
            converter = new ReservationCreatedDataV2Converter();
        }

        return converter.Read(ref reader, typeof(ReservationCreatedDataV2), options);
    }
}
