using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

/// <summary>
/// The data associated with this event.
/// </summary>
public record PaymentCancelledData : WebhookData
{
    /// <summary>
    /// The cancellation id
    /// </summary>
    [Required]
    [JsonPropertyName("cancelId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid CancelId { get; init; }

    /// <summary>
    /// The list of order items that are associated with the canceled payment. Contains at least one order item.
    /// </summary>
    [Required]
    [JsonPropertyName("orderItems")]
    public IList<Item> OrderItems { get; init; } = new List<Item>();

    /// <summary>
    /// The amount of the charge.
    /// </summary>
    [Required]
    [JsonPropertyName("amount")]
    public WebhookAmount Amount { get; init; } = new();
}