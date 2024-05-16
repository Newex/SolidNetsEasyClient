using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Requests.Customers.Addresses;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Payments;

/// <summary>
/// A charge
/// </summary>
public record Charge
{
    /// <summary>
    /// The amount to be charged
    /// </summary>
    [Required]
    [JsonPropertyName("amount")]
    public int Amount { get; init; }

    /// <summary>
    /// The order items list to charge for
    /// </summary>
    /// <remarks>
    /// Only required for partial charges
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("orderItems")]
    public IEnumerable<Item>? OrderItems { get; init; }

    /// <summary>
    /// Final shipping information
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("shipping")]
    public ChargeShipping? Shipping { get; init; }

    /// <summary>
    /// Flag to release remaining reservation.
    /// If set to true, the remaining authorization amount will be released.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("finalCharge")]
    public bool? FinalCharge { get; init; }

    /// <summary>
    /// Merchant payment reference
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("myReference")]
    public string? MyReference { get; init; }
}
