using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Contacts;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Orders;

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
