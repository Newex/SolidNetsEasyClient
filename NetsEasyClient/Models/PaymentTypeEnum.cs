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
