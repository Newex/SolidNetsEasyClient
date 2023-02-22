using System;
using System.ComponentModel.DataAnnotations;
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
    public IHasher Hasher { get; set; } = new HmacSHA256Hasher();

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

    /// <summary>
    /// The authorization key that Nets inserts into the Authorization header on webhook callback.
    /// </summary>
    /// <remarks>
    /// Only alphanumeric characters are allowed with a maximum length of 32
    /// </remarks>
    [StringLength(32)]
    [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only alphanumeric characters are allowed")]
    public string AuthorizationKey { get; set; } = string.Empty;

    /// <summary>
    /// True if using a simple authorization with the given <see cref="AuthorizationKey"/>. If false a hash will be calculated and an endpoint url will be constructed so that the hash value can be validated using a-priori information.
    /// </summary>
    /// <remarks>
    /// A webhook bulk event notification will always use the <see cref="AuthorizationKey"/>
    /// </remarks>
    public bool UseSimpleAuthorization { get; set; } = true;

    /// <summary>
    /// The parameter name to indicate to the webhook that this request is a bulk event - thus we revert to use simple authorization.
    /// </summary>
    public string BulkIndicatorName { get; set; } = "bulk";

    internal const string WebhookEncryptionConfigurationSection = NetsEasyOptions.NetsEasyConfigurationSection + ":Webhook";
}
