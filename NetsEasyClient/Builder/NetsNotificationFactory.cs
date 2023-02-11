using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.Builder;

/// <summary>
/// Notifications builder factory
/// </summary>
public class NetsNotificationFactory
{
    private readonly string baseUrl;
    private readonly WebhookEncryptionOptions webhookOptions;
    private readonly LinkGenerator linkGenerator;

    /// <summary>
    /// Instantiate a new <see cref="NetsNotificationFactory"/>
    /// </summary>
    /// <param name="options">The nets easy options</param>
    /// <param name="webhookOptions">The webhook encryption options</param>
    /// <param name="linkGenerator">The link generator</param>
    public NetsNotificationFactory(
        IOptions<NetsEasyOptions> options,
        IOptions<WebhookEncryptionOptions> webhookOptions,
        LinkGenerator linkGenerator
    )
    {
        baseUrl = options.Value.BaseUrl;
        this.webhookOptions = webhookOptions.Value;
        this.linkGenerator = linkGenerator;
    }

    /// <summary>
    /// Create notification builder
    /// </summary>
    /// <returns>The notification builder</returns>
    public NetsNotificationBuilder CreateNotificationBuilder()
    {
        return new NetsNotificationBuilder(baseUrl, linkGenerator, webhookOptions);
    }
}
