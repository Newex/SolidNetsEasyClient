using System.Text.Json;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Tests.SerializationTests;

[UnitTest, Feature("SerializationTests")]
public class PaymentMethodTypeSerializationTests
{
    [Fact]
    public void Serialize_payment_method_to_json_string()
    {
        // Arrange
        const PaymentMethodEnum dankort = PaymentMethodEnum.Dankort;

        // Act
        var actual = JsonSerializer.Serialize(dankort);
        const string expected = "\"Dankort\"";

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Deserialize_payment_method_to_CLR_type()
    {
        // Arrange
        const string jsonDankort = "\"Dankort\"";

        // Act
        var actual = JsonSerializer.Deserialize<PaymentMethodEnum>(jsonDankort);
        const PaymentMethodEnum expected = PaymentMethodEnum.Dankort;

        // Assert
        Assert.Equal(expected, actual);
    }
}
