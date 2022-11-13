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
public class ChargeCreatedWebhookSerializationTests
{
    [Fact]
    public void Can_deserialize_expected_string_to_ChargedCreated_object()
    {
        // Arrange
        // Example from: https://developers.nets.eu/nets-easy/en-EU/api/webhooks/#charge-created
        const string json = "{\n" +
            "\"id\": \"01ee00006091b2196937598058c4e488\",\n" +
            "\"timestamp\": \"2021-05-04T22:44:10.1185+02:00\",\n" +
            "\"merchantNumber\": 100017120,\n" +
            "\"event\": \"payment.charge.created.v2\",\n" +
            "\"data\": {\n" +
                "\"chargeId\": \"01ee00006091b2196937598058c4e488\",\n" +
                "\"orderItems\": [\n" +
                    "{\n" +
                        "\t\t\t\"reference\": \"Sneaky NE2816-82\",\n" +
                        "\t\t\t\"name\": \"Sneaky\",\n" +
                        "\t\t\t\"quantity\": 2,\n" +
                        "\t\t\t\"unit\": \"pcs\",\n" +
                        "\t\t\t\"unitPrice\": 2500,\n" +
                        "\t\t\t\"taxRate\": 1000,\n" +
                        "\t\t\t\"taxAmount\": 500,\n" +
                        "\t\t\t\"netTotalAmount\": 5000,\n" +
                        "\t\t\t\"grossTotalAmount\": 5500\n" +
                    "}\n" +
                "],\n" +
                "\"paymentMethod\": \"Visa\",\n" +
                "\"paymentType\": \"PREPAID-INVOICE\",\n" +
                "\"amount\": {\n" +
                    "\"amount\": 5500,\n" +
                    "\"currency\": \"SEK\"\n" +
                "},\n" +
                "\"paymentId\": \"025400006091b1ef6937598058c4e487\"\n" +
            "}\n" +
        "}\n";

        Debug.WriteLine(json);

        var expected = new ChargeCreated
        {
            Id = new("01ee00006091b2196937598058c4e488"),
            Timestamp = DateTimeOffset.Parse("2021-05-04T22:44:10.1185+02:00"),
            MerchantNumber = 100017120,
            Event = EventName.ChargeCreated,
            Data = new()
            {
                ChargeId = new("01ee00006091b2196937598058c4e488"),
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
                PaymentMethod = PaymentMethodEnum.Visa,
                PaymentType = PaymentTypeEnum.PrepaidInvoice,
                Amount = new()
                {
                    Amount = 5500,
                    Currency = Currency.SEK
                },
                PaymentId = new("025400006091b1ef6937598058c4e487")
            },
        };

        // Act
        var actual = JsonSerializer.Deserialize<ChargeCreated>(json);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Can_deserialize_actual_response_to_ChargeCreated_object()
    {
        // Arrange
        const string response = CleanedResponses.ChargeCreated;
        var expected = new ChargeCreated
        {
            Id = new("006b0000636f4149e30174516bf6aa5a"),
            Timestamp = DateTimeOffset.Parse("2022-11-12T07:46:33.7120+01:00"),
            MerchantNumber = 123456,
            Event = EventName.ChargeCreated,
            Data = new()
            {
                ChargeId = new("006b0000636f4149e30174516bf6aa5a"),
                OrderItems = new List<Item>
                {
                    new()
                    {
                        Name = "Nuka-Cola",
                        Quantity = 1,
                        Reference = "f32be43c-19f8-4546-bb8b-5fcd273d19a1",
                        Unit = "ea",
                        UnitPrice = 40_00,
                        TaxRate = 0
                    }
                },
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
        var actual = JsonSerializer.Deserialize<ChargeCreated>(response);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
