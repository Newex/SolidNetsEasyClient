using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using SolidNetsEasyClient.Constants;

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
}
