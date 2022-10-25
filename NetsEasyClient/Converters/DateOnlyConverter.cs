using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// Converter for <see cref="DateOnly"/>
/// </summary>
public class DateOnlyConverter : JsonConverter<DateOnly?>
{
    /// <inheritdoc />
    public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var json = reader.GetString();
        if (json is null)
        {
            return null;
        }

        var dateOnly = DateOnly.ParseExact(json, "yyMM");
        var lastDayInMonth = new DateOnly(dateOnly.Year, dateOnly.Month, DateTime.DaysInMonth(dateOnly.Year, dateOnly.Month));
        return lastDayInMonth;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
    {
        if (value is null)
            return;

        writer.WriteStringValue(value.Value.ToString("yyMM"));
    }
}
