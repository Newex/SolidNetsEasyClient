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
        const ConsumerTypeEnum consumerType = ConsumerTypeEnum.B2B;

        // Act
        var actual = JsonSerializer.Serialize(consumerType);
        const string expected = "\"B2B\"";

        // Assert
        Assert.Equal(expected, actual);
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
