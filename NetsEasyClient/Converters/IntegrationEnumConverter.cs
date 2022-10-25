using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// Converter for <see cref="Integration"/>
/// </summary>
public class IntegrationEnumConverter : JsonConverter<Integration>
{
    /// <inheritdoc />
    public override Integration Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var text = reader.GetString();
        var isIntegrationType = Enum.TryParse<Integration>(text, ignoreCase: true, out var integration);
        if (!isIntegrationType)
        {
            throw new NotSupportedException("Cannot convert value to IntegrationType");
        }

        return integration;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Integration value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
