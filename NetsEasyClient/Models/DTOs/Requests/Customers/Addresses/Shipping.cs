using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Customers.Addresses;

/// <summary>
/// Shipping details
/// </summary>
public record Shipping
{
    /// <summary>
    /// A list of three-letter country code (ISO3166)
    /// </summary>
    [JsonPropertyName("countries")]
    public IEnumerable<ShippingCountry> Countries { get; init; } = [];

    /// <summary>
    /// Determines if the merchant will handle the shipping cost
    /// </summary>
    /// <remarks>
    /// If set to true the payment order is required to be updated (using the
    /// Update order method) with shipping.costSpecified set to true before the
    /// customer can complete a purchase. Defaults to false if not specified.
    /// </remarks>
    [JsonPropertyName("merchantHandlesShippingCost")]
    public bool MerchantHandlesShippingCost { get; init; }

    /// <summary>
    /// Specify a separate shipping address
    /// </summary>
    /// <remarks>
    /// If set to true, the customer is provided an option to specify separate
    /// addresses for billing and shipping on the checkout page. If set to
    /// false, the billing address is used as the shipping address.
    /// </remarks>
    [JsonPropertyName("enableBillingAddress")]
    public bool EnableBillingAddress { get; init; }
}
