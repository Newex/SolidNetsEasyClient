using System.Text.Json;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Tests.SerializationTests;

[UnitTest, Feature("SerializationTests")]
public class RefundStateEnumSerializationTests
{
    [Fact]
    public void Pending_json_deserializes_to_pending_enum()
    {
        // Arrange
        const string json = "\"Pending\"";

        // Act
        var pending = JsonSerializer.Deserialize<RefundStateEnum>(json);

        // Assert
        Assert.Equal(RefundStateEnum.Pending, pending);
    }

    [Fact]
    public void Pending_enum_serializes_to_pending_json()
    {
        // Arrange
        const RefundStateEnum pending = RefundStateEnum.Pending;

        // Act
        var json = JsonSerializer.Serialize(pending);

        // Assert
        Assert.Equal("\"Pending\"", json);
    }
}
