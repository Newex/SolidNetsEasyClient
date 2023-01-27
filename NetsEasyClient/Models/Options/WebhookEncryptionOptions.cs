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
}
