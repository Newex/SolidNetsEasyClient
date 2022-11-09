using System.Text.Json;
using SolidNetsEasyClient.Models.DTOs;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;

namespace SolidNetsEasyClient.Tests.SerializationTests;

[UnitTest, Feature("SerializationTests")]
public class PaymentSerializationTests
{
    [Fact]
    public void Deserialize_example_item()
    {
        // Arrange
        const string itemJson = @"
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
        const string orderJson = @"
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
}
