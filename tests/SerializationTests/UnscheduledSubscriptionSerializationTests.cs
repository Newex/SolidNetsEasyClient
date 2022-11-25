using System;
using System.Text.Json;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;

namespace SolidNetsEasyClient.Tests.SerializationTests;

[UnitTest]
public class UnscheduledSubscriptionSerializationTests
{
    [Fact]
    public void Can_deserialize_unscheduled_subscription_example_to_UnscheduledSubscriptionDetails_object()
    {
        // Arrange
        const string json = "{\n" +
            "\"unscheduledSubscriptionId\": \"92143051-9e78-40af-a01f-245ccdcd9c03\",\n" +
            "\"paymentDetails\": {\n" +
                "\"paymentType\": \"CARD\",\n" +
                "\"paymentMethod\": \"VISA\",\n" +
                "\"cardDetails\": {\n" +
                    "\"expiryDate\": \"0626\",\n" +
                    "\"maskedPan\": \"string\"\n" +
                "}\n" +
            "}\n" +
        "}";
        var expected = new UnscheduledSubscriptionDetails
        {
            UnscheduledSubscriptionId = new("92143051-9e78-40af-a01f-245ccdcd9c03"),
            PaymentDetails = new()
            {
                PaymentType = PaymentTypeEnum.Card,
                PaymentMethod = PaymentMethodEnum.Visa,
                CardDetails = new()
                {
                    ExpiryDate = new(26, 06),
                    MaskedPan = "string"
                }
            }
        };

        // Act
        var actual = JsonSerializer.Deserialize<UnscheduledSubscriptionDetails>(json);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
