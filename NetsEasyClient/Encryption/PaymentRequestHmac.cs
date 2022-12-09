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
    /// Encode an input with a given key
    /// </summary>
    /// <param name="input">The input</param>
    /// <param name="key">The key</param>
    /// <returns>A base62 encoded signature</returns>
    public static string Encode(string input, string key)
    {
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var byteKey = Encoding.UTF8.GetBytes(key);
        using var hmac = new HMACSHA1(byteKey);
        using var stream = new MemoryStream(inputBytes);
        var hash = hmac.ComputeHash(stream);
        var converter = new CustomBase62Converter();
        var encode = converter.Encode(hash);
        return encode;
    }

    /// <summary>
    /// Sign an <see cref="Order"/> which contains a reference with HMAC-SHA1
    /// </summary>
    /// <param name="order">The order</param>
    /// <param name="key">The signing key</param>
    /// <returns>A base62 encoded signature</returns>
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
        var hash = hmac.ComputeHash(stream);
        var converter = new CustomBase62Converter();
        var encode = converter.Encode(hash);
        return encode;
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
        var converter = new CustomBase62Converter();
        var expected = converter.Decode(signature);

        var equal = ByteArraysEqual(actual, expected);
        return equal;
    }

    /// <summary>
    /// Validate a string with a given order reference
    /// </summary>
    /// <param name="request">The http request</param>
    /// <param name="orderReference">The order reference</param>
    /// <param name="key">The corresponding signing key</param>
    /// <returns>True if the created payment matches the signature otherwise false</returns>
    public static bool ValidateOrderReference(this HttpRequest request, string orderReference, string key)
    {
        var signature = request.Headers.Authorization;
        var orderId = Encoding.UTF8.GetBytes(orderReference);
        var byteKey = Encoding.UTF8.GetBytes(key);
        using var hmac = new HMACSHA1(byteKey);
        using var stream = new MemoryStream(orderId);
        var actual = hmac.ComputeHash(orderId);
        var converter = new CustomBase62Converter();
        var expected = converter.Decode(signature);

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
