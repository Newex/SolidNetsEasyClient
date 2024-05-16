using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// Custom converter for charge created webhook callback
/// </summary>
public class ChargeCreatedConverter : JsonConverter<ChargeCreated>
{
    /// <inheritdoc />
    public override ChargeCreated? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var converter = new IWebhookConverter();
        var result = converter.Read(ref reader, typeof(IWebhook<WebhookData>), options);
        return result as ChargeCreated;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ChargeCreated value, JsonSerializerOptions options)
    {
        var converter = new IWebhookConverter();
        converter.Write(writer, value, options);
    }
}
