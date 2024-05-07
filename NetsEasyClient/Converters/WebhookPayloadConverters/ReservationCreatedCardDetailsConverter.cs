using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Converters.WebhookPayloadConverters;

/// <summary>
/// Custom json converter
/// </summary>
public class ReservationCreatedCardDetailsConverter : JsonConverter<ReservationCreatedCardDetails>
{
    /// <inheritdoc />
    public override ReservationCreatedCardDetails? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonToken = reader.TokenType;
        if (jsonToken == JsonTokenType.Null || jsonToken == JsonTokenType.None)
        {
            return null;
        }

        string? creditDebitIndicator = null;
        int? expiryMonth = null;
        int? expiryYear = null;
        string? issuerCountry = null;
        string? truncatedPan = null;
        ReservationCreatedThreeDSecure? threeDSecure = null;

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
                    if (propertyName.Equals("creditDebitIndicator")) creditDebitIndicator = text;
                    else if (propertyName.Equals("issuerCountry")) issuerCountry = text;
                    else if (propertyName.Equals("truncatedPan")) truncatedPan = text;
                    break;
                case JsonTokenType.Number:
                    var number = reader.GetInt32();
                    if (propertyName.Equals("expiryMonth")) expiryMonth = number;
                    else if (propertyName.Equals("expiryYear")) expiryYear = number;
                    break;
                case JsonTokenType.StartObject:
                    if (propertyName.Equals("threeDSecure"))
                    {
                        threeDSecure = GetReservationCreatedThreeDSecure(ref reader);
                    }
                    break;
                case JsonTokenType.EndObject:
                    parsing = false;
                    break;
            }
        }

        if (creditDebitIndicator is null
            || !expiryMonth.HasValue
            || !expiryYear.HasValue
            || issuerCountry is null
            || truncatedPan is null
            || threeDSecure is null)
        {
            throw new JsonException("Missing properties, cannot deserialize to ReservationCreatedCardDetails.");
        }

        return new()
        {
            CreditDebitIndicator = creditDebitIndicator,
            ExpiryMonth = expiryMonth.GetValueOrDefault(),
            ExpiryYear = expiryYear.GetValueOrDefault(),
            IssuerCountry = issuerCountry,
            TruncatedPan = truncatedPan,
            ThreeDSecure = threeDSecure,
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ReservationCreatedCardDetails value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("creditDebitIndicator", value.CreditDebitIndicator);
        writer.WriteNumber("expiryMonth", value.ExpiryMonth);
        writer.WriteNumber("expiryYear", value.ExpiryYear);
        writer.WriteString("issuerCountry", value.IssuerCountry);
        writer.WriteString("truncatedPan", value.TruncatedPan);

        writer.WritePropertyName("threeDSecure");
        writer.WriteStartObject();
        writer.WriteString("authenticationEnrollmentStatus", value.ThreeDSecure.AuthenticationEnrollmentStatus);
        writer.WriteString("authenticationStatus", value.ThreeDSecure.AuthenticationStatus);
        writer.WriteString("eci", value.ThreeDSecure.ECI);
        writer.WriteEndObject();

        writer.WriteEndObject();
    }

    private ReservationCreatedThreeDSecure? GetReservationCreatedThreeDSecure(ref Utf8JsonReader reader)
    {
        string? authenticationEnrollmentStatus = null;
        string? authenticationStatus = null;
        string? eci = null;

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
                    if (propertyName.Equals("authenticationEnrollmentStatus")) authenticationEnrollmentStatus = text;
                    else if (propertyName.Equals("authenticationStatus")) authenticationStatus = text;
                    else if (propertyName.Equals("eci")) eci = text;
                    break;
                case JsonTokenType.EndObject:
                    parsing = false;
                    break;
            }
        }

        if (authenticationEnrollmentStatus is null || authenticationStatus is null || eci is null)
        {
            throw new JsonException("Could not parse ReservationCreatedThreeDSecure. Missing properties");
        }

        return new()
        {
            AuthenticationEnrollmentStatus = authenticationEnrollmentStatus,
            AuthenticationStatus = authenticationStatus,
            ECI = eci
        };
    }
}
