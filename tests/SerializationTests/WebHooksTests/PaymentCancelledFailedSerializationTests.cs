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
public class PaymentCancelledFailedSerializationTests
{
    const string Json = """
    {
        "id": "df7f9346097842bdb90c869b5c9ccfa9",
        "merchantId": 100017120,
        "timestamp": "2021-05-06T11:37:30.1114+02:00",
        "event": "payment.cancel.failed",
        "data": {
            "error": {
                "code": "25",
                "message": "Trans not found",
                "source": "Internal"
            },
            "cancelId": "df7f9346097842bdb90c869b5c9ccfa9",
            "orderItems": [
                {
                    "reference": "Sneaky NE2816-82",
                    "name": "Sneaky",
                    "quantity": 2,
                    "unit": "pcs",
                    "unitPrice": 2500,
                    "taxRate": 1000,
                    "taxAmount": 500,
                    "netTotalAmount": 5000,
                    "grossTotalAmount": 5500
                }
            ],
            "amount": {
                "amount": 5500,
                "currency": "SEK"
            },
            "paymentId": "023a00005ea744ed368812223c86c299"
        }
    }
    """;

    private readonly PaymentCancellationFailed expected = new()
    {
        Id = new("df7f9346097842bdb90c869b5c9ccfa9"),
        MerchantId = 100017120,
        Timestamp = DateTimeOffset.Parse("2021-05-06T11:37:30.1114+02:00", CultureInfo.InvariantCulture),
        Event = EventName.PaymentCancellationFailed,
        Data = new()
        {
            Error = new()
            {
                Code = "25",
                Message = "Trans not found",
                Source = "Internal"
            },
            CancelId = new("df7f9346097842bdb90c869b5c9ccfa9"),
            OrderItems =
                [
                    new()
                    {
                        Reference = "Sneaky NE2816-82",
                        Name = "Sneaky",
                        Quantity = 2,
                        Unit = "pcs",
                        UnitPrice = 2500,
                        TaxRate = 1000,
                    }
                ],
            Amount = new()
            {
                Amount = 5500,
                Currency = Currency.SEK
            },
            PaymentId = new("023a00005ea744ed368812223c86c299")
        }
    };

    [Fact]
    public void Can_deserialize_payment_cancelled_failed_event_to_PaymentCancelledFailed_object()
    {
        // Arrange

        // Act
        var actual = JsonSerializer.Deserialize<PaymentCancellationFailed>(Json);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Deserialize_payment_cancellation_failed_using_custom_converter()
    {
        // Arrange
        var options = new JsonSerializerOptions(JsonSerializerOptions.Default);
        options.Converters.Add(new IWebhookConverter());
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(Json));

        // Act
        var actual = JsonSerializer.Deserialize<IWebhook<WebhookData>>(ref reader, options);
        var paymentCancellationFailed = actual as PaymentCancellationFailed;

        // Assert
        paymentCancellationFailed.Should().NotBeNull().And.BeEquivalentTo(expected);
    }
}
