using SolidNetsEasyClient.Constants;

namespace SolidNetsEasyClient.Models.Options;

/// <summary>
/// The platform payment options
/// </summary>
public record PlatformPaymentOptions
{
    /// <summary>
    /// The http client mode, which can be either in test mode or in live mode
    /// </summary>
    public ClientMode ClientMode { get; set; }

    /// <summary>
    /// The secret API key
    /// </summary>
    /// <remarks>
    /// Do not expose this key to your end users. Only use back channels to directly communicate with nets easy api using this key.
    /// </remarks>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// The checkout key
    /// </summary>
    /// <remarks>
    /// Use key from the front end. Can be exposed to the end user.
    /// </remarks>
    public string CheckoutKey { get; init; } = string.Empty;

    /// <summary>
    /// The checkout url
    /// </summary>
    public string CheckoutUrl { get; init; } = string.Empty;

    /// <summary>
    /// The terms and conditions url for your site
    /// </summary>
    public string TermsUrl { get; init; } = string.Empty;

    /// <summary>
    /// Where to return customer after completed payment when using a hosted checkout page
    /// </summary>
    public string? ReturnUrl { get; init; }

    /// <summary>
    /// The privacy policy url for your site
    /// </summary>
    public string PrivacyPolicyUrl { get; init; } = string.Empty;

    /// <summary>
    /// An identifier of the ecommerce platform
    /// </summary>
    public string? CommercePlatformTag { get; set; }

    /// <summary>
    /// The nets easy configuration section
    /// </summary>
    internal const string NetsEasyConfigurationSection = "SolidNetsEasy";
}
