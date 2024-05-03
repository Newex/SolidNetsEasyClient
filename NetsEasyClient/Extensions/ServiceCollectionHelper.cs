using Microsoft.Extensions.DependencyInjection;
using SolidNetsEasyClient.Builder;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.Extensions;

/// <summary>
/// Extension methods for the <see cref="IServiceCollection"/> class
/// </summary>
public static class ServiceCollectionHelper
{
    /// <summary>
    /// Add nets easy client as an embedded checkout. The customer will only interact with your website.
    /// </summary>
    /// <remarks>
    /// Must also remember to set the secret API key, and the checkout key in the <see cref="NetsEasyOptions"/> options.
    /// </remarks>
    /// <param name="services">The services collection</param>
    /// <param name="checkoutUrl">The checkout url, including protocol and must match exactly to the checkout url page. The checkout url is the url that the customer sees on checkout.</param>
    /// <param name="termsUrl">The payment terms and conditions url for your webshop.</param>
    /// <param name="privacyPolicyUrl">The privacy policy url for your webshop.</param>
    /// <returns>A configuration builder</returns>
    public static NetsConfigurationBuilder AddNetsEasyEmbeddedCheckout(this IServiceCollection services, string checkoutUrl, string termsUrl, string privacyPolicyUrl)
    {
        return NetsConfigurationBuilder.Create(services).ConfigureNetsEasyOptions(options =>
        {
            options.CheckoutUrl = checkoutUrl;
            options.TermsUrl = termsUrl;
            options.PrivacyPolicyUrl = privacyPolicyUrl;
            options.IntegrationType = Integration.EmbeddedCheckout;
        });
    }

    /// <summary>
    /// Add nets easy client as a hosted checkout. The customer will be redirected to nets easy site.
    /// </summary>
    /// <param name="services">The services collection</param>
    /// <param name="returnUrl">The return url, where customer should return upon completing the checkout.</param>
    /// <param name="cancelUrl">The cancel url, where the customer should be redirected to upon cancelling the checkout.</param>
    /// <param name="termsUrl">The payment terms and conditions url for your webshop.</param>
    /// <param name="privacyPolicyUrl">The privacy policy url for your webshop.</param>
    /// <returns>A configuration builder</returns>
    public static NetsConfigurationBuilder AddNetsEasyHostedCheckout(this IServiceCollection services, string returnUrl, string cancelUrl, string termsUrl, string privacyPolicyUrl)
    {
        return NetsConfigurationBuilder.Create(services).ConfigureNetsEasyOptions(options =>
        {
            options.ReturnUrl = returnUrl;
            options.CancelUrl = cancelUrl;
            options.TermsUrl = termsUrl;
            options.PrivacyPolicyUrl = privacyPolicyUrl;
            options.IntegrationType = Integration.HostedPaymentPage;
        });
    }
}
