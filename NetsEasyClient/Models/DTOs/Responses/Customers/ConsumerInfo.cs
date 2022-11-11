using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Orders;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Customers;

/// <summary>
/// The consumer information
/// </summary>
public record ConsumerInfo
{
    /// <summary>
    /// The consumer shipping address information
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("shippingAddress")]
    public ShippingAddressStatus? ShippingAddress { get; init; }

    /// <summary>
    /// The consumer company information
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("company")]
    public CompanyInfo? Company { get; init; }

    /// <summary>
    /// The natural private person information
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("privatePerson")]
    public PrivatePersonInfo? PrivatePerson { get; init; }

    /// <summary>
    /// The billing address information
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("billingAddress")]
    public ShippingAddressStatus? BillingAddress { get; init; }
}
