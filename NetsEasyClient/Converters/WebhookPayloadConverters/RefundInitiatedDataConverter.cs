using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Converters.WebhookPayloadConverters;

/// <summary>
/// Custom json converter
/// </summary>
public class RefundInitiatedDataConverter : JsonConverter<RefundInitiatedData>
{
    /// <inheritdoc />
    public override RefundInitiatedData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonToken = reader.TokenType;
        if (jsonToken == JsonTokenType.None)
        {
            // try to advance once
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

        Guid? paymentId = null;
        Guid? refundId = null;
        Guid? chargeId = null;
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
                    else if (propertyName.Equals("refundId")) refundId = new Guid(text);
                    else if (propertyName.Equals("chargeId")) chargeId = new Guid(text);
                    break;
                case JsonTokenType.StartObject:
                    if (propertyName.Equals("amount"))
                    {
                        amount = webhookAmountConverter.Read(ref reader, typeof(WebhookAmount), options);
                    }
                    break;
                case JsonTokenType.EndObject:
                    parsing = false;
                    break;
            }
        }

        if (!paymentId.HasValue || !refundId.HasValue || !chargeId.HasValue || amount is null)
        {
            throw new JsonException("Missing properties, cannot deserialize to RefundInitiatedData.");
        }

        return new()
        {
            PaymentId = paymentId.GetValueOrDefault(),
            RefundId = refundId.GetValueOrDefault(),
            ChargeId = chargeId.GetValueOrDefault(),
            Amount = amount
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, RefundInitiatedData value, JsonSerializerOptions options)
    {
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookAmount))) is not JsonConverter<WebhookAmount> webhookAmountConverter)
        {
            webhookAmountConverter = new WebhookAmountConverter();
        }

        writer.WriteStartObject();
        writer.WriteString("paymentId", value.PaymentId.ToString("N"));
        writer.WriteString("refundId", value.RefundId.ToString("N"));
        writer.WriteString("chargeId", value.ChargeId.ToString("N"));

        writer.WritePropertyName("amount");
        webhookAmountConverter.Write(writer, value.Amount, options);

        writer.WriteEndObject();
    }
}
