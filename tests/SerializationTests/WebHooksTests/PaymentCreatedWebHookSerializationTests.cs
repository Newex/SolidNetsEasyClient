using System;
using System.Globalization;
using System.Text;
using System.Text.Json;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Converters.WebhookPayloadConverters;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;
using SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests.ActualResponses;

namespace SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests;

[UnitTest]
public class PaymentCreatedWebHookSerializationTests
{
    const string PaymentCreatedJson = """
    {
      "id": "458a4e068f454f768a40b9e576914820",
      "merchantId": 100017120,
      "timestamp": "2021-05-04T22:08:16.6623+02:00",
      "event": "payment.created",
      "data": {
          "order": {
              "amount": {
                  "amount": 5500,
                  "currency": "SEK"
              },
              "reference": "42369",
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
              ]
          },
          "paymentId": "02a900006091a9a96937598058c4e474"
      }
    }
    """;

    private static readonly PaymentCreated PaymentCreatedExpected = new()
    {
        Id = new Guid("458a4e068f454f768a40b9e576914820"),
        MerchantId = 100017120,
        Timestamp = DateTimeOffset.Parse("2021-05-04T22:08:16.6623+02:00", CultureInfo.InvariantCulture),
        Event = EventName.PaymentCreated,
        Data = new()
        {
            Order = new()
            {
                Amount = new()
                {
                    Amount = 5500,
                    Currency = Currency.SEK
                },
                Reference = "42369",
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
            },
            PaymentId = new Guid("02a900006091a9a96937598058c4e474")
        }
    };

    [Fact]
    public void Can_deserialize_expected_string_to_PaymentCreated_object()
    {
        // Arrange

        // Act
        var actual = JsonSerializer.Deserialize<PaymentCreated>(PaymentCreatedJson);

        // Assert
        actual.Should().BeEquivalentTo(PaymentCreatedExpected);
    }

    [Fact]
    public void Can_deserialize_actual_response_to_PaymentCreated_object()
    {
        // Arrange
        const string response = CleanedResponses.PaymentCreated;
        var expected = new PaymentCreated
        {
            Id = new("484107b70c134d7d8b582354e05cd1f9"),
            MerchantId = 123456,
            Timestamp = DateTimeOffset.Parse("2022-11-12T06:33:21.2310+00:00", CultureInfo.InvariantCulture),
            Event = EventName.PaymentCreated,
            Data = new()
            {
                Order = new()
                {
                    Amount = new()
                    {
                        Amount = 40_00,
                        Currency = Currency.DKK
                    },
                    Reference = "282f89f2-d620-4cc0-91bb-9cce1897f0bc",
                    OrderItems =
                    [
                        new()
                        {
                            Name ="Nuka-Cola",
                            Quantity = 1,
                            Reference = "f32be43c-19f8-4546-bb8b-5fcd273d19a1",
                            TaxRate = 0,
                            Unit = "ea",
                            UnitPrice = 40_00,
                        }
                    ],
                },
                PaymentId = new("023e0000636f3df7e30174516bf6aa48")
            }
        };

        // Act
        var actual = JsonSerializer.Deserialize<PaymentCreated>(response);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Deserialize_payment_response_using_custom_converter()
    {
        // Arrange
        var options = new JsonSerializerOptions(JsonSerializerOptions.Default);
        options.Converters.Add(new OrderItemsConverter());
        options.Converters.Add(new WebhookOrderConverter());
        options.Converters.Add(new PaymentCreatedDataConverter());
        options.Converters.Add(new IWebhookConverter());
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(PaymentCreatedJson));

        // Act
        var actual = JsonSerializer.Deserialize<IWebhook<WebhookData>>(ref reader, options);
        var paymentCreated = actual as PaymentCreated;

        // Assert
        paymentCreated.Should().NotBeNull().And.BeEquivalentTo(PaymentCreatedExpected);
    }
}
