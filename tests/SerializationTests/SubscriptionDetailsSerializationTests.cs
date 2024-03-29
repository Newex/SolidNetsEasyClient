using System;
using System.Globalization;
using System.Text.Json;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;
using SolidNetsEasyClient.Tests.Tools;

namespace SolidNetsEasyClient.Tests.SerializationTests;

[UnitTest]
public class SubscriptionDetailsSerializationTests
{
    [Fact]
    public void Can_deserialize_retrieved_subscription_to_SubscriptionDetails_object()
    {
        // Arrange
        const string json = TestResponses.SubscriptionStatus;
        var expected = new SubscriptionDetails
        {
            SubscriptionId = new("d079718b-ff63-45dd-947b-4950c023750f"),
            EndDate = DateTimeOffset.Parse("2019-08-24T14:15:22Z", CultureInfo.InvariantCulture),
            PaymentDetails = new()
            {
                PaymentType = PaymentTypeEnum.Card,
                PaymentMethod = PaymentMethodEnum.Visa,
                CardDetails = new()
                {
                    ExpiryDate = new(2023, 12),
                    MaskedPan = "string"
                },
            },
            ImportError = new()
            {
                ImportStepsResponseCode = "string",
                ImportStepsResponseSource = "string",
                ImportStepsResponseText = "string"
            }
        };

        // Act
        var actual = JsonSerializer.Deserialize<SubscriptionDetails>(json);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
