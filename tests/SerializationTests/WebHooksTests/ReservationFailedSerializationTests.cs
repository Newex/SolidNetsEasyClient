using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests;

[UnitTest]
public class ReservationFailedSerializationTests
{
    const string Json = """
    {
        "id": "ef0f698086ac4e7493439ab4290695da",
        "merchantId": 100008172,
        "timestamp": "2022-06-30T10:54:07.7765+02:00",
        "event": "payment.reservation.failed",
        "data": {
            "error": {
            "code": "33",
            "message": "Direct charge failed for payment id: 020b000062bd64ae0a5e7c95f6055f66. ErrorMessage: Refused by issuer",
            "source": "Issuer"
            },
            "orderItems": [
            {
                "grossTotalAmount": 133,
                "name": "NameBulkCharge1",
                "netTotalAmount": 133,
                "quantity": 1,
                "reference": "bulk123",
                "taxRate": 0,
                "taxAmount": 0,
                "unit": "quantity",
                "unitPrice": 133
            }
            ],
            "amount": {
            "amount": 133,
            "currency": "DKK"
            },
            "paymentId": "020b000062bd64ae0a5e7c95f6055f66"
        }
    }
    """;

    private readonly ReservationFailed expected = new()
    {
        Id = new("ef0f698086ac4e7493439ab4290695da"),
        MerchantId = 100008172,
        Timestamp = DateTimeOffset.Parse("2022-06-30T10:54:07.7765+02:00", CultureInfo.InvariantCulture),
        Event = EventName.ReservationFailed,
        Data = new()
        {
            Error = new()
            {
                Code = "33",
                Message = "Direct charge failed for payment id: 020b000062bd64ae0a5e7c95f6055f66. ErrorMessage: Refused by issuer",
                Source = "Issuer"
            },
            OrderItems = new List<Item>
                {
                    new()
                    {
                        Name = "NameBulkCharge1",
                        Quantity = 1,
                        Reference = "bulk123",
                        TaxRate = 0,
                        Unit = "quantity",
                        UnitPrice = 133
                    }
                },
            Amount = new()
            {
                Amount = 133,
                Currency = Currency.DKK
            },
            PaymentId = new("020b000062bd64ae0a5e7c95f6055f66")
        }
    };

    [Fact]
    public void Can_deserialize_example_reservation_failed_string_to_ReservationFailed_object()
    {
        // Arrange

        // Act
        var actual = JsonSerializer.Deserialize<ReservationFailed>(Json);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Deserialize_reservation_failed_response_using_custom_converter()
    {
        // Arrange
        var options = new JsonSerializerOptions(JsonSerializerOptions.Default);
        options.Converters.Add(new IWebhookConverter());
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(Json));

        // Act
        var actual = JsonSerializer.Deserialize<IWebhook<WebhookData>>(ref reader, options);
        var reservationFailed = actual as ReservationFailed;

        // Assert
        reservationFailed.Should().NotBeNull().And.BeEquivalentTo(expected);
    }
}
