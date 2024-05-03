using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;

namespace SolidNetsEasyClient.Models.Options;

/// <summary>
/// The platform payment options
/// </summary>
public record NetsEasyOptions
{
    /// <summary>
    /// The http client mode, which can be either in test mode or in live mode
    /// </summary>
    [Required]
    public required ClientMode ClientMode { get; set; }

    /// <summary>
    /// The secret API key
    /// </summary>
    /// <remarks>
    /// Do not expose this key to your end users. Only use back channels to directly communicate with nets easy api using this key.
    /// </remarks>
    [Required]
    public required string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// The checkout key
    /// </summary>
    /// <remarks>
    /// Use key from the front end. Can be exposed to the end user.
    /// </remarks>
    public string CheckoutKey { get; set; } = string.Empty;

    /// <summary>
    /// The checkout url
    /// </summary>
    [Url]
    public string? CheckoutUrl { get; set; }

    /// <summary>
    /// The terms and conditions url for your site
    /// </summary>
    [Url]
    public string TermsUrl { get; set; } = string.Empty;

    /// <summary>
    /// Where to return customer after completed payment when using a hosted checkout page
    /// </summary>
    [Url]
    public string? ReturnUrl { get; set; }

    /// <summary>
    /// Where to return customer after cancelled payment when using a hosted checkout page
    /// </summary>
    [Url]
    public string? CancelUrl { get; set; } = string.Empty;

    /// <summary>
    /// The privacy policy url for your site
    /// </summary>
    [Url]
    public string PrivacyPolicyUrl { get; set; } = string.Empty;

    /// <summary>
    /// An identifier of the ecommerce platform
    /// </summary>
    public string? CommercePlatformTag { get; set; }

    /// <summary>
    /// A semi-colon ";" separated string of IPs for the Nets Easy webhook endpoints.
    /// </summary>
    /// <remarks>
    /// If empty these values will be used: 20.103.218.104/30 and 20.31.57.60/30 for the Live and Test mode respectively.
    /// Must be in CIDR format.
    /// </remarks>
    public string? NetsIPWebhookEndpoints { get; set; }

    /// <summary>
    /// Blacklist IPs requests sent to the webhook endpoint
    /// </summary>
    /// <remarks>
    /// Each IP should be separated by a semi-colon (;)
    /// </remarks>
    public string? BlacklistIPsForWebhook { get; set; }

    /// <summary>
    /// The integration type, to be used in the checkout page.
    /// </summary>
    /// <remarks>
    /// The default value is <see cref="Integration.EmbeddedCheckout"/>.
    /// </remarks>
    public Integration? IntegrationType { get; set; }

    /// <summary>
    /// The default customer type. Valid values are 'B2C' or 'B2B'. Default is 'B2C'.
    /// </summary>
    public ConsumerTypeEnum? DefaultCostumerType { get; set; }

    /// <summary>
    /// The supported costumer types to show on the checkout page.
    /// </summary>
    public IEnumerable<ConsumerTypeEnum>? SupportedTypes { get; set; }

    /// <summary>
    /// If true, you must handle customer data. Name, email etc. and regulatory
    /// compliance (GDPR etc.).
    /// This setting allows customers to NOT input further information on checkout.
    /// </summary>
    /// <remarks>
    /// Allows you to initiate the checkout with customer data so that your
    /// customer only need to provide payment details. It is possible to exclude
    /// all consumer and company information from the payment (only for certain
    /// payment methods) when it is set to true. If you still want to add
    /// consumer information to the payment you need to use the consumer object
    /// (either a privatePerson or a company, not both).
    /// </remarks>
    public bool? MerchantHandlesConsumerData { get; set; }

    /// <summary>
    /// The merchants (your) 3 letter country code.
    /// </summary>
    public string? CountryCode { get; set; }

    /// <summary>
    /// The list of countries the merchant ships to. If null, all countries supported by NETS will be accepted.
    /// The format is a 3 letter country code (ISO 3166-1).
    /// </summary>
    /// <remarks>
    /// <![CDATA[ Supported countries: https://developer.nexigroup.com/nexi-checkout/en-EU/api/#country-codes-and-phone-prefixes ]]>
    /// </remarks>
    public IEnumerable<string>? SupportedShippingCountries { get; set; }

    /// <summary>
    /// The merchant number. Use this only if you are a Nexi Group
    /// partner and initiating the checkout with your partner keys. If you are
    /// using the integration keys for your webshop, there is no need to specify
    /// this header. The maximum length is 128 characters.
    /// </summary>
    public string? NetsPartnerMerchantNumber { get; set; }

    /// <summary>
    /// The optional supported payment methods to be used in the checkout process.
    /// </summary>
    public IEnumerable<PaymentMethodConfiguration>? PaymentMethodsConfiguration { get; set; }

    /// <summary>
    /// The minimum allowed payment to check for in the payment builder
    /// </summary>
    public int MinimumAllowedPayment { get; set; } = 5_00;

    /// <summary>
    /// The authorization to use when NETS calls the webhook.
    /// </summary>
    public string? WebhookAuthorization { get; set; }

    /// <summary>
    /// The nets easy configuration section
    /// </summary>
    internal const string NetsEasyConfigurationSection = "SolidNetsEasy";
}
