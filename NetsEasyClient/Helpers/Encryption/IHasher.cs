using System.Security.Cryptography;

namespace SolidNetsEasyClient.Helpers.Encryption;

/// <summary>
/// Hashing interface for the webhook authorization header value calculation
/// </summary>
public interface IHasher
{
    /// <summary>
    /// Get a keyed hashing algorithm
    /// </summary>
    /// <returns>A keyed hashing algorithm</returns>
    KeyedHashAlgorithm GetHashAlgorithm();
}
