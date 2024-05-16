using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Converters;


/// <summary>
/// Custom json converter
/// </summary>
public class ReservationCreatedV1Converter : JsonConverter<ReservationCreatedV1>
{
    /// <inheritdoc />
    public override ReservationCreatedV1? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var converter = new IWebhookConverter();
        var result = converter.Read(ref reader, typeof(IWebhook<WebhookData>), options);
        return result as ReservationCreatedV1;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ReservationCreatedV1 value, JsonSerializerOptions options)
    {
        var converter = new IWebhookConverter();
        converter.Write(writer, value, options);
    }
}
