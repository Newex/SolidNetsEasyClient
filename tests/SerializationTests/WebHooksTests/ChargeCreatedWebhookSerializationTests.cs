using System;
using System.Collections.Generic;
using System.Diagnostics;
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
public class ChargeCreatedWebhookSerializationTests
{
    const string Json = """
    {
        "id": "01ee00006091b2196937598058c4e488",
        "timestamp": "2021-05-04T22:44:10.1185+02:00",
        "merchantNumber": 100017120,
        "event": "payment.charge.created.v2",
        "data": {
            "chargeId": "01ee00006091b2196937598058c4e488",
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
            "paymentMethod": "Visa",
            "paymentType": "PREPAID-INVOICE",
            "amount": {
                "amount": 5500,
                "currency": "SEK"
            },
            "paymentId": "025400006091b1ef6937598058c4e487"
        }
    }
    """;

    private readonly ChargeCreated chargeCreated = new()
    {
        Id = new("01ee00006091b2196937598058c4e488"),
        Timestamp = DateTimeOffset.Parse("2021-05-04T22:44:10.1185+02:00", CultureInfo.InvariantCulture),
        MerchantNumber = 100017120,
        Event = EventName.ChargeCreated,
        Data = new()
        {
            ChargeId = new("01ee00006091b2196937598058c4e488"),
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

    [Fact]
    public void Can_deserialize_expected_string_to_ChargedCreated_object()
    {
        // Arrange
        // Example from: https://developers.nets.eu/nets-easy/en-EU/api/webhooks/#charge-created

        Debug.WriteLine(Json);


        // Act
        var actual = JsonSerializer.Deserialize<ChargeCreated>(Json);

        // Assert
        actual.Should().BeEquivalentTo(chargeCreated);
    }

    [Fact]
    public void Can_deserialize_actual_response_to_ChargeCreated_object()
    {
        // Arrange
        const string response = CleanedResponses.ChargeCreated;
        var expected = new ChargeCreated
        {
            Id = new("006b0000636f4149e30174516bf6aa5a"),
            Timestamp = DateTimeOffset.Parse("2022-11-12T07:46:33.7120+01:00", CultureInfo.InvariantCulture),
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

    [Fact]
    public void Deserialize_payment_response_using_custom_converter()
    {
        // Arrange
        var options = new JsonSerializerOptions(JsonSerializerOptions.Default);
        options.Converters.Add(new IWebhookConverter());
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(Json));

        // Act
        var actual = JsonSerializer.Deserialize<IWebhook<WebhookData>>(ref reader, options);
        var chargeCreated = actual as ChargeCreated;

        // Assert
        chargeCreated.Should().NotBeNull().And.BeEquivalentTo(chargeCreated);
    }
}
