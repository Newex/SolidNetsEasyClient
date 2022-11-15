using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Payments;

/// <summary>
/// Subscription details
/// </summary>
public record SubscriptionDetails
{
    /// <summary>
    /// The subscription identifier.
    /// </summary>
    [Required]
    [JsonPropertyName("subscriptionId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid SubscriptionId { get; init; }

    /// <summary>
    /// The frequency
    /// </summary>
    [JsonPropertyName("frequency")]
    public int Frequency { get; init; }

    /// <summary>
    /// Defines the minimum number of days between each recurring charge. This interval commences from either the day the subscription was created or the most recent subscription charge, whichever is later. An interval value of 0 means that there are no payment interval restrictions.
    /// </summary>
    [Required]
    [JsonPropertyName("interval")]
    public int Interval { get; init; }

    /// <summary>
    /// Refers to the date and time the subscription will expire. The field has three components: date, time, and time zone (offset from GMT), for example: 2021-07-02T12:00:00.0000+02:00.
    /// </summary>
    [Required]
    [JsonPropertyName("endDate")]
    public DateTimeOffset EndDate { get; init; }

    /// <summary>
    /// The payment details
    /// </summary>
    [Required]
    [JsonPropertyName("paymentDetails")]
    public PaymentDetails PaymentDetails { get; init; } = new();

    /// <summary>
    /// Represents an error that occurred during the import of a subscription from an external ecommerce system.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ImportError? ImportError { get; init; }
}

/// <summary>
/// The payment details
/// </summary>
public record PaymentDetails
{
    /// <summary>
    /// The type of payment. Possible values are: 'CARD', 'INVOICE', 'A2A', 'INSTALLMENT', 'WALLET', and 'PREPAID-INVOICE'.
    /// </summary>
    [Required]
    [JsonPropertyName("paymentType")]
    public PaymentTypeEnum PaymentType { get; init; }

    /// <summary>
    /// The payment method. For example Visa or Mastercard.
    /// </summary>
    [Required]
    [JsonPropertyName("paymentMethod")]
    public PaymentMethodEnum PaymentMethod { get; init; }

    /// <summary>
    /// The card details
    /// </summary>
    [Required]
    public CardDetailsInfo CardDetails { get; init; } = new();
}

/// <summary>
/// Represents an error that occurred during the import of a subscription from an external ecommerce system.
/// </summary>
public record ImportError
{
    /// <summary>
    /// The error code.
    /// </summary>
    [JsonPropertyName("importStepsResponseCode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ImportStepsResponseCode { get; init; }

    /// <summary>
    /// The source of the error.
    /// </summary>
    [JsonPropertyName("importStepsResponseSource")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ImportStepsResponseSource { get; init; }

    /// <summary>
    /// The error message.
    /// </summary>
    [JsonPropertyName("importStepsResponseText")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ImportStepsResponseText { get; init; }
}