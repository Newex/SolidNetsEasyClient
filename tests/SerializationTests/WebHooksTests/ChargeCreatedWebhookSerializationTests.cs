using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using SolidNetsEasyClient.Models;
using SolidNetsEasyClient.Models.WebHooks;

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
            Event = EventNames.Payment.ChargeCreated,
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
                    Currency = "SEK",
                },
                PaymentId = new("025400006091b1ef6937598058c4e487")
            },
        };

        // Act
        var actual = JsonSerializer.Deserialize<ChargeCreated>(json);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
