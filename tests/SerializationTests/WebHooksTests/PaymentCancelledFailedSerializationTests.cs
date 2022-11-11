using System;
using System.Collections.Generic;
using System.Text.Json;
using SolidNetsEasyClient.Models.DTOs;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

namespace SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests;

[UnitTest]
public class PaymentCancelledFailedSerializationTests
{
    [Fact]
    public void Can_deserialize_payment_cancelled_failed_event_to_PaymentCancelledFailed_object()
    {
        // Arrange
        const string json = "{\n" +
            "\"id\": \"df7f9346097842bdb90c869b5c9ccfa9\",\n" +
            "\"merchantId\": 100017120,\n" +
            "\"timestamp\": \"2021-05-06T11:37:30.1114+02:00\",\n" +
            "\"event\": \"payment.cancel.failed\",\n" +
            "\"data\": {\n" +
                "\"error\": {\n" +
                    "\"code\": \"25\",\n" +
                    "\"message\": \"Trans not found\",\n" +
                    "\"source\": \"Internal\"\n" +
                "},\n" +
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
                "\"paymentId\": \"023a00005ea744ed368812223c86c299\"\n" +
            "}\n" +
        "}\n";

        var expected = new PaymentCancelledFailed()
        {
            Id = new("df7f9346097842bdb90c869b5c9ccfa9"),
            MerchantId = 100017120,
            Timestamp = DateTimeOffset.Parse("2021-05-06T11:37:30.1114+02:00"),
            Event = EventName.ReservationCancellationFailed,
            Data = new()
            {
                Error = new()
                {
                    Code = "25",
                    Message = "Trans not found",
                    Source = "Internal"
                },
                CancelId = new("df7f9346097842bdb90c869b5c9ccfa9"),
                OrderItems = new List<Item>
                {
                    new()
                    {
                        Reference = "Sneaky NE2816-82",
                        Name = "Sneaky",
                        Quantity = 2,
                        Unit = "pcs",
                        UnitPrice = 2500,
                        TaxRate = 1000,
                    }
                },
                Amount = new()
                {
                    Amount = 5500,
                    Currency = "SEK",
                },
                PaymentId = new("023a00005ea744ed368812223c86c299")
            }
        };

        // Act
        var actual = JsonSerializer.Deserialize<PaymentCancelledFailed>(json);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
