using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// Custom json converter for payment created webhook callback
/// </summary>
public class PaymentCreatedConverter : JsonConverter<PaymentCreated>
{
    /// <inheritdoc />
    public override PaymentCreated? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var converter = new IWebhookConverter();
        var result = converter.Read(ref reader, typeof(IWebhook<WebhookData>), options);
        return result as PaymentCreated;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, PaymentCreated value, JsonSerializerOptions options)
    {
        var converter = new IWebhookConverter();
        converter.Write(writer, value, options);
    }
}
