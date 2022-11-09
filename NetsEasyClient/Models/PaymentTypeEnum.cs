using System;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models;

/// <summary>
/// Payment types
/// </summary>
[JsonConverter(typeof(PaymentTypeEnumConverter))]
public enum PaymentTypeEnum
{
    /// <summary>
    /// Card payment type
    /// </summary>
    Card,

    /// <summary>
    /// Invoice payment type
    /// </summary>
    Invoice,

    /// <summary>
    /// Installment payment type
    /// </summary>
    Installment,

    /// <summary>
    /// Account-2-account payment type
    /// </summary>
    A2A,

    /// <summary>
    /// Wallet payment type
    /// </summary>
    Wallet,

    /// <summary>
    /// Prepaid invoice payment type
    /// </summary>
    PrepaidInvoice
}

/// <summary>
/// Helper for the <see cref="PaymentTypeEnum"/>
/// </summary>
public static class PaymentTypeHelper
{
    /// <summary>
    /// The prepaid invoice name
    /// </summary>
    public const string PrepaidInvoice = "PREPAID-INVOICE";

    /// <summary>
    /// Get the name of the payment type
    /// </summary>
    /// <param name="paymentType">The payment type</param>
    /// <returns>The name of the payment type</returns>
    public static string GetName(this PaymentTypeEnum paymentType)
    {
        return paymentType switch
        {
            PaymentTypeEnum.PrepaidInvoice => PrepaidInvoice,
            PaymentTypeEnum x => x.ToString(),
        };
    }

    /// <summary>
    /// Convert a string to a payment type
    /// </summary>
    /// <param name="paymentType">The string payment type</param>
    /// <returns>A payment enum type or null</returns>
    public static PaymentTypeEnum? Convert(string paymentType)
    {
        var hasEnum = Enum.TryParse<PaymentTypeEnum>(paymentType, ignoreCase: true, out var result);
        if (!hasEnum)
        {
            if (string.Equals(paymentType, PrepaidInvoice, StringComparison.OrdinalIgnoreCase))
            {
                return PaymentTypeEnum.PrepaidInvoice;
            }

            return null;
        }

        return result;
    }
}
