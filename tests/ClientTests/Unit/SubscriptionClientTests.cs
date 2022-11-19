using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using SolidNetsEasyClient.Clients;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Models.DTOs;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;

namespace SolidNetsEasyClient.Tests.ClientTests.Unit;

[UnitTest]
public class SubscriptionClientTests
{
    [Fact]
    public async void Retrieving_non_existing_subscription_throws_exception()
    {
        // Arrange
        const string response = "{\"error\": \"Not found\"}";
        var subscriptionId = Guid.NewGuid();
        var client = Setup.SubscriptionClient(HttpMethod.Get, NetsEndpoints.Relative.Subscription + $"/{subscriptionId}", HttpStatusCode.NotFound, response);

        // Act
        var act = async () => await client.RetrieveSubscriptionAsync(subscriptionId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async void Bulk_charging_subscriptions_returns_a_deserializable_BulkId_object()
    {
        const string response = @"{ ""bulkId"": ""50490f2b-98bd-4782-b08d-413ee70aa1f7"" }";
        var subscriptions = new List<SubscriptionCharge>
        {
            new()
            {
                ExternalReference = "my_ref1",
                Order = new()
                {
                    Currency = Currency.DKK,
                    Items = new List<Item>
                    {
                        Tools.Fakes.RandomItem()
                    }
                }
            }
        };

        var client = Setup.SubscriptionClient(HttpMethod.Post, NetsEndpoints.Relative.Subscription + "/charges", HttpStatusCode.Accepted, response);

        // Act
        var result = await client.BulkChargeSubscriptionsAsync(subscriptions, CancellationToken.None);

        // Assert
        result.Should().Match<BulkId>(x => x.Id == new Guid("50490f2b-98bd-4782-b08d-413ee70aa1f7"));
    }
}
