using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using SolidNetsEasyClient.Models.DTOs;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests.ActualResponses;

namespace SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests;

[UnitTest]
public class PaymentCancelledSerializationTests
{
    [Fact]
    public void Can_deserialize_example_payment_cancelled_event_to_PaymentCancelled_object()
    {
        // Arrange
        const string json = "{\n" +
        "\"id\": \"df7f9346097842bdb90c869b5c9ccfa9\",\n" +
        "\"merchantId\": 100017120,\n" +
        "\"timestamp\": \"2021-05-04T22:33:33.5969+02:00\",\n" +
        "\"event\": \"payment.cancel.created\",\n" +
        "\"data\": {\n" +
                "\"cancelId\": \"df7f9346097842bdb90c869b5c9ccfa9\",\n" +
                "\"orderItems\": [\n" +
                    "{\n" +
                        "\"reference\": \"Sneaky NE2816-82\",\n" +
                        "\"name\": \"Sneaky\",\n" +
                        "\"quantity\": 2,\n" +
                        "\"unit\": \"pcs\",\n" +
                        "\"unitPrice\": 2500,\n" +
                        "\"taxRate\": 1000,\n" +
                        "\"taxAmount\": 500,\n" +
                        "\"netTotalAmount\": 5000,\n" +
                        "\"grossTotalAmount\": 5500\n" +
                    "}\n" +
                "],\n" +
                "\"amount\": {\n" +
                    "\"amount\": 5500,\n" +
                    "\"currency\": \"SEK\"\n" +
                "},\n" +
                "\"paymentId\": \"006400006091abfe6937598058c4e47e\"\n" +
            "}\n" +
        "}\n";
        var expected = new PaymentCancelled
        {
            Id = new("df7f9346097842bdb90c869b5c9ccfa9"),
            MerchantId = 100017120,
            Timestamp = DateTimeOffset.Parse("2021-05-04T22:33:33.5969+02:00"),
            Event = EventName.ReservationCancelled,
            Data = new()
            {
                CancelId = new("df7f9346097842bdb90c869b5c9ccfa9"),
                OrderItems = new List<Item>
                {
                    new()
                    {
                        Reference = "Sneaky NE2816-82",
                        Name = "Sneaky",
                        Quantity = 2,
                        Unit = "pcs",
                        UnitPrice = 25_00,
                        TaxRate = 1000,
                    }
                },
                Amount = new()
                {
                    Amount = 5500,
                    Currency = "SEK"
                },
                PaymentId = new("006400006091abfe6937598058c4e47e")
            },
        };

        // Act
        var actual = JsonSerializer.Deserialize<PaymentCancelled>(json);

        // Assert
        actual.Should().BeEquivalentTo(expected);
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
            Timestamp = DateTimeOffset.Parse("2022-11-12T21:15:14.9378+01:00"),
            Event = EventName.ReservationCancelled,
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
                    Currency = "DKK"
                },
                PaymentId = new("00220000636ffe1c530a07afca5d2b1e")
            }
        };

        // Act
        var actual = JsonSerializer.Deserialize<PaymentCancelled>(response);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
