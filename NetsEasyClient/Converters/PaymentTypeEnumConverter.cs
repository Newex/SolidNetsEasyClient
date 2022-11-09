using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// Converter for the <see cref="PaymentTypeEnum"/>
/// </summary>
public class PaymentTypeEnumConverter : JsonConverter<PaymentTypeEnum>
{
    /// <inheritdoc />
    public override PaymentTypeEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var json = reader.GetString();
        if (json is null)
        {
            throw new NotSupportedException("Cannot convert payment type to enum");
        }

        return PaymentTypeHelper.Convert(json) ?? throw new NotSupportedException("Cannot convert payment type to enum");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, PaymentTypeEnum value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.GetName());
    }
}
