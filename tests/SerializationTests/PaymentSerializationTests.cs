using System;
using System.Text.Json;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;
using SolidNetsEasyClient.SerializationContexts;

namespace SolidNetsEasyClient.Tests.SerializationTests;

[UnitTest, Feature("SerializationTests")]
public class PaymentSerializationTests
{
    [Fact]
    public void Deserialize_example_item()
    {
        // Arrange
        const string itemJson = /*lang=json,strict*/ @"
        {
            ""reference"": ""MyReference"",
            ""name"": ""Name Of My Product"",
            ""quantity"": 1,
            ""unit"": ""pcs"",
            ""unitPrice"": 800,
            ""grossTotalAmount"": 800,
            ""netTotalAmount"": 800
        }
        ";

        // Act
        var item = JsonSerializer.Deserialize<Item>(itemJson);

        // Assert
        Assert.Equal(800, item!.UnitPrice);
    }

    [Fact]
    public void Deserialize_example_order()
    {
        // Arrange
        const string orderJson = /*lang=json,strict*/ @"
        {
            ""items"": [
                {
                    ""reference"": ""MyReference"",
                    ""name"": ""Name Of My Product"",
                    ""quantity"": 1,
                    ""unit"": ""pcs"",
                    ""unitPrice"": 800,
                    ""grossTotalAmount"": 800,
                    ""netTotalAmount"": 800
                }
            ],
            ""amount"": 800,
            ""currency"": ""DKK"",
            ""reference"": ""my order reference""
        }
        ";

        // Act
        var order = JsonSerializer.Deserialize<Order>(orderJson);

        // Assert
        Assert.Equal(800, order!.Amount);
    }

    [Fact]
    public void PaymentResult_deserialization()
    {
        // Arrange
        var responseJson = """
        {
            "paymentId": "458a4e068f454f768a40b9e576914820"
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize(responseJson, PaymentResultSerializationContext.Default.PaymentResult);

        // Assert
        result.Should().NotBeNull().And.BeEquivalentTo(new PaymentResult
        {
            PaymentId = new Guid("458a4e068f454f768a40b9e576914820")
        });
    }

    [Fact]
    public void Integration_actual_payment_info_response_deserialization()
    {
        // Arrange
        var json = """
        {
            "payment": {
                "paymentId": "51511a8f45d74441a5ab82970cefb52b",
                "summary": {},
                "consumer": {
                    "shippingAddress": {
                        "phoneNumber": {}
                    },
                    "company": {
                        "contactDetails": {
                            "phoneNumber": {}
                        }
                    },
                    "privatePerson": {
                        "phoneNumber": {}
                    },
                    "billingAddress": {
                        "phoneNumber": {}
                    }
                },
                "paymentDetails": {
                    "invoiceDetails": {},
                    "cardDetails": {}
                },
                "orderDetails": {
                    "amount": 2500,
                    "currency": "DKK",
                    "reference": "my-order-id-reference"
                },
                "checkout": {
                    "url": "https://localhost:5110/checkout",
                    "cancelUrl": ""
                },
                "created": "2024-05-10T20:58:46.2859+00:00",
                "myReference": "my-payment-reference"
            }
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize(json, PaymentSerializationContext.Default.PaymentStatus);

        // Assert
        result.Should().NotBeNull().And.BeEquivalentTo(new PaymentStatus
        {
            Payment = new()
            {
                PaymentId = new Guid("51511a8f45d74441a5ab82970cefb52b"),
                OrderDetails = new()
                {
                    Amount = 2500,
                    Currency = Currency.DKK,
                    Reference = "my-order-id-reference"
                },
                Checkout = new()
                {
                    Url = "https://localhost:5110/checkout",
                    CancelUrl = string.Empty
                },
                Created = DateTimeOffset.Parse("2024-05-10T20:58:46.2859+00:00"),
                MyReference = "my-payment-reference",
                Consumer = new()
                {
                    Company = new()
                    {
                        ContactDetails = new()
                        {
                            PhoneNumber = new()
                        }
                    },
                    ShippingAddress = new()
                    {
                        PhoneNumber = new()
                    },
                    PrivatePerson = new()
                    {
                        PhoneNumber = new()
                    },
                    BillingAddress = new()
                    {
                        PhoneNumber = new()
                    }
                },
                Summary = new(),
                PaymentDetails = new()
                {
                    InvoiceDetails = new(),
                    CardDetails = new()
                }
            }
        });
    }
}
