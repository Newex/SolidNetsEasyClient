using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Payments;

/// <summary>
/// A charge item
/// </summary>
public record ChargeStatus
{
    /// <summary>
    /// A unique identifier of the charge
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("chargeId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid? ChargeId { get; init; }

    /// <summary>
    /// The total amount of this charge
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("amount")]
    public int? Amount { get; init; }

    /// <summary>
    /// The date and time when the charge was initiated
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("created")]
    public DateTimeOffset? Created { get; init; }

    /// <summary>
    /// The array of order items associated with the charge
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("orderItems")]
    public IEnumerable<Item> OrderItems { get; init; } = Enumerable.Empty<Item>();
}
