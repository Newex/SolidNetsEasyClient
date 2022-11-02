using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models;

/// <summary>
/// Defines the behavior and style of the checkout page
/// </summary>
public record Checkout
{
    /// <summary>
    /// Specifies where the checkout will be loaded if using an embedded checkout page
    /// </summary>
    /// <seealso cref="IntegrationType"/>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("url")]
    public string? Url { get; init; }

    /// <summary>
    /// Determines whether the checkout should be embedded in your webshop or if the checkout should be hosted by Nets on a separate page
    /// </summary>
    /// <remarks>
    /// Valid values are: 'EmbeddedCheckout' (default) or 'HostedPaymentPage'. Please note that the string values are case sensitive.If set to 'HostedPaymentPage', your website should redirect the customer to the hostedPaymentPageUrl provided in the response body. Using a hosted checkout page requires that you specify the returnUrl property.If set to 'EmbeddedCheckout', the checkout page will be embedded within an iframe on your website using the Checkout JS SDK. Using an embedded checkout page requires that you also specify the url property
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("integrationType")]
    public Integration? IntegrationType { get; init; }

    /// <summary>
    /// Specifies where your customer will return after a completed payment when using a hosted checkout page
    /// </summary>
    /// <seealso cref="IntegrationType"/>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("returnUrl")]
    public string? ReturnUrl { get; init; }

    /// <summary>
    /// Specifies where your customer will return after a canceled payment when using a hosted checkout page
    /// </summary>
    /// <seealso cref="IntegrationType"/>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("cancelUrl")]
    public string? CancelUrl { get; init; }

    /// <summary>
    /// Contains information about the customer. If provided, this information will be used for initating the consumer data of the payment object
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("consumer")]
    public Consumer? Consumer { get; init; }

    /// <summary>
    /// The URL to the terms and conditions of your webshop
    /// </summary>
    [Required]
    [JsonPropertyName("termsUrl")]
    public string TermsUrl { get; init; } = string.Empty;

    /// <summary>
    /// The URL to the privacy and cookie settings of your webshop
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("merchantTermsUrl")]
    public string? MerchantTermsUrl { get; init; }

    /// <summary>
    /// An array of countries that limits the set of countries available for shipping. If left unspecified, all countries supported by Easy Checkout will be available for shipping on the checkout page
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("shippingCountries")]
    public IEnumerable<ShippingCountry>? ShippingCountries { get; init; }

    /// <summary>
    /// Shipping details
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("shipping")]
    public Shipping? Shipping { get; init; }

    /// <summary>
    /// Configures which consumer types should be accepted
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("consumerType")]
    public ConsumerType? ConsumerType { get; init; }

    /// <summary>
    /// If set to true, the transaction will be charged automatically after the reservation has been accepted. Default value is false if not specified
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("charge")]
    public bool? Charge { get; init; }

    /// <summary>
    /// If set to true, the checkout will not load any user data, and also the checkout will not remember the current consumer on this device. Default value is false if not specified
    /// </summary>
    [JsonPropertyName("publicDevice")]
    public bool PublicDevice { get; init; }

    /// <summary>
    /// Allows you to initiate the checkout with customer data so that your customer only need to provide payment details. If set to true, you also need to specify a consumer object (either a privatePerson or a company, not both)
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("merchantHandlesConsumerData")]
    public bool? MerchantHandlesConsumerData { get; init; }

    /// <summary>
    /// Defines the appearance of the checkout page
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("appearance")]
    public CheckoutAppearance? Appearance { get; init; }

    /// <summary>
    /// Merchant's three-letter checkout country code (ISO 3166-1), for example GBR
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; init; }
}
