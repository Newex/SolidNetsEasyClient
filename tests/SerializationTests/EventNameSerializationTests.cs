using System.Text.Json;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Tests.SerializationTests;

[UnitTest, Feature("SerializationTests")]
public class EventNameSerializationTests
{
    [Theory]
    [InlineData(EventName.PaymentCreated, EventNameConstants.PaymentCreated)]
    [InlineData(EventName.ReservationCreatedV1, EventNameConstants.ReservationCreatedV1)]
    [InlineData(EventName.ReservationCreatedV2, EventNameConstants.ReservationCreatedV2)]
    [InlineData(EventName.ReservationFailed, EventNameConstants.ReservationFailed)]
    [InlineData(EventName.CheckoutCompleted, EventNameConstants.CheckoutCompleted)]
    [InlineData(EventName.ChargeCreated, EventNameConstants.ChargeCreated)]
    [InlineData(EventName.ChargeFailed, EventNameConstants.ChargeFailed)]
    [InlineData(EventName.RefundInitiated, EventNameConstants.RefundInitiated)]
    [InlineData(EventName.RefundFailed, EventNameConstants.RefundFailed)]
    [InlineData(EventName.RefundCompleted, EventNameConstants.RefundCompleted)]
    [InlineData(EventName.PaymentCancelled, EventNameConstants.ReservationCancelled)]
    [InlineData(EventName.ReservationCancellationFailed, EventNameConstants.ReservationCancellationFailed)]
    public void Serialize_event_to_json_string(EventName eventEnum, string expected)
    {
        // Arrange
        var quotedEvent = "\"" + expected + "\"";

        // Act
        var actual = JsonSerializer.Serialize(eventEnum);

        // Assert
        actual.Should().BeEquivalentTo(quotedEvent);
    }

    [Fact]
    public void Deserialize_event_to_EventName()
    {
        // Arrange
        const string jsonEventName = "\"payment.created\"";

        // Act
        var actual = JsonSerializer.Deserialize<EventName>(jsonEventName);
        const EventName expected = EventName.PaymentCreated;

        // Assert
        Assert.Equal(expected, actual);
    }
}
