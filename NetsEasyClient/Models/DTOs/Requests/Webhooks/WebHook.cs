using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Webhooks;

/// <summary>
/// Represents a webhook, which allows for HTTP callbacks upon subscribing to different events
/// </summary>
public record WebHook
{
    /// <summary>
    /// The name of the event you want to subscribe to
    /// </summary>
    [JsonConverter(typeof(EventNameConverter))]
    [JsonPropertyName("eventName")]
    public EventNames? EventName { get; init; }

    /// <summary>
    /// The callback is sent to this URL
    /// </summary>
    /// <remarks>
    /// Must be HTTPS to ensure a secure communication. Maximum allowed length of the URL is 256 characters
    /// </remarks>
    [JsonPropertyName("url")]
    public string? Url { get; init; }

    /// <summary>
    /// The credentials that will be sent in the HTTP Authorization request header of the callback
    /// </summary>
    /// <remarks>
    /// Must be between 8 and 32 characters long and contain alphanumeric characters
    /// </remarks>
    [JsonPropertyName("authorization")]
    public string? Authorization { get; init; }
}
