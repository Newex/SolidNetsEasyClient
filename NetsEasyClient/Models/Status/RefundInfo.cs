using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.Status;

/// <summary>
/// A refund information
/// </summary>
public record RefundInfo
{
    /// <summary>
    /// A unique identifier of this refund
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("refundId")]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid? RefundId { get; init; }

    /// <summary>
    /// The total amount of the refund
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("amount")]
    public int? Amount { get; init; }

    /// <summary>
    /// The current state of the refund
    /// </summary>
    /// <remarks>
    /// Possible values are: 'Pending', 'Cancelled', 'Failed', 'Completed', 'Expired'
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("state")]
    public RefundStateEnum? State { get; init; }

    /// <summary>
    /// The date and time when the refund was last updated
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(NullableDateTimeOffsetConverter))]
    [JsonPropertyName("lastUpdated")]
    public DateTimeOffset? LastUpdated { get; init; }

    /// <summary>
    /// The list of returned and canceled order items that are associated with the refund. At least one order item is required
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("orderItems")]
    public IEnumerable<Item> OrderItems { get; init; } = Enumerable.Empty<Item>();
}
