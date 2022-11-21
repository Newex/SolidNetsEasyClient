using System;
using System.Text.Json;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Tests.SerializationTests;

[UnitTest]
public class SubscriptionStatusEnumSerializationTests
{
    [Fact]
    public void Pending_string_is_deserialized_to_SubscriptionSucceeded_enum()
    {
        // Arrange
        const string json = @"""succeeded""";
        const SubscriptionStatus expected = SubscriptionStatus.Succeeded;

        // Act
        var actual = JsonSerializer.Deserialize<SubscriptionStatus>(json);

        // Assert
        actual.Should().BeOneOf(expected);
    }
}
