using System;
using SolidNetsEasyClient.Helpers.Encryption.Encodings;
using SolidNetsEasyClient.Helpers.Invariants;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Helpers.Encryption.Flows;

/// <summary>
/// Procedures for when creating payment webhooks callback for the <see cref="EventName.PaymentCreated"/> event
/// </summary>
public static class PaymentCreatedFlow
{
    /// <summary>
    /// Construct a tuple of string authorization with a maximum size of 32, where any overflow will be inserted into the complement.
    /// </summary>
    /// <param name="hasher">The hashing function retriever</param>
    /// <param name="key">The private key</param>
    /// <param name="payment">The payment invariant</param>
    /// <returns>A tuple of authorization and the complement string</returns>
    public static (string Authorization, string? Complement) CreateAuthorization(IHasher hasher, byte[] key, PaymentCreatedInvariant payment)
    {
        var paymentBytes = ByteObjectConverter.Serialize(payment);
        using var hash = hasher.GetHashAlgorithm();
        hash.Key = key;
        var resultBytes = hash.ComputeHash(paymentBytes);

        var encode = CustomBase62Converter.Encode(resultBytes);

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

        return (Authorization: authorization.ToString(), Complement: complement?.ToString());
    }

    /// <summary>
    /// Validate actual response if they hash to same output
    /// </summary>
    /// <param name="hasher">The hasher</param>
    /// <param name="key">The private key</param>
    /// <param name="payment">The payment invariants</param>
    /// <param name="authorization">The authorization header value</param>
    /// <param name="complement">The authorization complement value</param>
    /// <returns>True if same hash otherwise false</returns>
    public static bool ValidatePaymentCreatedEventCallback(IHasher hasher, byte[] key, PaymentCreatedInvariant payment, string authorization, string? complement)
    {
        var (computedAuthorization, computedComplement) = CreateAuthorization(hasher, key, payment);
        var isValid = authorization == computedAuthorization && complement == computedComplement;
        return isValid;
    }
}
