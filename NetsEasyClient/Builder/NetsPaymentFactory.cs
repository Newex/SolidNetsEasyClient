using Microsoft.Extensions.Options;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.Builder;

/// <summary>
/// The payment builder factory
/// </summary>
public sealed class NetsPaymentFactory
{
    private readonly NetsEasyOptions netsOptions;
    private readonly WebhookEncryptionOptions webhookOptions;

    /// <summary>
    /// Instantiate a new <see cref="NetsPaymentFactory"/>
    /// </summary>
    /// <param name="netsOptions">The Nets Easy options</param>
    /// <param name="webhookOptions">The webhook options</param>
    public NetsPaymentFactory(
        IOptions<NetsEasyOptions> netsOptions,
        IOptions<WebhookEncryptionOptions> webhookOptions
    )
    {
        this.netsOptions = netsOptions.Value;
        this.webhookOptions = webhookOptions.Value;
    }

    /// <summary>
    /// Create a new payment builder
    /// </summary>
    /// <param name="order">The order</param>
    /// <returns>A payment builder</returns>
    public NetsPaymentBuilder CreatePaymentBuilder(Order order)
    {
        return new NetsPaymentBuilder(netsOptions.BaseUrl, webhookOptions.ComplementName, webhookOptions.NonceName, webhookOptions.Hasher, webhookOptions.Key, webhookOptions.NonceLength, order, netsOptions.MinimumAllowedPayment);
    }
}
