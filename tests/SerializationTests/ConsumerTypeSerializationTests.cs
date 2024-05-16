using System.Text.Json;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Tests.SerializationTests;

[UnitTest, Feature("SerializationTests")]
public class ConsumerTypeSerializationTests
{
    [Fact]
    public void Serialize_enum_to_json_string()
    {
        // Arrange
        const ConsumerTypeEnum ConsumerType = ConsumerTypeEnum.B2B;

        // Act
        var actual = JsonSerializer.Serialize(ConsumerType);
        const string Expected = "\"B2B\"";

        // Assert
        actual.Should().Be(Expected);
    }

    [Fact]
    public void Deserialize_consumer_type_string_to_enum()
    {
        // Arrange
        const string consumerType = "\"B2B\"";

        // Act
        var actual = JsonSerializer.Deserialize<ConsumerTypeEnum>(consumerType);
        const ConsumerTypeEnum expected = ConsumerTypeEnum.B2B;

        // Assert
        Assert.Equal(expected, actual);
    }
}
