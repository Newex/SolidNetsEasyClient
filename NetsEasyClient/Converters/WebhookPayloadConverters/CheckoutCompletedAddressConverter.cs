using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Converters.WebhookPayloadConverters;

/// <summary>
/// Custom json converter
/// </summary>
public class CheckoutCompletedAddressConverter : JsonConverter<CheckoutCompletedAddress>
{
    /// <inheritdoc />
    public override CheckoutCompletedAddress? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonToken = reader.TokenType;
        if (jsonToken == JsonTokenType.Null || jsonToken == JsonTokenType.None)
        {
            return null;
        }

        string? addressLine1 = null;
        string? addressLine2 = null;
        string? city = null;
        string? country = null;
        string? postCode = null;
        string? receiverLine = null;

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
                    if (propertyName.Equals("addressLine1")) addressLine1 = text;
                    else if (propertyName.Equals("addressLine2")) addressLine2 = text;
                    else if (propertyName.Equals("city")) city = text;
                    else if (propertyName.Equals("country")) country = text;

                    // NOTE: In the NETS example they write lowercase, in documentation they write camelCase.
                    // We compromise and accept any.
                    else if (propertyName.Equals("postCode", StringComparison.OrdinalIgnoreCase)) postCode = text;
                    else if (propertyName.Equals("receiverLine")) receiverLine = text;
                    break;
                case JsonTokenType.EndObject:
                    parsing = false;
                    break;
            }
        }

        return new()
        {
            AddressLine1 = addressLine1 ?? "",
            AddressLine2 = addressLine2 ?? "",
            City = city ?? "",
            Country = country ?? "",
            PostCode = postCode ?? "",
            ReceiverLine = receiverLine ?? "",
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, CheckoutCompletedAddress value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString("addressLine1", value.AddressLine1);
        writer.WriteString("addressLine2", value.AddressLine2);
        writer.WriteString("city", value.City);
        writer.WriteString("country", value.Country);
        writer.WriteString("postCode", value.PostCode);
        writer.WriteString("receiverLine", value.ReceiverLine);

        writer.WriteEndObject();
    }
}
