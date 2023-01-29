using Microsoft.Extensions.Options;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.Builder;

public sealed class NetsPaymentFactory
{
    private readonly PlatformPaymentOptions netsOptions;
    private readonly WebhookEncryptionOptions webhookOptions;

    public NetsPaymentFactory(
        IOptions<PlatformPaymentOptions> netsOptions,
        IOptions<WebhookEncryptionOptions> webhookOptions
    )
    {
        this.netsOptions = netsOptions.Value;
        this.webhookOptions = webhookOptions.Value;
    }

    public NetsPaymentBuilder CreatePaymentBuilder(Order order)
    {
        return new NetsPaymentBuilder(netsOptions.BaseUrl, webhookOptions.ComplementName, webhookOptions.NonceName, webhookOptions.Hasher, webhookOptions.Key, webhookOptions.NonceLength, order, netsOptions.MinimumAllowedPayment);
    }
}
