using System;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;
using SolidNetsEasyClient.Tests.Tools;
using SolidNetsEasyClient.Validators;

namespace SolidNetsEasyClient.Tests.ValidatorTests;

[UnitTest]
public class SubscriptionValidatorTests
{
    private static readonly Order singleOrder = new()
    {
        Items =
        [
            Fakes.RandomItem()
        ]
    };

    [Fact]
    public void Subscription_must_at_least_have_an_order()
    {
        // Arrange
        var subscription = new SubscriptionCharge
        {
            Order = singleOrder,
        };

        // Act
        var result = SubscriptionValidator.ValidateSubscriptionCharge(subscription);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Subscription_with_subscriptionId_and_external_reference_is_invalid()
    {
        // Arrange
        var subscription = new UnscheduledSubscriptionInfo
        {
            UnscheduledSubscriptionId = Guid.NewGuid(),
            ExternalReference = "an_external_ref"
        };

        // Act
        var result = SubscriptionValidator.OnlyEitherSubscriptionIdOrExternalRef(subscription);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Subscription_with_only_subscriptionId_is_valid()
    {
        // Arrange
        var subscription = new UnscheduledSubscriptionInfo
        {
            UnscheduledSubscriptionId = Guid.NewGuid(),
        };

        // Act
        var result = SubscriptionValidator.OnlyEitherSubscriptionIdOrExternalRef(subscription);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Subscription_with_only_external_ref_is_valid()
    {
        // Arrange
        var subscription = new UnscheduledSubscriptionInfo
        {
            ExternalReference = "an_external_ref"
        };

        // Act
        var result = SubscriptionValidator.OnlyEitherSubscriptionIdOrExternalRef(subscription);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Subscription_without_subscriptionId_or_external_ref_is_invalid()
    {
        // Arrange
        var subscription = new UnscheduledSubscriptionInfo();

        // Act
        var result = SubscriptionValidator.OnlyEitherSubscriptionIdOrExternalRef(subscription);

        // Assert
        result.Should().BeFalse();
    }
}
