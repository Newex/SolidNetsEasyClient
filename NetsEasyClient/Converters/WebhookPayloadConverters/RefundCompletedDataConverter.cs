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
public class RefundCompletedDataConverter : JsonConverter<RefundCompletedData>
{
    /// <inheritdoc />
    public override RefundCompletedData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonToken = reader.TokenType;
        if (jsonToken == JsonTokenType.Null || jsonToken == JsonTokenType.None)
        {
            return null;
        }

        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookAmount))) is not JsonConverter<WebhookAmount> webhookAmountConverter)
        {
            webhookAmountConverter = new WebhookAmountConverter();
        }
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookInvoiceDetails))) is not JsonConverter<WebhookInvoiceDetails> webhookInvoiceConverter)
        {
            webhookInvoiceConverter = new WebhookInvoiceDetailsConverter();
        }

        Guid? paymentId = null;
        Guid? refundId = null;
        WebhookAmount? amount = null;
        WebhookInvoiceDetails? invoiceDetails = null;

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
                    else if (propertyName.Equals("refundId")) refundId = new Guid(text);
                    break;
                case JsonTokenType.StartObject:
                    if (propertyName.Equals("amount"))
                    {
                        amount = webhookAmountConverter.Read(ref reader, typeof(WebhookAmount), options);
                    }
                    else if (propertyName.Equals("invoiceDetails"))
                    {
                        invoiceDetails = webhookInvoiceConverter.Read(ref reader, typeof(WebhookInvoiceDetails), options);
                    }
                    break;
                case JsonTokenType.EndObject:
                    parsing = false;
                    break;
            }
        }

        if (!paymentId.HasValue || !refundId.HasValue || amount is null || invoiceDetails is null)
        {
            throw new JsonException("Could not parse RefundCompledData. Missing properties.");
        }

        return new()
        {
            PaymentId = paymentId.GetValueOrDefault(),
            RefundId = refundId.GetValueOrDefault(),
            Amount = amount,
            InvoiceDetails = invoiceDetails
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, RefundCompletedData value, JsonSerializerOptions options)
    {
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookAmount))) is not JsonConverter<WebhookAmount> webhookAmountConverter)
        {
            webhookAmountConverter = new WebhookAmountConverter();
        }
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(WebhookInvoiceDetails))) is not JsonConverter<WebhookInvoiceDetails> webhookInvoiceConverter)
        {
            webhookInvoiceConverter = new WebhookInvoiceDetailsConverter();
        }

        writer.WriteStartObject();
        writer.WriteString("paymentId", value.PaymentId.ToString("N"));
        writer.WriteString("refundId", value.RefundId.ToString("N"));

        writer.WritePropertyName("amount");
        webhookAmountConverter.Write(writer, value.Amount, options);

        writer.WritePropertyName("invoiceDetails");
        webhookInvoiceConverter.Write(writer, value.InvoiceDetails, options);

        writer.WriteEndObject();
    }
}
