using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Payments;

/// <summary>
/// A payment method
/// </summary>
public record PaymentMethod
{
    /// <summary>
    /// The name of the payment method
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("name")]
    public PaymentTypeMethodName? Name { get; } = PaymentMethodEnum.EasyInvoice;

    /// <summary>
    /// Represents a line of a customer order. An order item refers to a product
    /// that the customer has bought. A product can be anything from a physical
    /// product to an online subscription or shipping
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("fee")]
    public Item? Fee { get; init; }
}
