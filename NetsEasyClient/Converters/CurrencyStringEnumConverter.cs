using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// Converter for <see cref="Currency"/>
/// </summary>
public class CurrencyStringEnumConverter : JsonConverter<Currency>
{
    /// <inheritdoc />
    public override Currency Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var input = reader.GetString();
        var hasCurrency = Enum.TryParse<Currency>(input, ignoreCase: true, out var currency);
        if (!hasCurrency)
        {
            throw new NotSupportedException("Currency not supported. See list of supported currencies at Nets Easy Developer page");
        }

        return currency;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Currency value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString().ToUpperInvariant());
    }
}
