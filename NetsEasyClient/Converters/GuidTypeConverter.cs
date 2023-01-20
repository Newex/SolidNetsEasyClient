using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// Convert string to Guid
/// </summary>
public class GuidTypeConverter : JsonConverter<Guid>
{
    /// <summary>
    /// Deserialize json string to a Guid
    /// </summary>
    /// <param name="reader">The reader</param>
    /// <param name="typeToConvert">The type to convert to</param>
    /// <param name="options">The serializer options</param>
    /// <returns>A Guid</returns>
    /// <exception cref="FormatException">Thrown when json cannot be parsed into a <see cref="Guid"/></exception>
    public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var json = reader.GetString();
        if (json is null)
        {
            throw new FormatException("Null json string, cannot be parsed into a Guid");
        }

        var guid = new Guid(json);
        return guid;
    }

    /// <summary>
    /// Serialize a Guid to a json string
    /// </summary>
    /// <param name="writer">The writer</param>
    /// <param name="value">The Guid value</param>
    /// <param name="options">The serializer options</param>
    public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("N"));
    }
}

/// <summary>
/// Convert string to a nullable Guid
/// </summary>
public class NullableGuidTypeConverter : JsonConverter<Guid?>
{
    /// <summary>
    /// Deserialize string to nullable guid
    /// </summary>
    /// <param name="reader">The reader</param>
    /// <param name="typeToConvert">The type to convert</param>
    /// <param name="options">The serializer options</param>
    /// <returns>A nullable guid</returns>
    public override Guid? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var json = reader.GetString();
        if (json is null)
        {
            return null;
        }

        var guid = new Guid(json);
        return guid;
    }

    /// <summary>
    /// Serialize a Guid to a json string
    /// </summary>
    /// <param name="writer">The writer</param>
    /// <param name="value">The Guid value</param>
    /// <param name="options">The serializer options</param>
    public override void Write(Utf8JsonWriter writer, Guid? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            return;
        }

        writer.WriteStringValue(value?.ToString("N"));
    }
}
