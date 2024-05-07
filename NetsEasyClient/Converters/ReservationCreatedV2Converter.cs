using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// Custom json converter
/// </summary>
public class ReservationCreatedV2Converter : JsonConverter<ReservationCreatedV2>
{
    /// <inheritdoc />
    public override ReservationCreatedV2? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var converter = new IWebhookConverter();
        var result = converter.Read(ref reader, typeof(IWebhook<WebhookData>), options);
        return result as ReservationCreatedV2;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ReservationCreatedV2 value, JsonSerializerOptions options)
    {
        var converter = new IWebhookConverter();
        converter.Write(writer, value, options);
    }
}
