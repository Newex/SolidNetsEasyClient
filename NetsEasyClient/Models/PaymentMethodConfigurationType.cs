using System;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models;

/// <summary>
/// Payment instance
/// </summary>
public enum PaymentInstance
{
    /// <summary>
    /// Specific provider
    /// </summary>
    Method,

    /// <summary>
    /// Generic payment
    /// </summary>
    Type
}

/// <summary>
/// A list of payment method types
/// </summary>
[JsonConverter(typeof(PaymentMethodTypeConverter))]
public record struct PaymentMethodConfigurationType
{
    private readonly string method;

    internal PaymentMethodConfigurationType(string method, PaymentInstance paymentInstance)
    {
        this.method = method;
        PaymentInstance = paymentInstance;
    }

    /// <summary>
    /// Determine if this is a payment method or type
    /// </summary>
    public PaymentInstance PaymentInstance { get; }

    /// <summary>
    /// Use <see cref="PaymentMethodConfigurationType"/> where a string is expected
    /// </summary>
    /// <param name="input">The payment method</param>
    public static implicit operator PaymentMethodConfigurationType(ValueTuple<string, bool> input) => new(input.Item1, input.Item2 ? PaymentInstance.Method : PaymentInstance.Type);

    /// <summary>
    /// Implicit conversion from string
    /// </summary>
    /// <remarks>
    /// Returns a payment <see cref="Methods"/>
    /// </remarks>
    /// <param name="input">The input string</param>
    public static implicit operator PaymentMethodConfigurationType(string input) => (input, true);

    /// <summary>
    /// Get the payment method
    /// </summary>
    /// <returns>The payment method</returns>
    public override string ToString() => method;

    /// <summary>
    /// Payment method types
    /// </summary>
    public static class Methods
    {
        /// <summary>
        /// Visa payment method
        /// </summary>
        public static PaymentMethodConfigurationType Visa => ("Visa", true);

        /// <summary>
        /// MasterCard payment method
        /// </summary>
        public static PaymentMethodConfigurationType MasterCard => ("MasterCard", true);

        /// <summary>
        /// Dankort payment method
        /// </summary>
        public static PaymentMethodConfigurationType Dankort => ("Dankort", true);

        /// <summary>
        /// AmericanExpress payment method
        /// </summary>
        public static PaymentMethodConfigurationType AmericanExpress => ("AmericanExpress", true);

        /// <summary>
        /// PayPal payment method
        /// </summary>
        public static PaymentMethodConfigurationType PayPal => ("PayPal", true);

        /// <summary>
        /// Vipps payment method
        /// </summary>
        public static PaymentMethodConfigurationType Vipps => ("Vipps", true);

        /// <summary>
        /// MobilePay payment method
        /// </summary>
        public static PaymentMethodConfigurationType MobilePay => ("MobilePay", true);

        /// <summary>
        /// Swish payment method
        /// </summary>
        public static PaymentMethodConfigurationType Swish => ("Swish", true);

        /// <summary>
        /// Arvato payment method
        /// </summary>
        public static PaymentMethodConfigurationType Arvato => ("Arvato", true);

        /// <summary>
        /// EasyInvoice payment method
        /// </summary>
        public static PaymentMethodConfigurationType EasyInvoice => ("EasyInvoice", true);

        /// <summary>
        /// EasyCampaign payment method
        /// </summary>
        public static PaymentMethodConfigurationType EasyCampaign => ("EasyCampaign", true);

        /// <summary>
        /// RatePayInvoice payment method
        /// </summary>
        public static PaymentMethodConfigurationType RatePayInvoice => ("RatePayInvoice", true);

        /// <summary>
        /// RatePayInstallment payment method
        /// </summary>
        public static PaymentMethodConfigurationType RatePayInstallment => ("RatePayInstallment", true);

        /// <summary>
        /// RatePaySepa payment method
        /// </summary>
        public static PaymentMethodConfigurationType RatePaySepa => ("RatePaySepa", true);

        /// <summary>
        /// Sofort payment method
        /// </summary>
        public static PaymentMethodConfigurationType Sofort => ("Sofort", true);

        /// <summary>
        /// Trustly payment method
        /// </summary>
        public static PaymentMethodConfigurationType Trustly => ("Trustly", true);
    }

    /// <summary>
    /// Payment types
    /// </summary>
    public static class Types
    {
        /// <summary>
        /// Card payment type
        /// </summary>
        public static PaymentMethodConfigurationType Card => ("Card", false);

        /// <summary>
        /// Invoice payment type
        /// </summary>
        public static PaymentMethodConfigurationType Invoice => ("Invoice", false);

        /// <summary>
        /// Installment payment type
        /// </summary>
        public static PaymentMethodConfigurationType Installment => ("Installment", false);

        /// <summary>
        /// Account-2-account payment type
        /// </summary>
        public static PaymentMethodConfigurationType A2A => ("A2A", false);

        /// <summary>
        /// Wallet payment type
        /// </summary>
        public static PaymentMethodConfigurationType Wallet => ("Wallet", false);

        /// <summary>
        /// Wallet payment type
        /// </summary>
        public static PaymentMethodConfigurationType PrepaidInvoice => ("Prepaid-invoice", false);
    }
}
