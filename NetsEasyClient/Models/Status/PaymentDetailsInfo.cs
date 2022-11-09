using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.Status;

/// <summary>
/// Payment details information
/// </summary>
public record PaymentDetailsInfo
{
    /// <summary>
    /// The type of payment
    /// </summary>
    /// <remarks>
    /// Possible values are: 'CARD', 'INVOICE', 'A2A', 'INSTALLMENT', 'WALLET', and 'PREPAID-INVOICE'
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("paymentType")]
    public PaymentTypeEnum? PaymentType { get; init; }

    /// <summary>
    /// The payment method, for example Visa or Mastercard
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("paymentMethod")]
    public PaymentMethodEnum? PaymentMethod { get; init; }

    /// <summary>
    /// The invoice details information
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("invoiceDetails")]
    public InvoiceDetailsInfo? InvoiceDetails { get; init; }

    /// <summary>
    /// The card details information
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("cardDetails")]
    public CardDetailsInfo? CardDetails { get; init; }
}
