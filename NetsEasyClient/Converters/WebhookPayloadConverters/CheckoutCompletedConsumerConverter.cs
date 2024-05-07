using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Contacts;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Converters.WebhookPayloadConverters;

/// <summary>
/// Custom json converter
/// </summary>
public class CheckoutCompletedConsumerConverter : JsonConverter<CheckoutCompletedConsumer>
{
    /// <inheritdoc />
    public override CheckoutCompletedConsumer? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonToken = reader.TokenType;
        if (jsonToken == JsonTokenType.Null || jsonToken == JsonTokenType.None)
        {
            return null;
        }

        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(CheckoutCompletedAddress))) is not JsonConverter<CheckoutCompletedAddress> checkoutCompletedAddressConverter)
        {
            checkoutCompletedAddressConverter = new CheckoutCompletedAddressConverter();
        }
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(PhoneNumber))) is not JsonConverter<PhoneNumber> phoneNumberConverter)
        {
            phoneNumberConverter = new PhoneNumberConverter();
        }

        string? firstname = null;
        string? lastname = null;
        CheckoutCompletedAddress? billingAddress = null;
        string? country = null;
        string? email = null;
        string? ip = null;
        PhoneNumber? phoneNumber = null;
        CheckoutCompletedAddress? shippingAddress = null;

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
                    if (propertyName.Equals("firstName")) firstname = text;
                    else if (propertyName.Equals("lastName")) lastname = text;
                    else if (propertyName.Equals("country")) country = text;
                    else if (propertyName.Equals("email")) email = text;
                    else if (propertyName.Equals("ip")) ip = text;
                    break;
                case JsonTokenType.StartObject:
                    if (propertyName.Equals("billingAddress"))
                    {
                        billingAddress = checkoutCompletedAddressConverter.Read(ref reader, typeof(CheckoutCompletedAddress), options);
                    }
                    else if (propertyName.Equals("shippingAddress"))
                    {
                        shippingAddress = checkoutCompletedAddressConverter.Read(ref reader, typeof(CheckoutCompletedAddress), options);
                    }
                    else if (propertyName.Equals("phoneNumber"))
                    {
                        phoneNumber = phoneNumberConverter.Read(ref reader, typeof(PhoneNumber), options);
                    }
                    break;
                case JsonTokenType.EndObject:
                    // Assumption: The other converters "eat" the EndObject token
                    parsing = false;
                    break;
            }
        }

        return new()
        {
            FirstName = firstname,
            LastName = lastname,
            BillingAddress = billingAddress,
            Country = country,
            Email = email,
            IP = ip,
            PhoneNumber = phoneNumber,
            ShippingAddress = shippingAddress,
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, CheckoutCompletedConsumer value, JsonSerializerOptions options)
    {
        var omitNull = options.DefaultIgnoreCondition.HasFlag(JsonIgnoreCondition.WhenWritingNull);
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(CheckoutCompletedAddress))) is not JsonConverter<CheckoutCompletedAddress> checkoutCompletedAddressConverter)
        {
            checkoutCompletedAddressConverter = new CheckoutCompletedAddressConverter();
        }
        if (options.Converters.FirstOrDefault(x => x.CanConvert(typeof(PhoneNumber))) is not JsonConverter<PhoneNumber> phoneNumberConverter)
        {
            phoneNumberConverter = new PhoneNumberConverter();
        }

        writer.WriteStartObject();
        writer.WriteString("firstName", value.FirstName);
        writer.WriteString("lastName", value.LastName);

        if (value.BillingAddress is not null)
        {
            writer.WritePropertyName("billingAddress");
            checkoutCompletedAddressConverter.Write(writer, value.BillingAddress, options);
        }
        else if (!omitNull)
        {
            writer.WritePropertyName("billingAddress");
            writer.WriteNullValue();
        }

        writer.WriteString("country", value.Country);
        writer.WriteString("email", value.Email);
        writer.WriteString("ip", value.IP);

        if (value.PhoneNumber is not null)
        {
            writer.WritePropertyName("phoneNumber");
            phoneNumberConverter.Write(writer, value.PhoneNumber, options);
        }
        else if (!omitNull)
        {
            writer.WriteNull("phoneNumber");
        }

        if (value.ShippingAddress is not null)
        {
            writer.WritePropertyName("shippingAddress");
            checkoutCompletedAddressConverter.Write(writer, value.ShippingAddress, options);
        }
        else if (!omitNull)
        {
            writer.WriteNull("shippingAddress");
        }

        writer.WriteEndObject();
    }
}
