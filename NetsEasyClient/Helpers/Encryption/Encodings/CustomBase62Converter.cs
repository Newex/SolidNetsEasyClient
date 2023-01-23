/*
Single source code copied (attribution):
---------------------------------------------------------------
MIT License

Copyright (c) 2021 Daniel Destouche

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
---------------------------------------------------------------
Modified source code from: https://github.com/ghost1face/base62/blob/38251a54a5fd442835496271f5863658c6a4c920/Base62/Base62Converter.cs
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace SolidNetsEasyClient.Helpers.Encryption.Encodings;

/// <summary>
/// Encodes and decodes text to and from base62 encoding.
/// </summary>
/// <remarks>
/// Modified from: https://github.com/ghost1face/base62
/// </remarks>
public static class CustomBase62Converter
{
    private const string DEFAULT_CHARACTER_SET = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private const string INVERTED_CHARACTER_SET = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    /// <summary>
    /// Encodes the input text to Base62 format.
    /// </summary>
    /// <param name="value">The input value.</param>
    /// <param name="inverted">Default false if true then an inverted character set is used</param>
    /// <returns>Encoded base62 value.</returns>
    public static string Encode(string value, bool inverted = false)
    {
        var arr = Encoding.UTF8.GetBytes(value);
        var converted = BaseConvert(arr, 256, 62);
        return AsString(converted, inverted);
    }

    /// <summary>
    /// Decodes the input text from Base62 format.
    /// </summary>
    /// <param name="value">The input value.</param>
    /// <param name="inverted">Default false if true then an inverted character set is used</param>
    /// <returns>The decoded value.</returns>
    public static byte[] Decode(string value, bool inverted = false)
    {
        var characterSet = inverted ? DEFAULT_CHARACTER_SET : INVERTED_CHARACTER_SET;
        var arr = new byte[value.Length];
        for (var i = 0; i < arr.Length; i++)
        {
            arr[i] = (byte)characterSet.IndexOf(value[i]);
        }

        var converted = Decode(arr);
        return converted;
        //return Encoding.UTF8.GetString(converted, 0, converted.Length);
    }

    /// <summary>
    /// Encodes the input bytes to Base62 format.
    /// </summary>
    /// <param name="value">The input value.</param>
    /// <param name="inverted">Default false if true then an inverted character set is used</param>
    /// <returns>Encoded base62 value.</returns>
    public static string Encode(byte[] value, bool inverted = false)
    {
        var encoded = BaseConvert(value, 256, 62);
        return AsString(encoded, inverted);
    }

    /// <summary>
    /// Convert a base62 byte array to a string representation
    /// </summary>
    /// <param name="base62">The byte array</param>
    /// <param name="inverted">Default false if true then an inverted character set is used</param>
    /// <returns>A base62 string</returns>
    public static string AsString(byte[] base62, bool inverted = false)
    {
        var characterSet = inverted ? DEFAULT_CHARACTER_SET : INVERTED_CHARACTER_SET;
        var builder = new StringBuilder();
        foreach (var c in base62)
        {
            _ = builder.Append(characterSet[c]);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Decodes the input bytes from Base62 format.
    /// </summary>
    /// <param name="value">The inpnut value.</param>
    /// <returns>The decoded value.</returns>
    public static byte[] Decode(byte[] value)
    {
        return BaseConvert(value, 62, 256);
    }

    /// <summary>
    /// Converts source byte array from the source base to the destination base.
    /// </summary>
    /// <param name="source">Byte array to convert.</param>
    /// <param name="sourceBase">Source base to convert from.</param>
    /// <param name="targetBase">Target base to convert to.</param>
    /// <returns>Converted byte array.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when value is outside 2 or 256 of either <paramref name="sourceBase"/> or <paramref name="targetBase"/></exception>
    private static byte[] BaseConvert(byte[] source, int sourceBase, int targetBase)
    {
        if (targetBase is < 2 or > 256)
        {
            throw new ArgumentOutOfRangeException(nameof(targetBase), targetBase, "Value must be between 2 & 256 (inclusive)");
        }

        if (sourceBase is < 2 or > 256)
        {
            throw new ArgumentOutOfRangeException(nameof(sourceBase), sourceBase, "Value must be between 2 & 256 (inclusive)");
        }

        // Set initial capacity estimate if the size is small.
        var startCapacity = source.Length < 1028
            ? (int)(source.Length * 1.5)
            : source.Length;

        var result = new List<int>(startCapacity);
        var quotient = new List<byte>((int)(source.Length * 0.5));
        int count;
        int initialStartOffset = 0;

        // This is a bug fix for the following issue:
        // https://github.com/ghost1face/base62/issues/4
        while (source[initialStartOffset] == 0)
        {
            result.Add(0);
            initialStartOffset++;
        }

        int startOffset = initialStartOffset;

        while ((count = source.Length) > 0)
        {
            quotient.Clear();
            int remainder = 0;
            for (var i = initialStartOffset; i != count; i++)
            {
                int accumulator = source[i] + (remainder * sourceBase);
                byte digit = (byte)((accumulator - (accumulator % targetBase)) / targetBase);
                remainder = accumulator % targetBase;
                if (quotient.Count > 0 || digit != 0)
                {
                    quotient.Add(digit);
                }
            }

            result.Insert(startOffset, remainder);
            source = quotient.ToArray();
            initialStartOffset = 0;
        }

        var output = new byte[result.Count];

        for (int i = 0; i < result.Count; i++)
        {
            output[i] = (byte)result[i];
        }

        return output;
    }

    /// <summary>
    /// Character set to use for encoding/decoding.
    /// </summary>
    public enum CharacterSet
    {
        /// <summary>
        /// Alpha numeric character set, using capital letters first before lowercase.
        /// </summary>
        DEFAULT = 0,

        /// <summary>
        /// Alpha numeric character set, using lower case letters first before uppercase.
        /// </summary>
        INVERTED = 1
    }
}
