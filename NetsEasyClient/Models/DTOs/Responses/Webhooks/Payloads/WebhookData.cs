using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

/// <summary>
/// A webhook data payload
/// </summary>
[JsonDerivedType(typeof(PaymentCreatedData))]
public record WebhookData
{
    /// <summary>
    /// The payment identifier
    /// </summary>
    [Required]
    [JsonPropertyName("paymentId")]
    public Guid PaymentId { get; init; }
}