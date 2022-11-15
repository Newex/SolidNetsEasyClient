using System;
using System.Text.Json;
using SolidNetsEasyClient.Models.DTOs;

namespace SolidNetsEasyClient.Tests.SerializationTests;

[UnitTest]
public class MonthOnlySerializationTests
{
    [Fact]
    public void Deserialize_MMYY_json_string_to_MonthOnly_object()
    {
        // Arrange
        const string json = "\"1226\"";
        var expected = new MonthOnly(2026, 12);

        // Act
        var actual = JsonSerializer.Deserialize<MonthOnly>(json);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
