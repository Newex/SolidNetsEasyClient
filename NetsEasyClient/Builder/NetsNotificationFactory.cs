using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.Builder;

/// <summary>
/// Notifications builder factory
/// </summary>
/// <remarks>
/// Instantiate a new <see cref="NetsNotificationFactory"/>
/// </remarks>
/// <param name="options">The nets easy options</param>
/// <param name="webhookOptions">The webhook encryption options</param>
/// <param name="linkGenerator">The link generator</param>
public class NetsNotificationFactory(
    IOptions<NetsEasyOptions> options,
    IOptions<WebhookEncryptionOptions> webhookOptions,
    LinkGenerator linkGenerator
    )
{
    private readonly string baseUrl = options.Value.BaseUrl;
    private readonly WebhookEncryptionOptions webhookOptions = webhookOptions.Value;
    private readonly LinkGenerator linkGenerator = linkGenerator;

    /// <summary>
    /// Create notification builder
    /// </summary>
    /// <returns>The notification builder</returns>
    public NetsNotificationBuilder CreateNotificationBuilder()
    {
        return new NetsNotificationBuilder(baseUrl, linkGenerator, webhookOptions);
    }
}
