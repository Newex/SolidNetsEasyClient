using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// A basic webhook event structure
/// </summary>
/// <typeparam name="T">The webhook data</typeparam>
[JsonDerivedType(typeof(PaymentCreated))]
public abstract record Webhook<T>
where T : WebhookData, new()
{
    /// <summary>
    /// A unique identifier of this event. You can use this identifier to detect whether this event is new or has already been handled by you.
    /// </summary>
    [JsonPropertyName("id")]
    [Required]
    public Guid Id { get; init; }

    /// <summary>
    /// The merchant number.
    /// </summary>
    [Required]
    [JsonPropertyName("merchantId")]
    public int MerchantId { get; init; }

    /// <summary>
    /// The time at which the event occurred formatted according to RFC339, for example 2021-03-23T15:30:55.23Z.
    /// </summary>
    [Required]
    [JsonPropertyName("timestamp")]
    public DateTimeOffset Timestamp { get; init; }

    /// <summary>
    /// The name of the event
    /// </summary>
    [Required]
    [JsonPropertyName("event")]
    [JsonConverter(typeof(EventNameConverter))]
    public EventName Event { get; init; } = EventName.PaymentCreated;

    /// <summary>
    /// The data associated with this event
    /// </summary>
    [Required]
    [JsonPropertyName("data")]
    public T Data { get; init; } = new();
}
