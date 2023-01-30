using System.Collections.Generic;
using SolidNetsEasyClient.Helpers.Encryption.Encodings;
using SolidNetsEasyClient.Models.DTOs;

namespace SolidNetsEasyClient.Helpers.Invariants;

/// <summary>
/// Represents a reservation failed event invariant for: before and after sending to Nets Easy.
/// </summary>
public readonly record struct ReservationFailedInvariant : IInvariantSerializable
{
    /// <summary>
    /// The order items
    /// </summary>
    public required IEnumerable<Item> OrderItems { get; init; }

    /// <summary>
    /// The order amount
    /// </summary>
    public required int Amount { get; init; }

    /// <summary>
    /// The optional nonce
    /// </summary>
    public string? Nonce { get; init; }

    /// <inheritdoc />
    public byte[] GetBytes()
    {
        return ByteObjectConverter.Serialize(this);
    }
}
