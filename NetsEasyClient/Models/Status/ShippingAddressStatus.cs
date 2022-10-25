using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.Status;

/// <summary>
/// The shipping address information
/// </summary>
public record ShippingAddressStatus : ShippingAddress
{
    /// <summary>
    /// The name (or company name) of the customer
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("receiverLine")]
    public string? ReceiverLine { get; init; }
}
