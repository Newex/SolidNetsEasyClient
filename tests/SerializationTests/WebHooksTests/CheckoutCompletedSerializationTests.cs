using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using SolidNetsEasyClient.Models.DTOs;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

namespace SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests;

[UnitTest]
public class CheckoutCompletedSerializationTests
{
    [Fact]
    public void Can_deserialize_example_checkout_completed_event_to_CheckoutCompleted_object()
    {
        // Arrange
        const string json = "{\n" +
            "\"id\": \"36ce3ff4a896450ea2b70f3263554772\",\n" +
            "\"merchantId\": 100017120,\n" +
            "\"timestamp\": \"2021-05-04T22:09:08.4342+02:00\",\n" +
            "\"event\": \"payment.checkout.completed\",\n" +
            "\"data\": {\n" +
                "\"order\": {\n" +
                    "\"amount\": {\n" +
                        "\"amount\": 5500,\n" +
                        "\"currency\": \"SEK\"\n" +
                    "},\n" +
                    "\"reference\": \"Hosted Demo Order\",\n" +
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
                    "]\n" +
                "},\n" +
                "\"consumer\": {\n" +
                    "\"firstName\": \"John\",\n" +
                    "\"lastName\": \"Doe\",\n" +
                    "\"billingAddress\": {\n" +
                        "\"addressLine1\": \"Solgatan 4\",\n" +
                        "\"addressLine2\": \"\",\n" +
                        "\"city\": \"STOCKHOLM\",\n" +
                        "\"country\": \"SWE\",\n" +
                        "\"postcode\": \"11522\",\n" +
                        "\"receiverLine\": \"John doe\"\n" +
                    "},\n" +
                    "\"country\": \"SWE\",\n" +
                    "\"email\": \"john.doe@example.com\",\n" +
                    "\"ip\": \"192.230.114.3\",\n" +
                    "\"phoneNumber\": {\n" +
                        "\"prefix\": \"+46\",\n" +
                        "\"number\": \"12345678\"\n" +
                    "},\n" +
                    "\"shippingAddress\": {\n" +
                        "\"addressLine1\": \"Solgatan 4\",\n" +
                        "\"addressLine2\": \"\",\n" +
                        "\"city\": \"STOCKHOLM\",\n" +
                        "\"country\": \"SWE\",\n" +
                        "\"postcode\": \"11522\",\n" +
                        "\"receiverLine\": \"John Doe\"\n" +
                    "}\n" +
                "},\n" +
                "\"paymentId\": \"02a900006091a9a96937598058c4e474\"\n" +
            "}\n" +
        "}\n";
        var expected = new CheckoutCompleted()
        {
            Id = new("36ce3ff4a896450ea2b70f3263554772"),
            MerchantId = 100017120,
            Timestamp = DateTimeOffset.Parse("2021-05-04T22:09:08.4342+02:00"),
            Event = EventName.CheckoutCompleted,
            Data = new()
            {
                Order = new()
                {
                    Amount = new()
                    {
                        Amount = 55_00,
                        Currency = "SEK",
                    },
                    Reference = "Hosted Demo Order",
                    OrderItems = new List<Item>()
                    {
                        new()
                        {
                            Reference = "Sneaky NE2816-82",
                            Name = "Sneaky",
                            Quantity = 2,
                            Unit = "pcs",
                            UnitPrice = 25_00,
                            TaxRate = 10_00,
                        }
                    },
                },
                Consumer = new()
                {
                    FirstName = "John",
                    LastName = "Doe",
                    BillingAddress = new()
                    {
                        AddressLine1 = "Solgatan 4",
                        AddressLine2 = "",
                        City = "STOCKHOLM",
                        Country = "SWE",
                        PostCode = "11522",
                        ReceiverLine = "John doe"
                    },
                    Country = "SWE",
                    Email = "john.doe@example.com",
                    IP = "192.230.114.3",
                    PhoneNumber = new()
                    {
                        Prefix = "+46",
                        Number = "12345678",
                    },
                    ShippingAddress = new()
                    {
                        AddressLine1 = "Solgatan 4",
                        AddressLine2 = "",
                        City = "STOCKHOLM",
                        Country = "SWE",
                        PostCode = "11522",
                        ReceiverLine = "John Doe"
                    }
                },
                PaymentId = new("02a900006091a9a96937598058c4e474")
            }
        };

        // Act
        var actual = JsonSerializer.Deserialize<CheckoutCompleted>(json);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
