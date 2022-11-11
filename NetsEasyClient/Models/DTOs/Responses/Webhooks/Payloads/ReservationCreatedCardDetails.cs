using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

/// <summary>
/// Reservation card details
/// </summary>
public record ReservationCreatedCardDetails
{
    /// <summary>
    /// The credit or debit indicator
    /// </summary>
    [JsonPropertyName("creditDebitIndicator")]
    public string CreditDebitIndicator { get; init; } = string.Empty;

    /// <summary>
    /// The expiry month
    /// </summary>
    [JsonPropertyName("expiryMonth")]
    public int ExpiryMonth { get; init; }

    /// <summary>
    /// The expiry year
    /// </summary>
    [JsonPropertyName("expiryYear")]
    public int ExpiryYear { get; init; }

    /// <summary>
    /// The issuer country
    /// </summary>
    [JsonPropertyName("issuerCountry")]
    public string IssuerCountry { get; init; } = string.Empty;

    /// <summary>
    /// The truncated pan
    /// </summary>
    [JsonPropertyName("truncatedPan")]
    public string TruncatedPan { get; init; } = string.Empty;

    /// <summary>
    /// The 3D secure
    /// </summary>
    [JsonPropertyName("threeDSecure")]
    public ReservationCreatedThreeDSecure ThreeDSecure { get; init; } = new();
}
