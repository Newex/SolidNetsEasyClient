using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;
using static VerifyXunit.Verifier;

namespace SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests.SnapshotTests;

[SnapshotTest]
public class ReservationCreatedV1SerializationSnapshotTests
{
    private readonly ReservationCreatedV1 expected = new()
    {
        Id = new("6f081ae39b9846c4bacff88fa2cecc98"),
        MerchantId = 100001234,
        Timestamp = DateTimeOffset.Parse("2022-09-21T09:50:05.9440+00:00", CultureInfo.InvariantCulture),
        Event = EventName.ReservationCreatedV1,
        Data = new()
        {
            CardDetails = new()
            {
                CreditDebitIndicator = "D",
                ExpiryMonth = 1,
                ExpiryYear = 24,
                IssuerCountry = "NO",
                TruncatedPan = "492500******0004",
                ThreeDSecure = new()
                {
                    AuthenticationEnrollmentStatus = "Y",
                    AuthenticationStatus = "Y",
                    ECI = "05"
                }
            },
            PaymentMethod = PaymentMethodEnum.Visa,
            PaymentType = PaymentTypeEnum.Card,
            Consumer = new()
            {
                IP = "10.230.197.32"
            },
            ReservationReference = "683884",
            ReserveId = new("6f081ae39b9846c4bacff88fa2cecc98"),
            Amount = new()
            {
                Amount = 10_00,
                Currency = Currency.SEK
            },
            PaymentId = new("01d40000632ade184172b85d8cc3f516")
        }
    };

    [Fact]
    public Task Serialize_ReservationCreated()
    {
        // Arrange
        var options = new JsonSerializerOptions(JsonSerializerOptions.Default);
        options.Converters.Add(new IWebhookConverter());
        var memoryStream = new MemoryStream();
        var writer = new Utf8JsonWriter(memoryStream);

        // Act
        JsonSerializer.Serialize<IWebhook<WebhookData>>(writer, expected, options);

        // Assert
        var bytes = memoryStream.ToArray();
        var jsonString = Encoding.UTF8.GetString(bytes);
        return VerifyJson(jsonString, SnapshotSettings.Settings);
    }
}
