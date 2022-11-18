using System;
using SolidNetsEasyClient.Clients;
using SolidNetsEasyClient.Validators;

namespace SolidNetsEasyClient.Tests.ValidatorTests;

[UnitTest]
public class SubscriptionValidatorTests
{
    [Fact]
    public void Subscription_with_subscriptionId_and_external_reference_is_invalid()
    {
        // Arrange
        var subscription = new SubscriptionCharge
        {
            SubscriptionId = Guid.NewGuid(),
            ExternalReference = "an_external_ref"
        };

        // Act
        var result = SubscriptionValidator.OnlyEitherSubscriptionIdOrExternalRef(subscription);

        // Assert
        result.Should().BeFalse();
    }
}
