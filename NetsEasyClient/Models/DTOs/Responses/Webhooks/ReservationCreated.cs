using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// A web hook payment reservation created event DTO
/// </summary>
public record ReservationCreated : Webhook<ReservationCreatedData>
{
    /// <summary>
    /// The merchant Id
    /// </summary>
    [JsonPropertyName("merchantNumber")]
    public int MerchantNumber
    {
        get { return MerchantId; }
        init { MerchantId = value; }
    }
}

/// <summary>
/// The reservation payload
/// </summary>
public record ReservationCreatedData
{
    /// <summary>
    /// The payment identifier
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

    /// <summary>
    /// The payment method
    /// </summary>
    [JsonPropertyName("paymentMethod")]
    public PaymentMethodEnum PaymentMethod { get; init; }

    /// <summary>
    /// The payment type
    /// </summary>
    [JsonPropertyName("paymentType")]
    public PaymentTypeEnum PaymentType { get; init; }

    /// <summary>
    /// The reservation amount
    /// </summary>
    [JsonPropertyName("amount")]
    public WebhookAmount Amount { get; init; } = new();

    /*
        Payment V1
    */

    /// <summary>
    /// The reservation card details
    /// </summary>
    [JsonPropertyName("cardDetails")]
    public ReservationCreatedCardDetails? V1CardDetails { get; init; }

    /// <summary>
    /// The consumer details
    /// </summary>
    [JsonPropertyName("consumer")]
    public ReservationCreatedConsumer? V1Consumer { get; init; }

    /// <summary>
    /// The reservation reference
    /// </summary>
    [JsonPropertyName("reservationReference")]
    public string? V1ReservationReference { get; init; }

    /// <summary>
    /// The reserve id
    /// </summary>
    [JsonPropertyName("reserveId")]
    public Guid? V1ReserveId { get; init; }
}

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
    public ThreeDSecure ThreeDSecure { get; init; } = new();
}

/// <summary>
/// The 3D secure
/// </summary>
public record ThreeDSecure
{
    /// <summary>
    /// The authentication enrollment status
    /// </summary>
    [JsonPropertyName("authenticationEnrollmentStatus")]
    public string AuthenticationEnrollmentStatus { get; init; } = string.Empty;

    /// <summary>
    /// The authentication status
    /// </summary>
    [JsonPropertyName("authenticationStatus")]
    public string AuthenticationStatus { get; init; } = string.Empty;

    /// <summary>
    /// The eci
    /// </summary>
    [JsonPropertyName("eci")]
    public string ECI { get; init; } = string.Empty;
}

/// <summary>
/// The consumer reservation details
/// </summary>
public record ReservationCreatedConsumer
{
    /// <summary>
    /// The consumer IP
    /// </summary>
    [JsonPropertyName("ip")]
    public string IP { get; init; } = string.Empty;
}