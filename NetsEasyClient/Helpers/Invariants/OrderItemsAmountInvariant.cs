using System.Collections.Generic;
using SolidNetsEasyClient.Helpers.Encryption.Encodings;
using SolidNetsEasyClient.Models.DTOs;

namespace SolidNetsEasyClient.Helpers.Invariants;

/// <summary>
/// Represents an invariant with order items, amount and an optional nonce.
/// </summary>
public readonly record struct OrderItemsAmountInvariant : IInvariantSerializable
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
