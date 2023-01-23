using System.IO;
using SolidNetsEasyClient.Helpers.Invariants;

namespace SolidNetsEasyClient.Helpers.Encryption.Encodings;

/// <summary>
/// Convert object to bytes
/// </summary>
public static class ByteObjectConverter
{
    /// <summary>
    /// Serialize <see cref="PaymentCreatedInvariant"/> to a byte array
    /// </summary>
    /// <param name="input">The payment input</param>
    /// <returns>A byte array</returns>
    public static byte[] Serialize(PaymentCreatedInvariant input)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new BinaryWriter(memoryStream);

        writer.Write(input.OrderReference);
        foreach (var item in input.OrderItems)
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

        writer.Write(input.Amount);
        if (input.Nonce is not null)
        {
            writer.Write(input.Nonce);
        }

        var binary = memoryStream.ToArray();
        return binary;
    }
}
