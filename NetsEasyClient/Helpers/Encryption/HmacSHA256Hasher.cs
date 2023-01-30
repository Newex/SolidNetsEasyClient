using System.Security.Cryptography;

namespace SolidNetsEasyClient.Helpers.Encryption;

/// <summary>
/// An HMAC-SHA256 hasher
/// </summary>
public class HmacSHA256Hasher : IHasher
{
    /// <inheritdoc />
    public KeyedHashAlgorithm GetHashAlgorithm()
    {
        return new HMACSHA256();
    }
}
