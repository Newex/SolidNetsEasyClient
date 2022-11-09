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
    Visa,

    /// <summary>
    /// MasterCard payment method
    /// </summary>
    MasterCard,

    /// <summary>
    /// Dankort payment method
    /// </summary>
    Dankort,

    /// <summary>
    /// AmericanExpress payment method
    /// </summary>
    AmericanExpress,

    /// <summary>
    /// PayPal payment method
    /// </summary>
    PayPal,

    /// <summary>
    /// Vipps payment method
    /// </summary>
    Vipps,

    /// <summary>
    /// MobilePay payment method
    /// </summary>
    MobilePay,

    /// <summary>
    /// Swish payment method
    /// </summary>
    Swish,

    /// <summary>
    /// Arvato payment method
    /// </summary>
    Arvato,

    /// <summary>
    /// EasyInvoice payment method
    /// </summary>
    EasyInvoice,

    /// <summary>
    /// EasyCampaign payment method
    /// </summary>
    EasyCampaign,

    /// <summary>
    /// RatePayInvoice payment method
    /// </summary>
    RatePayInvoice,

    /// <summary>
    /// RatePayInstallment payment method
    /// </summary>
    RatePayInstallment,

    /// <summary>
    /// RatePaySepa payment method
    /// </summary>
    RatePaySepa,

    /// <summary>
    /// Sofort payment method
    /// </summary>
    Sofort,

    /// <summary>
    /// Trustly payment method
    /// </summary>
    Trustly
}
