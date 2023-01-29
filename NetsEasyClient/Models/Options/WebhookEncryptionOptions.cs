using System;
using SolidNetsEasyClient.Helpers.Encryption;

namespace SolidNetsEasyClient.Models.Options;

/// <summary>
/// Webhook encryption options
/// </summary>
public record WebhookEncryptionOptions
{
    /// <summary>
    /// The hasher
    /// </summary>
    public IHasher Hasher { get; set; } = default!;

    /// <summary>
    /// The encryption key
    /// </summary>
    public byte[] Key { get; set; } = Array.Empty<byte>();

    /// <summary>
    /// The nonce length
    /// </summary>
    /// <remarks>
    /// The nonce is sent to Nets by URL callback and must be a reasonable size since Nets only allows 256 characters for the whole webhook url length
    /// Maximum allowed length is therefore 256 and this would leave no room for the url.
    /// </remarks>
    public int NonceLength { get; set; } = 10;

    /// <summary>
    /// The default complement name used in webhook
    /// </summary>
    public string ComplementName { get; set; } = "complement";

    /// <summary>
    /// The default nonce name used in webhook
    /// </summary>
    public string NonceName { get; set; } = "nonce";
}
