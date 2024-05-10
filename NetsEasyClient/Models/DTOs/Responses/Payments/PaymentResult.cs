using System;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Payments;

/// <summary>
/// A payment result object
/// </summary>
/// <remarks>
/// Created when a successfull payment has been made
/// </remarks>
public record PaymentResult
{
    /// <summary>
    /// The payment ID
    /// </summary>
    [JsonPropertyName("paymentId")]
    public Guid PaymentId { get; init; }

    /// <summary>
    /// The URL your website should redirect to if using a hosted pre-built checkout page
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("hostedPaymentPageUrl")]
    public string? HostedPaymentPageUrl { get; init; }
}
