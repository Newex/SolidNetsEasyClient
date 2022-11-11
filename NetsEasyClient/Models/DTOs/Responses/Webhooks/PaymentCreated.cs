using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// The <see cref="EventName.PaymentCreated"/> event is triggered when a new payment is created. This happens when the customer hits the "Pay" button on the checkout page.
/// </summary>
public record PaymentCreated : Webhook<CreatedData> { }

/// <summary>
/// The data associated with this event.
/// </summary>
public record CreatedData
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