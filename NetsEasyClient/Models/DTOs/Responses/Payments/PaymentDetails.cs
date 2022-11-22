using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Payments;

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
    [JsonPropertyName("cardDetails")]
    public CardDetailsInfo CardDetails { get; init; } = new();
}
