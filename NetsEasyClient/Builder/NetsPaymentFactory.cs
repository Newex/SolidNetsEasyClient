using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.Builder;

/// <summary>
/// The payment builder factory
/// </summary>
/// <remarks>
/// Instantiate a new <see cref="NetsPaymentFactory"/>
/// </remarks>
/// <param name="netsOptions">The Nets Easy options</param>
/// <param name="webhookOptions">The webhook options</param>
/// <param name="linkGenerator">The link generator</param>
public sealed class NetsPaymentFactory(
    IOptions<NetsEasyOptions> netsOptions,
    IOptions<WebhookEncryptionOptions> webhookOptions,
    LinkGenerator linkGenerator
    )
{
    private readonly NetsEasyOptions netsOptions = netsOptions.Value;
    private readonly WebhookEncryptionOptions webhookOptions = webhookOptions.Value;
    private readonly LinkGenerator linkGenerator = linkGenerator;

    /// <summary>
    /// Create a new payment builder
    /// </summary>
    /// <param name="order">The order</param>
    /// <returns>A payment builder</returns>
    public NetsPaymentBuilder CreatePaymentBuilder(Order order)
    {
        return new NetsPaymentBuilder(
            netsOptions.BaseUrl,
            webhookOptions.ComplementName,
            webhookOptions.NonceName,
            webhookOptions.Hasher,
            webhookOptions.Key,
            webhookOptions.NonceLength,
            order,
            netsOptions.MinimumAllowedPayment,
            webhookOptions.UseSimpleAuthorization,
            webhookOptions.AuthorizationKey,
            linkGenerator);
    }
}
