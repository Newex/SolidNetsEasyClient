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
public record ChargeFailedData : WebhookData
{
    /// <summary>
    /// Contains information about an error (client error or server error).
    /// </summary>
    [Required]
    [JsonPropertyName("error")]
    public WebhookError Error { get; init; } = new();

    /// <summary>
    /// The charge identifier.
    /// </summary>
    [Required]
    [JsonPropertyName("chargeId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid ChargeId { get; init; }

    /// <summary>
    /// The list of order items that are associated with the canceled payment. Contains at least one order item.
    /// </summary>
    [Required]
    [JsonPropertyName("orderItems")]
    public IList<Item> OrderItems { get; init; } = [];

    /// <summary>
    /// A unique identifier (UUID) for the reservation that can help in diagnostics.
    /// </summary>
    [Required]
    [JsonPropertyName("reservationId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid ReservationId { get; init; }

    /// <summary>
    /// The amount of the charge.
    /// </summary>
    [Required]
    [JsonPropertyName("amount")]
    public WebhookAmount Amount { get; init; } = new();
}