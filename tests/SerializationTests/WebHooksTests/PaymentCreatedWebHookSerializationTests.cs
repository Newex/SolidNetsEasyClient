using System;
using System.Collections.Generic;
using System.Text.Json;
using SolidNetsEasyClient.Models;
using SolidNetsEasyClient.Models.WebHooks;

namespace SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests;

[UnitTest]
public class PaymentCreatedWebHookSerializationTests
{
    [Fact]
    public void Can_deserialize_expected_string_to_PaymentCreated_object()
    {
        // Arrange
        const string json = "\n" +
            "{\n" +
                "\"id\": \"458a4e068f454f768a40b9e576914820\",\n" +
                "\"merchantId\": 100017120,\n" +
                "\"timestamp\": \"2021-05-04T22:08:16.6623+02:00\",\n" +
                "\"event\": \"payment.created\",\n" +
                "\"data\": {\n" +
                "\t\"order\": {\n" +
                "\t\t\"amount\": {\n" +
                    "\t\t\t\"amount\": 5500,\n" +
                    "\t\t\t\"currency\": \"SEK\"\n" +
                "\t\t},\n" +
                "\t\t\"reference\": \"42369\",\n" +
                "\t\t\"orderItems\": [\n" +
                "\t\t{\n" +
                    "\t\t\t\"reference\": \"Sneaky NE2816-82\",\n" +
                    "\t\t\t\"name\": \"Sneaky\",\n" +
                    "\t\t\t\"quantity\": 2,\n" +
                    "\t\t\t\"unit\": \"pcs\",\n" +
                    "\t\t\t\"unitPrice\": 2500,\n" +
                    "\t\t\t\"taxRate\": 1000,\n" +
                    "\t\t\t\"taxAmount\": 500,\n" +
                    "\t\t\t\"netTotalAmount\": 5000,\n" +
                    "\t\t\t\"grossTotalAmount\": 5500\n" +
                "\t\t}\n" +
                "\t\t]\n" +
                "\t},\n" +
                "\t\"paymentId\": \"02a900006091a9a96937598058c4e474\"\n" +
                "}\n" +
            "}\n" +
        "";

        // Act
        var actual = JsonSerializer.Deserialize<PaymentCreated>(json);

        // Assert
        var expected = new PaymentCreated
        {
            Id = new Guid("458a4e068f454f768a40b9e576914820"),
            MerchantId = 100017120,
            Timestamp = DateTimeOffset.Parse("2021-05-04T22:08:16.6623+02:00"),
            Event = EventNames.Payment.PaymentCreated,
            Data = new()
            {
                Order = new()
                {
                    Amount = new()
                    {
                        Amount = 5500,
                        Currency = "SEK",
                    },
                    Reference = "42369",
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
                },
                PaymentId = new Guid("02a900006091a9a96937598058c4e474")
            }
        };

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
