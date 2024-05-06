using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// The <see cref="EventName.PaymentCreated"/> event is triggered when a new payment is created. This happens when the customer hits the "Pay" button on the checkout page.
/// </summary>
public record PaymentCreated : Webhook<PaymentCreatedData>
{
    /// <summary>
    /// The data associated with this event
    /// </summary>
    [Required]
    [JsonPropertyName("data")]
    public override PaymentCreatedData Data { get; init; } = new();
}
