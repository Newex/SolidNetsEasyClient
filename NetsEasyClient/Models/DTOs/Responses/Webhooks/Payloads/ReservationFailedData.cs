using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

/// <summary>
/// The failed reservation data
/// </summary>
public record ReservationFailedData
{
    /// <summary>
    /// The payment identifier.
    /// </summary>
    [Required]
    [JsonConverter(typeof(GuidTypeConverter))]
    [JsonPropertyName("paymentId")]
    public Guid PaymentId { get; init; }

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