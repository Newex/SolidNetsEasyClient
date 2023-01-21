using System;
using System.Collections.Generic;
using SolidNetsEasyClient.Models.DTOs;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;
using SolidNetsEasyClient.Tests.Tools;
using SolidNetsEasyClient.Validators;

namespace SolidNetsEasyClient.Tests.ValidatorTests;

[UnitTest]
public class SubscriptionValidatorTests
{
    private static readonly Order SingleOrder = new()
    {
        Items = new List<Item>
        {
            Fakes.RandomItem()
        }
    };

    [Fact]
    public void Subscription_with_subscriptionId_and_external_reference_is_invalid()
    {
        // Arrange
        var subscription = new SubscriptionCharge
        {
            Order = SingleOrder,
            SubscriptionId = Guid.NewGuid(),
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
        var subscription = new SubscriptionCharge
        {
            Order = SingleOrder,
            SubscriptionId = Guid.NewGuid(),
            ExternalReference = null
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
        var subscription = new SubscriptionCharge
        {
            Order = SingleOrder,
            SubscriptionId = null,
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
        var subscription = new SubscriptionCharge
        {
            Order = SingleOrder,
            SubscriptionId = null,
            ExternalReference = null
        };

        // Act
        var result = SubscriptionValidator.OnlyEitherSubscriptionIdOrExternalRef(subscription);

        // Assert
        result.Should().BeFalse();
    }
}
