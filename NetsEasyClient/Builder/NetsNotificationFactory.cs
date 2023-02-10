using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using SolidNetsEasyClient.Helpers.Encryption;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.Builder;

/// <summary>
/// Notifications builder factory
/// </summary>
public class NetsNotificationFactory
{
    private readonly string baseUrl;
    private readonly LinkGenerator linkGenerator;
    private readonly string complementName;
    private readonly string nonceName;
    private readonly int nonceLength;
    private readonly IHasher hasher;
    private readonly byte[] key;

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
        this.linkGenerator = linkGenerator;
        complementName = webhookOptions.Value.ComplementName;
        nonceName = webhookOptions.Value.NonceName;
        nonceLength = webhookOptions.Value.NonceLength;
        hasher = webhookOptions.Value.Hasher;
        key = webhookOptions.Value.Key;
    }

    /// <summary>
    /// Create notification builder
    /// </summary>
    /// <returns>The notification builder</returns>
    public NetsNotificationBuilder CreateNotificationBuilder()
    {
        return new NetsNotificationBuilder(baseUrl, complementName, nonceName, linkGenerator, hasher, key, nonceLength);
    }
}
