using System;
using System.Text.Json;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;
using SolidNetsEasyClient.Tests.Tools;

namespace SolidNetsEasyClient.Tests.SerializationTests;

[UnitTest, Feature("SerializationTests")]
public class PaymentStatusSerializationTests
{
    [Fact]
    public void Can_deserialize_payment_status_to_object()
    {
        // Arrange
        const string json = TestResponses.PaymentStatusResponseJson;

        // Act
        var status = JsonSerializer.Deserialize<PaymentStatus>(json);
        var hasID = status!.Payment.PaymentId != Guid.Empty;

        // Assert
        Assert.True(hasID);
    }
}
