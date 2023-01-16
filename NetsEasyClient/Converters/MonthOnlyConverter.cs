using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// Month only json converter
/// </summary>
public class MonthOnlyConverter : JsonConverter<MonthOnly>
{
    /// <inheritdoc />
    public override MonthOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var json = reader.GetString();
        if (json is null)
        {
            throw new FormatException("The month only value is null");
        }

        if (json.Length != 4)
        {
            throw new FormatException("The json month only format must be in yyMM");
        }

        var value = json.AsSpan();
        var MM = value[..2];
        var month = int.Parse(MM, CultureInfo.InvariantCulture);

        var YY = value.Slice(2, 2);
        var year = int.Parse(YY, CultureInfo.InvariantCulture);

        return new MonthOnly(year, month);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, MonthOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
