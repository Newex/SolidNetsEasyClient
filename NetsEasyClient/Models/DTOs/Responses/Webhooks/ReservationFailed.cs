using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// A web hook payment reservation failed event DTO
/// </summary>
public record ReservationFailed : Webhook<ReservationFailedData>
{
    /// <summary>
    /// The list of order items that are associated with the failed reservation and charge. Contains at least one order item.
    /// </summary>
    public IList<Item> OrderItems
    {
        get { return Data.OrderItems; }
        init { Data.OrderItems = value; }
    }
}

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
    public ReservationFailedError Error { get; init; } = new();
}

/// <summary>
/// The reservation error details
/// </summary>
public record ReservationFailedError
{
    /// <summary>
    /// The error code
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; init; } = string.Empty;

    /// <summary>
    /// The error message
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// The error source
    /// </summary>
    [JsonPropertyName("source")]
    public string Source { get; init; } = string.Empty;
}
