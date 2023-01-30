using System.Collections.Generic;
using System.IO;
using SolidNetsEasyClient.Helpers.Invariants;
using SolidNetsEasyClient.Models.DTOs;

namespace SolidNetsEasyClient.Helpers.Encryption.Encodings;

/// <summary>
/// Convert object to bytes
/// </summary>
internal static class ByteObjectConverter
{
    internal static byte[] Serialize(OrderReferenceItemsAmountInvariant input)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new BinaryWriter(memoryStream);

        writer.Write(input.OrderReference);
        WriteItems(input.OrderItems, in writer);

        writer.Write(input.Amount);
        if (input.Nonce is not null)
        {
            writer.Write(input.Nonce);
        }

        var binary = memoryStream.ToArray();
        return binary;
    }

    internal static byte[] Serialize(AmountInvariant input)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new BinaryWriter(memoryStream);

        writer.Write(input.Amount);
        if (input.Nonce is not null)
        {
            writer.Write(input.Nonce);
        }

        var binary = memoryStream.ToArray();
        return binary;
    }

    internal static byte[] Serialize(OrderItemsAmountInvariant input)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new BinaryWriter(memoryStream);

        WriteItems(input.OrderItems, in writer);

        writer.Write(input.Amount);
        if (input.Nonce is not null)
        {
            writer.Write(input.Nonce);
        }

        var binary = memoryStream.ToArray();
        return binary;
    }

    private static void WriteItems(IEnumerable<Item> items, in BinaryWriter writer)
    {
        foreach (var item in items)
        {
            writer.Write(item.Reference);
            writer.Write(item.Name);
            writer.Write(item.Quantity);
            writer.Write(item.Unit);
            writer.Write(item.UnitPrice);
            if (item.TaxRate is not null)
            {
                writer.Write(item.TaxRate.GetValueOrDefault());
            }
        }
    }
}
