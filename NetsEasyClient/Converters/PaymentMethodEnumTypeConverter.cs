using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// Converter for the <see cref="PaymentMethodEnum"/>
/// </summary>
public class PaymentMethodEnumTypeConverter : JsonConverter<PaymentMethodEnum>
{
    /// <inheritdoc />
    public override PaymentMethodEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var json = reader.GetString();
        if (json is null)
        {
            throw new NotSupportedException("Cannot convert payment method to enum");
        }

        var hasEnum = Enum.TryParse<PaymentMethodEnum>(json, ignoreCase: true, out var method);
        if (!hasEnum)
        {
            throw new NotSupportedException("Cannot convert payment method to enum");
        }

        return method;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, PaymentMethodEnum value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
