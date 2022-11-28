using System.Collections.Generic;
using System.Text.Json;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;

namespace SolidNetsEasyClient.Tests.SerializationTests;

[UnitTest]
public class PaginatedSubscriptionSerializationTests
{
    [Fact]
    public void Can_deserialize_example_paginated_subscriptions_response_to_PaginatedSubscriptions_object()
    {
        // Arrange
        const string json = "{\n" +
        "\"page\": [\n" +
            "{\n" +
                "\"subscriptionId\": \"d079718b-ff63-45dd-947b-4950c023750f\",\n" +
                "\"paymentId\": \"472e651e-5a1e-424d-8098-23858bf03ad7\",\n" +
                "\"chargeId\": \"aec0aceb-a4db-49fb-b366-75e90229c640\",\n" +
                "\"status\": \"Succeeded\",\n" +
                "\"message\": \"string\",\n" +
                "\"code\": \"string\",\n" +
                "\"source\": \"string\",\n" +
                "\"externalReference\": \"string\"\n" +
            "}\n" +
        "],\n" +
        "\"more\": true,\n" +
        "\"status\": \"Processing\"\n" +
        "}";
        var expected = new PageResult<SubscriptionProcessStatus>
        {
            Page = new List<SubscriptionProcessStatus>()
            {
                new()
                {
                    SubscriptionId = new("d079718b-ff63-45dd-947b-4950c023750f"),
                    PaymentId = new("472e651e-5a1e-424d-8098-23858bf03ad7"),
                    ChargeId = new("aec0aceb-a4db-49fb-b366-75e90229c640"),
                    Status = SubscriptionStatus.Succeeded,
                    Message = "string",
                    Code = "string",
                    Source = "string",
                    ExternalReference = "string"
                }
            },
            More = true,
            Status = BulkStatus.Processing
        };

        // Act
        var actual = JsonSerializer.Deserialize<PageResult<SubscriptionProcessStatus>>(json);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
