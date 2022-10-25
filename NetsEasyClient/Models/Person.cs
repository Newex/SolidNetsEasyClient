using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models;

/// <summary>
/// A natural person
/// </summary>
public record Person
{
    /// <summary>
    /// The first name
    /// </summary>
    /// <remarks>
    ///  Also known as given name
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("firstName")]
    public string? FirstName { get; init; }

    /// <summary>
    /// The last name
    /// </summary>
    /// <remarks>
    ///  Also known as surname/family name
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("lastName")]
    public string? LastName { get; init; }
}
