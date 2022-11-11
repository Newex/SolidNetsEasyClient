using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

/// <summary>
/// The checkout completed data payload
/// </summary>
public record CheckoutCompletedData
{
    /// <summary>
    /// The payment identifier
    /// </summary>
    [Required]
    [JsonConverter(typeof(GuidTypeConverter))]
    [JsonPropertyName("paymentId")]
    public Guid PaymentId { get; init; }

    /// <summary>
    /// Specifies the order associated with the payment.
    /// </summary>
    [Required]
    [JsonPropertyName("order")]
    public WebhookOrder Order { get; init; } = new();

    /// <summary>
    /// The consumer (from the example)
    /// </summary>
    [JsonPropertyName("consumer")]
    public CheckoutCompletedConsumer? Consumer { get; init; }
}
