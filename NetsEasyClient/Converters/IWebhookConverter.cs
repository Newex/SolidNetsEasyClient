using System;
using System.Linq;
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
                        // Parse data depending on event name
                        // Assumption: eventname is parsed before data!
                        if (eventName is null)
                        {
                            continue;
                        }

                        data = eventName.GetValueOrDefault() switch
                        {
                            EventName.PaymentCreated => GetPaymentCreatedData(ref reader, options),
                            EventName.PaymentCancelled => GetPaymentCancelledData(ref reader, options),
                            EventName.ChargeCreated => throw new NotImplementedException(),
                            EventName.ChargeFailed => throw new NotImplementedException(),
                            EventName.CheckoutCompleted => throw new NotImplementedException(),
                            EventName.PaymentCancellationFailed => throw new NotImplementedException(),
                            EventName.RefundCompleted => throw new NotImplementedException(),
                            EventName.RefundFailed => throw new NotImplementedException(),
                            EventName.RefundInitiated => throw new NotImplementedException(),
                            EventName.ReservationCreatedV1 => throw new NotImplementedException(),
                            EventName.ReservationCreatedV2 => throw new NotImplementedException(),
                            EventName.ReservationFailed => throw new NotImplementedException(),
                            _ => throw new NotSupportedException("event not supported")
                        };
                    }
                    break;
            }
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
                break;
            case EventName.ChargeFailed:
                break;
            case EventName.CheckoutCompleted:
                break;
            case EventName.PaymentCancellationFailed:
                break;
            case EventName.RefundCompleted:
                break;
            case EventName.RefundFailed:
                break;
            case EventName.RefundInitiated:
                break;
            case EventName.ReservationCreatedV1:
                break;
            case EventName.ReservationCreatedV2:
                break;
            case EventName.ReservationFailed:
                break;
        }

        writer.WriteEndObject();
    }

    private WebhookData? GetPaymentCreatedData(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(PaymentCreatedData))) is not JsonConverter<PaymentCreatedData> paymentCreatedDataConverter)
        {
            paymentCreatedDataConverter = new PaymentCreatedDataConverter();
        }

        return paymentCreatedDataConverter.Read(ref reader, typeof(PaymentCreatedData), options);
    }

    private static WebhookData? GetPaymentCancelledData(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(PaymentCancelledData))) is not JsonConverter<PaymentCancelledData> paymentCancelledDataConverter)
        {
            paymentCancelledDataConverter = new PaymentCancelledDataConverter();
        }

        return paymentCancelledDataConverter.Read(ref reader, typeof(PaymentCreatedData), options);
    }
}
