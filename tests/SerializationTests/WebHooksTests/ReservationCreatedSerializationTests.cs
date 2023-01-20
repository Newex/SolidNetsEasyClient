using System;
using System.Globalization;
using System.Text.Json;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests.ActualResponses;

namespace SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests;

[UnitTest]
public class ReservationCreatedSerializationTests
{
    [Fact]
    public void Can_deserialize_expected_string_v1_to_ReservationCreated_object()
    {
        // Arrange
        // Example from: https://developers.nets.eu/nets-easy/en-EU/api/webhooks/#reservation-created
        const string json = "{\n" +
            "\"id\": \"6f081ae39b9846c4bacff88fa2cecc98\",\n" +
            "\"merchantId\": 100001234,\n" +
            "\"timestamp\": \"2022-09-21T09:50:05.9440+00:00\",\n" +
            "\"event\": \"payment.reservation.created\",\n" +
            "\"data\": {\n" +
                "\"cardDetails\": {\n" +
                    "\"creditDebitIndicator\": \"D\",\n" +
                    "\"expiryMonth\": 1,\n" +
                    "\"expiryYear\": 24,\n" +
                    "\"issuerCountry\": \"NO\",\n" +
                    "\"truncatedPan\": \"492500******0004\",\n" +
                    "\"threeDSecure\": {\n" +
                        "\"authenticationEnrollmentStatus\": \"Y\",\n" +
                        "\"authenticationStatus\": \"Y\",\n" +
                        "\"eci\": \"05\"\n" +
                    "}\n" +
                "},\n" +
                "\"paymentMethod\": \"Visa\",\n" +
                "\"paymentType\": \"CARD\",\n" +
                "\"consumer\": {\n" +
                    "\"ip\": \"10.230.197.32\"\n" +
                "},\n" +
                "\"reservationReference\": \"683884\",\n" +
                "\"reserveId\": \"6f081ae39b9846c4bacff88fa2cecc98\",\n" +
                "\"amount\": {\n" +
                    "\"amount\": 1000,\n" +
                    "\"currency\": \"SEK\"\n" +
                "},\n" +
                "\"paymentId\": \"01d40000632ade184172b85d8cc3f516\"\n" +
            "}\n" +
        "}";
        var expected = new ReservationCreatedV1
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

        // Act
        var actual = JsonSerializer.Deserialize<ReservationCreatedV1>(json);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Can_deserialize_expected_v2_string_to_ReservationCreated_object()
    {
        // Arrange
        const string json = "{\n" +
        "\"id\": \"c25459e92ba54be1925493f987fb05a7\",\n" +
        "\"timestamp\": \"2021-05-04T22:09:08.4342+02:00\",\n" +
        "\"merchantNumber\": 100017120,\n" +
        "\"event\": \"payment.reservation.created.v2\",\n" +
        "\"data\": {\n" +
            "\"paymentMethod\": \"Visa\",\n" +
            "\"paymentType\": \"CARD\",\n" +
            "\"amount\": {\n" +
                "\"amount\": 5500,\n" +
                "\"currency\": \"SEK\"\n" +
            "},\n" +
            "\"paymentId\": \"02a900006091a9a96937598058c4e474\"\n" +
        "}\n" +
        "}";

        var expected = new ReservationCreatedV2
        {
            Id = new("c25459e92ba54be1925493f987fb05a7"),
            Timestamp = DateTimeOffset.Parse("2021-05-04T22:09:08.4342+02:00", CultureInfo.InvariantCulture),
            MerchantNumber = 100017120,
            Event = EventName.ReservationCreatedV2,
            Data = new()
            {
                PaymentMethod = PaymentMethodEnum.Visa,
                PaymentType = PaymentTypeEnum.Card,
                Amount = new()
                {
                    Amount = 55_00,
                    Currency = Currency.SEK
                },
                PaymentId = new("02a900006091a9a96937598058c4e474")
            }
        };

        // Act
        var actual = JsonSerializer.Deserialize<ReservationCreatedV2>(json);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Can_deserialize_actual_response_to_ReservationCreatedV1_object()
    {
        // Arrange
        const string response = CleanedResponses.ReservationCreatedV1;
        var expected = new ReservationCreatedV1
        {
            Id = new("ffb26a376517427da7236b55e06478d9"),
            MerchantId = 123456,
            Timestamp = DateTimeOffset.Parse("2022-11-12T06:33:24.3795+00:00", CultureInfo.InvariantCulture),
            Event = EventName.ReservationCreatedV1,
            Data = new()
            {
                CardDetails = new()
                {
                    CreditDebitIndicator = "D",
                    ExpiryMonth = 12,
                    ExpiryYear = 26,
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
                    IP = "192.168.0.1"
                },
                ReservationReference = "211569",
                ReserveId = new("ffb26a376517427da7236b55e06478d9"),
                Amount = new()
                {
                    Amount = 40_00,
                    Currency = Currency.DKK
                },
                PaymentId = new("023e0000636f3df7e30174516bf6aa48")
            }
        };

        // Act
        var actual = JsonSerializer.Deserialize<ReservationCreatedV1>(response);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Can_deserialize_actual_response_to_ReservationCreatedV2_object()
    {
        // Arrange
        const string response = CleanedResponses.ReservationCreatedV2;
        var expected = new ReservationCreatedV2
        {
            Id = new("ffb26a376517427da7236b55e06478d9"),
            Timestamp = DateTimeOffset.Parse("2022-11-12T06:33:24.3795+00:00", CultureInfo.InvariantCulture),
            MerchantNumber = 123456,
            Event = EventName.ReservationCreatedV2,
            Data = new()
            {
                PaymentMethod = PaymentMethodEnum.Visa,
                PaymentType = PaymentTypeEnum.Card,
                Amount = new()
                {
                    Amount = 40_00,
                    Currency = Currency.DKK
                },
                PaymentId = new("023e0000636f3df7e30174516bf6aa48")
            }
        };

        // Act
        var actual = JsonSerializer.Deserialize<ReservationCreatedV2>(response);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
