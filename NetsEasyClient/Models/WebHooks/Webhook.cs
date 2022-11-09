using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.WebHooks;

/// <summary>
/// A basic webhook event structure
/// </summary>
/// <typeparam name="T">The webhook data</typeparam>
public abstract record Webhook<T>
where T : new()
{
    /// <summary>
    /// A unique identifier of this event. You can use this identifier to detect whether this event is new or has already been handled by you.
    /// </summary>
    [JsonConverter(typeof(GuidTypeConverter))]
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
    [JsonConverter(typeof(DateTimeOffsetConverter))]
    public DateTimeOffset Timestamp { get; init; }

    /// <summary>
    /// The name of the event
    /// </summary>
    [Required]
    [JsonPropertyName("event")]
    public EventNames Event { get; init; } = EventNames.Payment.PaymentCreated;

    /// <summary>
    /// The data associated with this event
    /// </summary>
    [Required]
    [JsonPropertyName("data")]
    public T Data { get; init; } = new();
}
