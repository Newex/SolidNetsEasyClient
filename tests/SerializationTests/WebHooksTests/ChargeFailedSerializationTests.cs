using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using SolidNetsEasyClient.Models.DTOs;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

namespace SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests;

[UnitTest]
public class ChargeFailedSerializationTests
{
    [Fact]
    public void Can_deserialize_expected_charge_failed_json_to_ChargeFailed_object()
    {
        // Arrange
        const string json = "" +
        "{\n" +
            "\"id\": \"02a8000060923bcb6937598058c4e77a\",\n" +
            "\"merchantId\": 100017120,\n" +
            "\"timestamp\": \"2021-05-05T08:31:39.2481+02:00\",\n" +
            "\"event\": \"payment.charge.failed\",\n" +
            "\"data\": {\n" +
                "\"error\": {\n" +
                    "\"code\": \"99\",\n" +
                    "\"message\": \"Auth Fin Failure\",\n" +
                    "\"source\": \"Internal\"\n" +
                "},\n" +
                "\"chargeId\": \"02a8000060923bcb6937598058c4e77a\",\n" +
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
                "\"reservationId\": \"0527cb1dc5d14491824644a84d5ccf69\",\n" +
                "\"amount\": {\n" +
                    "\"amount\": 5500,\n" +
                    "\"currency\": \"SEK\"\n" +
                "},\n" +
                "\"paymentId\": \"029b000060923a766937598058c4e6fa\"\n" +
            "}\n" +
        "}\n";
        Debug.WriteLine(json);

        var expected = new ChargeFailed
        {
            Id = new("02a8000060923bcb6937598058c4e77a"),
            MerchantId = 100017120,
            Timestamp = DateTimeOffset.Parse("2021-05-05T08:31:39.2481+02:00", CultureInfo.InvariantCulture),
            Event = EventName.ChargeFailed,
            Data = new()
            {
                Error = new()
                {
                    Code = "99",
                    Message = "Auth Fin Failure",
                    Source = "Internal"
                },
                ChargeId = new("02a8000060923bcb6937598058c4e77a"),
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
                ReservationId = new("0527cb1dc5d14491824644a84d5ccf69"),
                Amount = new()
                {
                    Amount = 5500,
                    Currency = Currency.SEK
                },
                PaymentId = new("029b000060923a766937598058c4e6fa")
            }
        };

        // Act
        var actual = JsonSerializer.Deserialize<ChargeFailed>(json);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
