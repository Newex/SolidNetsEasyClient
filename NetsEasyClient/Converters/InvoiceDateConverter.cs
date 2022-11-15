using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// A converter for the invoice date only parameter
/// </summary>
public class InvoiceDateConverter : JsonConverter<DateOnly>
{
    /// <inheritdoc />
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var json = reader.GetString();
        if (json is null)
        {
            throw new NotSupportedException();
        }

        var dateOnly = DateOnly.Parse(json);
        return dateOnly;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        var date = value.ToString("yyyy-MM-dd\\T\\0\\0\\:\\0\\0\\:\\0\\0");
        writer.WriteStringValue(date);
    }
}
