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

    [Fact]
    public void Can_deserialize_result_from_an_unscheduled_subscription_charge_to_UnscheduledSubscriptionChargeResult()
    {
        // Arrange
        const string json = "{\n" +
            "\"paymentId\": \"472e651e-5a1e-424d-8098-23858bf03ad7\",\n" +
            "\"chargeId\": \"aec0aceb-a4db-49fb-b366-75e90229c640\"\n" +
        "}";
        var expected = new UnscheduledSubscriptionChargeResult
        {
            PaymentId = new("472e651e-5a1e-424d-8098-23858bf03ad7"),
            ChargeId = new("aec0aceb-a4db-49fb-b366-75e90229c640")
        };

        // Act
        var actual = JsonSerializer.Deserialize<UnscheduledSubscriptionChargeResult>(json);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
