using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Models.DTOs;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;

namespace SolidNetsEasyClient.Tests.ClientTests.Unit;

[UnitTest]
public class SubscriptionClientTests
{
    [Fact]
    public async void Retrieving_non_existing_subscription_throws_exception()
    {
        // Arrange
        const string response = /*lang=json,strict*/ "{\"error\": \"Not found\"}";
        var subscriptionId = Guid.NewGuid();
        var client = Setup.SubscriptionClient(HttpMethod.Get, NetsEndpoints.Relative.Subscription + $"/{subscriptionId}", HttpStatusCode.NotFound, response);

        // Act
        var act = async () => await client.RetrieveSubscriptionAsync(subscriptionId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async void Retrieving_existing_subscription_by_external_reference_returns_SubscriptionDetails()
    {
        // Arrange
        const string response = "{\n" +
            "\"subscriptionId\": \"d079718b-ff63-45dd-947b-4950c023750f\",\n" +
            "\"frequency\": 0,\n" +
            "\"interval\": 0,\n" +
            "\"endDate\": \"2019-08-24T14:15:22Z\",\n" +
            "\"paymentDetails\": {\n" +
                "\"paymentType\": \"Card\",\n" +
                "\"paymentMethod\": \"VISA\",\n" +
                "\"cardDetails\": {\n" +
                    "\"expiryDate\": \"1226\",\n" +
                    "\"maskedPan\": \"string\"\n" +
                "}\n" +
            "},\n" +
            "\"importError\": {\n" +
                "\"importStepsResponseCode\": \"string\",\n" +
                "\"importStepsResponseSource\": \"string\",\n" +
                "\"importStepsResponseText\": \"string\"\n" +
            "}\n" +
        "}";
        var expected = new SubscriptionDetails
        {
            SubscriptionId = new("d079718b-ff63-45dd-947b-4950c023750f"),
            Frequency = 0,
            Interval = 0,
            EndDate = DateTimeOffset.Parse("2019-08-24T14:15:22Z", CultureInfo.InvariantCulture),
            PaymentDetails = new()
            {
                PaymentType = PaymentTypeEnum.Card,
                PaymentMethod = PaymentMethodEnum.Visa,
                CardDetails = new()
                {
                    ExpiryDate = new(2026, 12),
                    MaskedPan = "string"
                }
            },
            ImportError = new()
            {
                ImportStepsResponseCode = "string",
                ImportStepsResponseSource = "string",
                ImportStepsResponseText = "string"
            }
        };
        var externalReference = Guid.NewGuid();
        var client = Setup.SubscriptionClient(HttpMethod.Get, NetsEndpoints.Relative.Subscription + "?externalReference=" + externalReference, HttpStatusCode.OK, response);

        // Act
        var actual = await client.RetrieveSubscriptionByExternalReferenceAsync(externalReference.ToString(), CancellationToken.None);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async void Bulk_charging_subscriptions_returns_a_deserializable_BulkId_object()
    {
        const string response = /*lang=json,strict*/ @"{ ""bulkId"": ""50490f2b-98bd-4782-b08d-413ee70aa1f7"" }";
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

    [Fact]
    public async void Retrieving_bulk_charges_returns_PaginatedSubscriptions()
    {
        // Arrange
        const string response = "{\n" +
        "\"page\": [\n" +
            "{\n" +
                "\"subscriptionId\": \"d079718b-ff63-45dd-947b-4950c023750f\",\n" +
                "\"paymentId\": \"472e651e-5a1e-424d-8098-23858bf03ad7\",\n" +
                "\"chargeId\": \"aec0aceb-a4db-49fb-b366-75e90229c640\",\n" +
                "\"status\": \"Failed\",\n" +
                "\"message\": \"string\",\n" +
                "\"code\": \"string\",\n" +
                "\"source\": \"string\",\n" +
                "\"externalReference\": \"string\"\n" +
            "}\n" +
        "],\n" +
            "\"more\": true,\n" +
            "\"status\": \"Processing\"\n" +
        "}\n";
        var expected = new PageResult<SubscriptionProcessStatus>
        {
            Page = new List<SubscriptionProcessStatus>
            {
                new()
                {
                    SubscriptionId = new("d079718b-ff63-45dd-947b-4950c023750f"),
                    PaymentId = new("472e651e-5a1e-424d-8098-23858bf03ad7"),
                    ChargeId = new("aec0aceb-a4db-49fb-b366-75e90229c640"),
                    Status = SubscriptionStatus.Failed,
                    Message = "string",
                    Code = "string",
                    Source = "string",
                    ExternalReference = "string"
                }
            },
            More = true,
            Status = BulkStatus.Processing
        };

        var bulkId = Guid.NewGuid();
        var client = Setup.SubscriptionClient(HttpMethod.Get, NetsEndpoints.Relative.Subscription + $"/charges/{bulkId}?skip=2&take=5", HttpStatusCode.OK, response);

        // Act
        var actual = await client.RetrieveBulkChargesAsync(bulkId, skip: 2, take: 5, CancellationToken.None);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async void Verify_subscriptions_returns_bulk_id()
    {
        // Arrange
        const string response = /*lang=json,strict*/ @"{ ""bulkId"": ""50490f2b-98bd-4782-b08d-413ee70aa1f7"" }";
        var bulk = new BulkSubscriptionVerification
        {
            ExternalBulkVerificationId = "123",
            Subscriptions = new List<BaseSubscription>
            {
                new()
                {
                    SubscriptionId = Guid.NewGuid()
                }
            }
        };
        var expected = new BulkId
        {
            Id = new Guid("50490f2b-98bd-4782-b08d-413ee70aa1f7")
        };
        var client = Setup.SubscriptionClient(HttpMethod.Post, NetsEndpoints.Relative.Subscription + "/verifications", HttpStatusCode.Accepted, response);

        // Act
        var actual = await client.VerifyBulkSubscriptionsAsync(bulk, CancellationToken.None);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async void Retrieve_paginated_subscriptions()
    {
        // Arrange
        const string response = "{\n" +
        "\"page\": [\n" +
            "{\n" +
                "\"subscriptionId\": \"d079718b-ff63-45dd-947b-4950c023750f\",\n" +
                "\"paymentId\": \"472e651e-5a1e-424d-8098-23858bf03ad7\",\n" +
                "\"chargeId\": \"aec0aceb-a4db-49fb-b366-75e90229c640\",\n" +
                "\"status\": \"Failed\",\n" +
                "\"externalReference\": \"string\"\n" +
            "}\n" +
        "],\n" +
            "\"more\": true,\n" +
            "\"status\": \"Processing\"\n" +
        "}\n";
        var expected = new PageResult<SubscriptionProcessStatus>
        {
            Page = new List<SubscriptionProcessStatus>
            {
                new()
                {
                    SubscriptionId = new("d079718b-ff63-45dd-947b-4950c023750f"),
                    PaymentId = new("472e651e-5a1e-424d-8098-23858bf03ad7"),
                    ChargeId = new("aec0aceb-a4db-49fb-b366-75e90229c640"),
                    Status = SubscriptionStatus.Failed,
                    ExternalReference = "string"
                }
            },
            More = true,
            Status = BulkStatus.Processing
        };
        var bulkId = Guid.NewGuid();
        var client = Setup.SubscriptionClient(HttpMethod.Get, NetsEndpoints.Relative.Subscription + $"/verifications/{bulkId}?skip=2&take=5", HttpStatusCode.OK, response);

        // Act
        var actual = await client.RetrieveBulkVerificationsAsync(bulkId, skip: 2, take: 5, null, null, CancellationToken.None);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
