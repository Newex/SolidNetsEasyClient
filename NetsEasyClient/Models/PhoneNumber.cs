using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models;

/// <summary>
/// An international phone number
/// </summary>
public record PhoneNumber
{
    /// <summary>
    /// The country calling code
    /// </summary>
    /// <remarks>
    /// For example 001
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("prefix")]
    public string? Prefix { get; init; }

    /// <summary>
    /// The phone number
    /// </summary>
    /// <remarks>
    /// Without the country code prefix
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("number")]
    public string? Number { get; init; }
}
