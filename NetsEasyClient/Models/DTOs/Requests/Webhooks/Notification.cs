using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Webhooks;

/// <summary>
/// Notification allows you to subscribe to status updates for a payment
/// </summary>
public record Notification
{
    /// <summary>
    /// The list of webooks
    /// </summary>
    /// <remarks>
    /// The maximum number of webhooks is 32
    /// </remarks>
    [JsonPropertyName("webHooks")]
    public IEnumerable<WebHook> WebHooks { get; init; } = Enumerable.Empty<WebHook>();
}
