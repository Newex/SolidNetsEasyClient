using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;

namespace SolidNetsEasyClient.Converters.WebhookPayloadConverters;

/// <summary>
/// Custom json converter
/// </summary>
public class WebhookInvoiceDetailsConverter : JsonConverter<WebhookInvoiceDetails>
{
    /// <inheritdoc />
    public override WebhookInvoiceDetails? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonToken = reader.TokenType;
        if (jsonToken == JsonTokenType.Null || jsonToken == JsonTokenType.None)
        {
            return null;
        }

        string? distributionType = null;
        DateOnly? invoiceDueDate = null;
        string? invoiceNumber = null;

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
                    var text = reader.GetString();
                    if (propertyName.Equals("distributionType")) distributionType = text;
                    else if (propertyName.Equals("invoiceDueDate")) invoiceDueDate = DateOnly.Parse(text!);
                    else if (propertyName.Equals("invoiceNumber")) invoiceNumber = text;
                    break;
                case JsonTokenType.EndObject:
                    parsing = false;
                    break;
            }
        }

        if (!invoiceDueDate.HasValue)
        {
            throw new JsonException("Missing properties to deserialize to WebhookInvoiceDetails.");
        }

        return new()
        {
            DistributionType = distributionType!,
            InvoiceDueDate = invoiceDueDate.GetValueOrDefault(),
            InvoiceNumber = invoiceNumber!
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, WebhookInvoiceDetails value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("distributionType", value.DistributionType);
        var date = value.InvoiceDueDate.ToString("yyyy-MM-dd\\T\\0\\0\\:\\0\\0\\:\\0\\0", CultureInfo.InvariantCulture);
        writer.WriteString("invoiceDueDate", date);
        writer.WriteString("invoiceNumber", value.InvoiceNumber);
        writer.WriteEndObject();
    }
}
