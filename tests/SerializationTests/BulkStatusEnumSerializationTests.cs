using System.Text.Json;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Tests.SerializationTests;

[UnitTest]
public class BulkStatusEnumSerializationTests
{
    [Fact]
    public void Processing_json_string_deserializes_to_Processing_enum()
    {
        // Arrange
        const string json = "\"processing\"";
        const BulkStatus expected = BulkStatus.Processing;

        // Act
        var actual = JsonSerializer.Deserialize<BulkStatus>(json);

        // Assert
        actual.Should().BeOneOf(expected);
    }
}
