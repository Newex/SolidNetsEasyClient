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
    /// <exception cref="NullReferenceException">If the json string is null</exception>
    public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var json = reader.GetString();
        if (json is null)
        {
            throw new NullReferenceException("Null json string, cannot be parsed into a Guid");
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
