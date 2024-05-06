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
using SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests.ActualResponses;

namespace SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests;

[UnitTest]
public class PaymentCancelledSerializationTests
{
    const string Json = """
    {
      "id": "df7f9346097842bdb90c869b5c9ccfa9",
      "merchantId": 100017120,
      "timestamp": "2021-05-04T22:33:33.5969+02:00",
      "event": "payment.cancel.created",
      "data": {
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
          "paymentId": "006400006091abfe6937598058c4e47e"
        }
    }
    """;
    private readonly PaymentCancelled paymentCancelled = new()
    {
        Id = new("df7f9346097842bdb90c869b5c9ccfa9"),
        MerchantId = 100017120,
        Timestamp = DateTimeOffset.Parse("2021-05-04T22:33:33.5969+02:00", CultureInfo.InvariantCulture),
        Event = EventName.PaymentCancelled,
        Data = new()
        {
            CancelId = new("df7f9346097842bdb90c869b5c9ccfa9"),
            OrderItems =
                [
                    new()
                    {
                        Reference = "Sneaky NE2816-82",
                        Name = "Sneaky",
                        Quantity = 2,
                        Unit = "pcs",
                        UnitPrice = 25_00,
                        TaxRate = 1000,
                    }
                ],
            Amount = new()
            {
                Amount = 5500,
                Currency = Currency.SEK
            },
            PaymentId = new("006400006091abfe6937598058c4e47e")
        },
    };

    [Fact]
    public void Can_deserialize_example_payment_cancelled_event_to_PaymentCancelled_object()
    {
        // Arrange

        // Act
        var actual = JsonSerializer.Deserialize<PaymentCancelled>(Json);

        // Assert
        actual.Should().BeEquivalentTo(paymentCancelled);
    }

    [Fact]
    public void Can_deserialize_actual_reservation_cancellation_to_PaymentCancelled_object()
    {
        // Arrange
        const string response = CleanedResponses.ReservationCancelled;
        var expected = new PaymentCancelled
        {
            Id = new("78edb53c694944dc83c72277d12181e0"),
            MerchantId = 123456,
            Timestamp = DateTimeOffset.Parse("2022-11-12T21:15:14.9378+01:00", CultureInfo.InvariantCulture),
            Event = EventName.PaymentCancelled,
            Data = new()
            {
                CancelId = new("78edb53c694944dc83c72277d12181e0"),
                OrderItems = new List<Item>()
                {
                    new()
                    {
                        Name = "Nuka-Cola",
                        Quantity = 1,
                        Reference = "f32be43c-19f8-4546-bb8b-5fcd273d19a1",
                        TaxRate = 0,
                        Unit = "ea",
                        UnitPrice = 40_00
                    }
                },
                Amount = new()
                {
                    Amount = 40_00,
                    Currency = Currency.DKK
                },
                PaymentId = new("00220000636ffe1c530a07afca5d2b1e")
            }
        };

        // Act
        var actual = JsonSerializer.Deserialize<PaymentCancelled>(response);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Deserialize_payment_cancelled_response_webhook()
    {
        // Arrange
        var options = new JsonSerializerOptions(JsonSerializerOptions.Default);
        options.Converters.Add(new IWebhookConverter());
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(Json));

        // Act
        var actual = JsonSerializer.Deserialize<IWebhook<WebhookData>>(ref reader, options);
        var paymentCancelled = actual as PaymentCancelled;

        // Assert
        paymentCancelled.Should().NotBeNull().And.BeEquivalentTo(paymentCancelled);
    }
}
