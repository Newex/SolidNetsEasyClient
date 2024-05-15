using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Enums;

/// <summary>
/// Supported currencies for Nets Easy. see: https://developers.nets.eu/nets-easy/en-EU/api/#currency-and-amount
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<Currency>))]
public enum Currency
{
    /// <summary>
    /// Danish krone
    /// </summary>
    DKK = 0,

    /// <summary>
    /// Euro
    /// </summary>
    EUR = 1,

    /// <summary>
    /// Pound sterling
    /// </summary>
    GBP = 2,

    /// <summary>
    /// Norwegian krone
    /// </summary>
    NOK = 3,

    /// <summary>
    /// Swedish krona
    /// </summary>
    SEK = 4,

    /// <summary>
    /// United States dollar
    /// </summary>
    USD = 5,

    /// <summary>
    /// Złoty
    /// </summary>
    PLN = 6,

    /// <summary>
    /// Swiss franc
    /// </summary>
    CHF = 7
}
