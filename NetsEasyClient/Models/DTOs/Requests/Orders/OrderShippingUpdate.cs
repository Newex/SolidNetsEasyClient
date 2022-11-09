using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Orders;

/// <summary>
/// Order shipping update
/// </summary>
public record OrderShippingUpdate
{
    /// <summary>
    /// Determine if cost is specified?!
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("costSpecified")]
    public bool? CostSpecified { get; init; }
}
