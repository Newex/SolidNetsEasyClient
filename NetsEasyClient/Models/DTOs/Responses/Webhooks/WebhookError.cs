using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// An error DTO of a web hook
/// </summary>
public record WebhookError
{
    /// <summary>
    /// An internal error message. This message is not meant to be presented to the customer. Instead, this message can be logged and used for debugging purposes.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; init; }

    /// <summary>
    /// A numeric error code to be used for debugging purposes.
    /// </summary>
    [JsonPropertyName("code")]
    public string? Code { get; init; }

    /// <summary>
    /// The source of the error, for example: 'internal'.
    /// </summary>
    [JsonPropertyName("source")]
    public string? Source { get; init; }
}
