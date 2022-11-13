using System;
using System.Text.Json;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Tests.SerializationTests;

[UnitTest]
public class CurrenyEnumStringConverterSerializationTests
{
    [Fact]
    public void String_USD_deserializes_to_USD_enum()
    {
        // Arrange
        const string currency = "\"USD\"";
        const Currency expected = Currency.USD;

        // Act
        var actual = JsonSerializer.Deserialize<Currency>(currency);

        // Assert
        actual.Should().Be(expected);
    }
}
