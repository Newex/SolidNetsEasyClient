using System;
using System.Text.Json;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

namespace SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests;

[UnitTest]
public class RefundFailedSerializationTests
{
    [Fact]
    public void Can_deserialize_refund_failed_json_to_RefundFailed_object()
    {
        // Arrange
        const string json = "{\n" +
        "\"id\": \"458a4e068f454f768a40b9e576914820\",\n" +
        "\"merchantId\": 100017120,\n" +
        "\"timestamp\": \"2021-05-04T22:08:16.6623+02:00\",\n" +
        "\"event\": \"payment.refund.failed\",\n" +
        "\"data\": {\n" +
            "\"error\": {\n" +
                "\"code\": \"25\",\n" +
                "\"message\": \"Some error message\",\n" +
                "\"source\": \"Internal\"\n" +
            "},\n" +
            "\"refundId\": \"00fb000060923e006937598058c4e7f3\",\n" +
            "\"amount\": {\n" +
                "\"amount\": 5500,\n" +
                "\"currency\": \"SEK\"\n" +
            "},\n" +
            "\"paymentId\": \"012b000060923cf26937598058c4e7e6\"\n" +
            "}\n" +
        "}\n";

        var expected = new RefundFailed
        {
            Id = new("458a4e068f454f768a40b9e576914820"),
            MerchantId = 100017120,
            Timestamp = DateTimeOffset.Parse("2021-05-04T22:08:16.6623+02:00"),
            Event = EventName.RefundFailed,
            Data = new()
            {
                Error = new()
                {
                    Code = "25",
                    Message = "Some error message",
                    Source = "Internal"
                },
                RefundId = new("00fb000060923e006937598058c4e7f3"),
                Amount = new()
                {
                    Amount = 5500,
                    Currency = Currency.SEK
                },
                PaymentId = new("012b000060923cf26937598058c4e7e6")
            }
        };

        // Act
        var actual = JsonSerializer.Deserialize<RefundFailed>(json);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
