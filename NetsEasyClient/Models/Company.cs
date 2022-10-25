using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models;

/// <summary>
/// A business consumer
/// </summary>
public record Company
{
    /// <summary>
    /// The name of the company
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// The contact person
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("contact")]
    public Person? Contact { get; init; }
}
