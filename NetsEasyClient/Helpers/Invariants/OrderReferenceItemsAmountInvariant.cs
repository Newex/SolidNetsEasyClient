using System.Collections.Generic;
using SolidNetsEasyClient.Helpers.Encryption.Encodings;
using SolidNetsEasyClient.Models.DTOs;

namespace SolidNetsEasyClient.Helpers.Invariants;

/// <summary>
/// Invariant with order reference, order items and amount
/// </summary>
public readonly record struct OrderReferenceItemsAmountInvariant : IInvariantSerializable
{
    /// <summary>
    /// The order reference
    /// </summary>
    public required string OrderReference { get; init; }

    /// <summary>
    /// The order items
    /// </summary>
    public required IEnumerable<Item> OrderItems { get; init; }

    /// <summary>
    /// The order amount
    /// </summary>
    public required int Amount { get; init; }

    /// <summary>
    /// The nonce
    /// </summary>
    public string? Nonce { get; init; }

    /// <inheritdoc />
    public byte[] GetBytes()
    {
        return ByteObjectConverter.Serialize(this);
    }
}