using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// The webhook DTO for when a payment has failed to be cancelled
/// </summary>
/// <remarks>
/// The payment.cancel.failed event is triggered when a cancellation of a reservation has failed.
/// </remarks>
public record PaymentCancelledFailed : Webhook<PaymentCancelledData> { }

/// <summary>
/// The data associated with this event.
/// </summary>
public record PaymentCancelledFailedData
{
    /// <summary>
    /// The payment identifier.
    /// </summary>
    [Required]
    [JsonConverter(typeof(GuidTypeConverter))]
    [JsonPropertyName("paymentId")]
    public Guid PaymentId { get; init; }

    /// <summary>
    /// Contains information about an error (client error or server error).
    /// </summary>
    [Required]
    [JsonPropertyName("error")]
    public PaymentCancelledFailedError Error { get; init; } = new();

    /// <summary>
    /// The cancellation id
    /// </summary>
    [Required]
    [JsonConverter(typeof(GuidTypeConverter))]
    [JsonPropertyName("cancelId")]
    public Guid CancelId { get; init; }

    /// <summary>
    /// The list of order items that are associated with the charge. Contains at least one order item.
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

/// <summary>
/// An error object for the payment cancellation
/// </summary>
public record PaymentCancelledFailedError
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
