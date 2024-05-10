using System;
using System.Text.Json;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs;
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
            "paymentId": "458a4e068f454f768a40b9e576914820",
        }
        """;
        var options = new JsonSerializerOptions();
        options.Converters.Add(new GuidTypeConverter());

        // Act
        var result = JsonSerializer.Deserialize(responseJson, PaymentResultSerializationContext.Default.PaymentResult);

        // Assert
        result.Should().NotBeNull().And.BeEquivalentTo(new PaymentResult
        {
            PaymentId = new Guid("458a4e068f454f768a40b9e576914820")
        });
    }
}
