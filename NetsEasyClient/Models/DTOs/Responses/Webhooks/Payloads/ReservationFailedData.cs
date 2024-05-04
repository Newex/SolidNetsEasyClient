using System.Collections.Generic;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

/// <summary>
/// The failed reservation data
/// </summary>
public record ReservationFailedData : WebhookData
{
    /// <summary>
    /// The amount of the charge.
    /// </summary>
    [JsonPropertyName("amount")]
    public WebhookAmount Amount { get; init; } = new();

    /// <summary>
    /// Order items as in the example payload
    /// </summary>
    [JsonPropertyName("orderItems")]
    public IList<Item> OrderItems { get; set; } = new List<Item>();

    /// <summary>
    /// The error details. (From example)
    /// </summary>
    [JsonPropertyName("error")]
    public WebhookError Error { get; init; } = new();
}