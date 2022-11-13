using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Enums;

/// <summary>
/// Supported currencies for Nets Easy. see: https://developers.nets.eu/nets-easy/en-EU/api/#currency-and-amount
/// </summary>
[JsonConverter(typeof(CurrencyStringEnumConverter))]
public enum Currency
{
    /// <summary>
    /// Danish krone
    /// </summary>
    DKK,

    /// <summary>
    /// Euro
    /// </summary>
    EUR,

    /// <summary>
    /// Pound sterling
    /// </summary>
    GBP,

    /// <summary>
    /// Norwegian krone
    /// </summary>
    NOK,

    /// <summary>
    /// Swedish krona
    /// </summary>
    SEK,

    /// <summary>
    /// United States dollar
    /// </summary>
    USD,

    /// <summary>
    /// Złoty
    /// </summary>
    PLN,

    /// <summary>
    /// Swiss franc
    /// </summary>
    CHF
}
