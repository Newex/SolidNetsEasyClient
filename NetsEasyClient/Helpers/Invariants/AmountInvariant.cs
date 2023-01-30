using SolidNetsEasyClient.Helpers.Encryption.Encodings;

namespace SolidNetsEasyClient.Helpers.Invariants;

/// <summary>
/// Represents a reservation created v1 event invariant for: before and after sending to Nets Easy.
/// </summary>
public readonly record struct AmountInvariant : IInvariantSerializable
{
    /// <summary>
    /// The order amount
    /// </summary>
    public required int Amount { get; init; }

    /// <summary>
    /// The optional nonce value
    /// </summary>
    public string? Nonce { get; init; }

    /// <inheritdoc />
    public byte[] GetBytes()
    {
        return ByteObjectConverter.Serialize(this);
    }
}
