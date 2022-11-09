using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

namespace SolidNetsEasyClient.Encryption;

/// <summary>
/// Calculate a simple HMAC for a given order and validate it
/// </summary>
public static class PaymentRequestHmac
{
    /// <summary>
    /// Sign an <see cref="Order"/> which contains a reference with HMAC-SHA1
    /// </summary>
    /// <param name="order">The order</param>
    /// <param name="key">The signing key</param>
    /// <returns>A base64 encoded signature</returns>
    /// <exception cref="ArgumentException">Thrown if order reference is null</exception>
    public static string SignOrder(this Order order, string key)
    {
        if (order.Reference is null)
        {
            throw new ArgumentException("Order must have a reference");
        }

        var orderId = Encoding.UTF8.GetBytes(order.Reference);
        var byteKey = Encoding.UTF8.GetBytes(key);
        using var hmac = new HMACSHA1(byteKey);
        using var stream = new MemoryStream(orderId);
        var result = hmac.ComputeHash(stream);
        return Convert.ToBase64String(result);
    }

    /// <summary>
    /// Validate the created payment with a given key and signature
    /// </summary>
    /// <param name="request">The http request containing the Authorization header</param>
    /// <param name="paymentCreated">The created payment</param>
    /// <param name="key">The signing key</param>
    /// <returns>True if the created payment matches the signature otherwise false</returns>
    public static bool ValidateOrderReference(this HttpRequest request, PaymentCreated paymentCreated, string key)
    {
        var signature = request.Headers.Authorization;
        var orderId = Encoding.UTF8.GetBytes(paymentCreated.Data.Order.Reference);
        var byteKey = Encoding.UTF8.GetBytes(key);
        using var hmac = new HMACSHA1(byteKey);
        using var stream = new MemoryStream(orderId);
        var actual = hmac.ComputeHash(orderId);
        var expected = Convert.FromBase64String(signature);

        var equal = ByteArraysEqual(actual, expected);
        return equal;
    }

    private static bool ByteArraysEqual(byte[] b1, byte[] b2)
    {
        if (b1 == b2) return true;
        if (b1 == null || b2 == null) return false;
        if (b1.Length != b2.Length) return false;
        for (int i = 0; i < b1.Length; i++)
        {
            if (b1[i] != b2[i]) return false;
        }
        return true;
    }
}
