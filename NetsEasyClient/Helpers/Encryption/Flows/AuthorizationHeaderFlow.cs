using System;
using SolidNetsEasyClient.Helpers.Encryption.Encodings;
using SolidNetsEasyClient.Helpers.Invariants;

namespace SolidNetsEasyClient.Helpers.Encryption.Flows;

/// <summary>
/// Authorization header flow
/// </summary>
public static class AuthorizationHeaderFlow
{
    /// <summary>
    /// Create authorization header values
    /// </summary>
    /// <param name="hasher">The hasher</param>
    /// <param name="key">The key</param>
    /// <param name="invariant">The invariant</param>
    /// <returns>An authorization header model</returns>
    public static AuthorizationHeaderModel CreateAuthorization(IHasher hasher, byte[] key, IInvariantSerializable invariant)
    {
        var input = invariant.GetBytes();
        using var hash = hasher.GetHashAlgorithm();
        hash.Key = key;
        var output = hash.ComputeHash(input);
        var encode = CustomBase62Converter.Encode(output);

        ReadOnlySpan<char> authorization;
        string? complement;
        if (encode.Length > 32)
        {
            authorization = encode.AsSpan()[..32];
            complement = encode.AsSpan()[32..].ToString();
        }
        else
        {
            authorization = encode;
            complement = null;
        }

        return new AuthorizationHeaderModel
        {
            Authorization = authorization.ToString(),
            Complement = complement
        };
    }

    /// <summary>
    /// Validate an authorization header model
    /// </summary>
    /// <param name="hasher">The hasher</param>
    /// <param name="key">The key</param>
    /// <param name="invariant">The invariant</param>
    /// <param name="authorization">The authorization header value</param>
    /// <param name="complement">The option authorization complement</param>
    /// <returns>True if valid authorization header and complement otherwise false</returns>
    public static bool ValidateAuthorization(IHasher hasher, byte[] key, IInvariantSerializable invariant, string authorization, string? complement)
    {
        var expected = CreateAuthorization(hasher, key, invariant);
        return expected.Authorization == authorization && expected.Complement == complement;
    }
}
