using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

/// <summary>
/// The data associated with this event.
/// </summary>
public record PaymentCreatedData : IWebhookData
{
    /// <summary>
    /// The payment identifier.
    /// </summary>
    [JsonConverter(typeof(GuidTypeConverter))]
    [JsonPropertyName("paymentId")]
    public Guid PaymentId { get; init; }

    /// <summary>
    /// The complete order associated with the payment.
    /// </summary>
    [Required]
    [JsonPropertyName("order")]
    public WebhookOrder Order { get; init; } = new();
}