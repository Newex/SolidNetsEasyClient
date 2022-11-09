using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// Converter for the <see cref="PaymentTypeMethodName"/>
/// </summary>
public class PaymentTypeMethodNameConverter : JsonConverter<PaymentTypeMethodName>
{
    /// <inheritdoc />
    public override PaymentTypeMethodName Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var json = reader.GetString();
        if (json is null)
        {
            throw new NotSupportedException("Cannot convert payment type or name");
        }

        var paymentType = PaymentTypeHelper.Convert(json);
        if (paymentType is not null)
        {
            return new PaymentTypeMethodName(paymentType.Value);
        }

        var hasEnum = Enum.TryParse<PaymentMethodEnum>(json, ignoreCase: true, out var paymentMethod);
        if (hasEnum)
        {
            return new PaymentTypeMethodName(paymentMethod);
        }

        throw new NotSupportedException("Cannot convert payment type or name");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, PaymentTypeMethodName value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
