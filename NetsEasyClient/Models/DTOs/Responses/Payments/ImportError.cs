using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Payments;

/// <summary>
/// Represents an error that occurred during the import of a subscription from an external ecommerce system.
/// </summary>
public record ImportError
{
    /// <summary>
    /// The error code.
    /// </summary>
    [JsonPropertyName("importStepsResponseCode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ImportStepsResponseCode { get; init; }

    /// <summary>
    /// The source of the error.
    /// </summary>
    [JsonPropertyName("importStepsResponseSource")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ImportStepsResponseSource { get; init; }

    /// <summary>
    /// The error message.
    /// </summary>
    [JsonPropertyName("importStepsResponseText")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ImportStepsResponseText { get; init; }
}