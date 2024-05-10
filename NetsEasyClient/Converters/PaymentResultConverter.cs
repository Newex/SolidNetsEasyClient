using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// Custom payment result json converter
/// </summary>
public class PaymentResultConverter : JsonConverter<PaymentResult>
{
    /// <inheritdoc />
    public override PaymentResult? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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

        Guid? paymentId = null;
        string? hostedPaymentPageUrl = null;
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
                    else if (propertyName.Equals("hostedPaymentPageUrl")) hostedPaymentPageUrl = text;
                    break;
                case JsonTokenType.EndObject:
                    parsing = false;
                    break;
            }
        }

        if (!paymentId.HasValue)
        {
            throw new JsonException("Missing properties. Cannot deserialize to PaymentResult");
        }

        return new()
        {
            PaymentId = paymentId.GetValueOrDefault(),
            HostedPaymentPageUrl = hostedPaymentPageUrl
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, PaymentResult value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("paymentId", value.PaymentId.ToString("N"));
        writer.WriteString("hostedPaymentPageUrl", value.HostedPaymentPageUrl);
        writer.WriteEndObject();
    }
}
