using System;
using System.Collections.Generic;
using System.Text.Json;
using SolidNetsEasyClient.Models.DTOs;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

namespace SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests;

[UnitTest]
public class ReservationFailedSerializationTests
{
    [Fact]
    public void Can_deserialize_example_reservation_failed_string_to_ReservationFailed_object()
    {
        // Arrange
        const string json = "{\n" +
        "\"id\": \"ef0f698086ac4e7493439ab4290695da\",\n" +
        "\"merchantId\": 100008172,\n" +
        "\"timestamp\": \"2022-06-30T10:54:07.7765+02:00\",\n" +
        "\"event\": \"payment.reservation.failed\",\n" +
        "\"data\": {\n" +
            "\"error\": {\n" +
            "\"code\": \"33\",\n" +
            "\"message\": \"Direct charge failed for payment id: 020b000062bd64ae0a5e7c95f6055f66. ErrorMessage: Refused by issuer\",\n" +
            "\"source\": \"Issuer\"\n" +
            "},\n" +
            "\"orderItems\": [\n" +
            "{\n" +
                "\"grossTotalAmount\": 133,\n" +
                "\"name\": \"NameBulkCharge1\",\n" +
                "\"netTotalAmount\": 133,\n" +
                "\"quantity\": 1,\n" +
                "\"reference\": \"bulk123\",\n" +
                "\"taxRate\": 0,\n" +
                "\"taxAmount\": 0,\n" +
                "\"unit\": \"quantity\",\n" +
                "\"unitPrice\": 133\n" +
            "}\n" +
            "],\n" +
            "\"amount\": {\n" +
            "\"amount\": 133,\n" +
            "\"currency\": \"DKK\"\n" +
            "},\n" +
            "\"paymentId\": \"020b000062bd64ae0a5e7c95f6055f66\"\n" +
        "}\n" +
        "}\n";

        var expected = new ReservationFailed()
        {
            Id = new("ef0f698086ac4e7493439ab4290695da"),
            MerchantId = 100008172,
            Timestamp = DateTimeOffset.Parse("2022-06-30T10:54:07.7765+02:00"),
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

        // Act
        var actual = JsonSerializer.Deserialize<ReservationFailed>(json);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
