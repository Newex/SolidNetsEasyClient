using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// Custom json converter
/// </summary>
public class ReservationFailedConverter : JsonConverter<ReservationFailed>
{
    /// <inheritdoc />
    public override ReservationFailed? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var converter = new IWebhookConverter();
        var result = converter.Read(ref reader, typeof(IWebhook<WebhookData>), options);
        return result as ReservationFailed;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ReservationFailed value, JsonSerializerOptions options)
    {
        var converter = new IWebhookConverter();
        converter.Write(writer, value, options);
    }
}
