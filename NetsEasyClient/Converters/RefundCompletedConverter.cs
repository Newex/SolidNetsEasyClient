using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// Custom converter
/// </summary>
public class RefundCompletedConverter : JsonConverter<RefundCompleted>
{
    /// <inheritdoc />
    public override RefundCompleted? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var converter = new IWebhookConverter();
        var result = converter.Read(ref reader, typeof(IWebhook<WebhookData>), options);
        return result as RefundCompleted;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, RefundCompleted value, JsonSerializerOptions options)
    {
        var converter = new IWebhookConverter();
        converter.Write(writer, value, options);
    }
}
