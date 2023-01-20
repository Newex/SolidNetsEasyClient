using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Enums;

/// <summary>
/// Payment methods
/// </summary>
[JsonConverter(typeof(PaymentMethodEnumConverter))]
public enum PaymentMethodEnum
{
    /// <summary>
    /// Visa payment method
    /// </summary>
    Visa = 0,

    /// <summary>
    /// MasterCard payment method
    /// </summary>
    MasterCard = 1,

    /// <summary>
    /// Dankort payment method
    /// </summary>
    Dankort = 2,

    /// <summary>
    /// AmericanExpress payment method
    /// </summary>
    AmericanExpress = 3,

    /// <summary>
    /// PayPal payment method
    /// </summary>
    PayPal = 4,

    /// <summary>
    /// Vipps payment method
    /// </summary>
    Vipps = 5,

    /// <summary>
    /// MobilePay payment method
    /// </summary>
    MobilePay = 6,

    /// <summary>
    /// Swish payment method
    /// </summary>
    Swish = 7,

    /// <summary>
    /// Arvato payment method
    /// </summary>
    Arvato = 8,

    /// <summary>
    /// EasyInvoice payment method
    /// </summary>
    EasyInvoice = 9,

    /// <summary>
    /// EasyCampaign payment method
    /// </summary>
    EasyCampaign = 10,

    /// <summary>
    /// RatePayInvoice payment method
    /// </summary>
    RatePayInvoice = 11,

    /// <summary>
    /// RatePayInstallment payment method
    /// </summary>
    RatePayInstallment = 12,

    /// <summary>
    /// RatePaySepa payment method
    /// </summary>
    RatePaySepa = 13,

    /// <summary>
    /// Sofort payment method
    /// </summary>
    Sofort = 14,

    /// <summary>
    /// Trustly payment method
    /// </summary>
    Trustly = 15
}
