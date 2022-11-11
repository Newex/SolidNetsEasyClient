using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// The created order details
/// </summary>
public record WebhookOrder
{
    /// <summary>
    /// The amount of the charge.
    /// </summary>
    [JsonPropertyName("amount")]
    public WebhookAmount Amount { get; init; } = new();

    /// <summary>
    /// A reference to recognize this order. Usually a number sequence (order number).
    /// </summary>
    [JsonPropertyName("reference")]
    public string Reference { get; init; } = string.Empty;

    /// <summary>
    /// The list of order items that are associated with the charge. Contains at least one order item.
    /// </summary>
    [Required]
    [JsonPropertyName("orderItems")]
    public IList<Item> OrderItems { get; init; } = new List<Item>();
}