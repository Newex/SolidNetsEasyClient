using System.Text.Json;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Tests.SerializationTests;

[UnitTest, Feature("SerializationTests")]
public class EventNameSerializationTests
{
    [Fact]
    public void Serialize_event_to_json_string()
    {
        // Arrange
        var eventName = EventName.PaymentCreated;

        // Act
        var actual = JsonSerializer.Serialize(eventName);
        const string expected = "\"payment.created\"";

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Deserialize_event_to_EventName()
    {
        // Arrange
        const string jsonEventName = "\"payment.created\"";

        // Act
        var actual = JsonSerializer.Deserialize<EventName>(jsonEventName);
        var expected = EventName.PaymentCreated;

        // Assert
        Assert.Equal(expected, actual);
    }
}
