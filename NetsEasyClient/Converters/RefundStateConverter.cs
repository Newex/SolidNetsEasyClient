using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.Status;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// Converter for the <see cref="RefundStateEnum"/>
/// </summary>
public class RefundStateConverter : JsonConverter<RefundStateEnum>
{
    /// <inheritdoc />
    public override RefundStateEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var json = reader.GetString();
        if (json is null)
        {
            throw new NotSupportedException("Cannot convert refund state to enum");
        }

        var hasEnum = Enum.TryParse<RefundStateEnum>(json, ignoreCase: true, out var state);
        if (!hasEnum)
        {
            throw new NotSupportedException("Cannot convert refund state to enum");
        }

        return state;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, RefundStateEnum value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
