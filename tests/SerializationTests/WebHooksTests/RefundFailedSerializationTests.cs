using System;
using System.Globalization;
using System.Text;
using System.Text.Json;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests;

[UnitTest]
public class RefundFailedSerializationTests
{
    const string Json = """
    {
    "id": "458a4e068f454f768a40b9e576914820",
    "merchantId": 100017120,
    "timestamp": "2021-05-04T22:08:16.6623+02:00",
    "event": "payment.refund.failed",
    "data": {
        "error": {
            "code": "25",
            "message": "Some error message",
            "source": "Internal"
        },
        "refundId": "00fb000060923e006937598058c4e7f3",
        "amount": {
            "amount": 5500,
            "currency": "SEK"
        },
        "paymentId": "012b000060923cf26937598058c4e7e6"
        }
    }
    """;

    private readonly RefundFailed expected = new()
    {
        Id = new("458a4e068f454f768a40b9e576914820"),
        MerchantId = 100017120,
        Timestamp = DateTimeOffset.Parse("2021-05-04T22:08:16.6623+02:00", CultureInfo.InvariantCulture),
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

    [Fact]
    public void Can_deserialize_refund_failed_json_to_RefundFailed_object()
    {
        // Arrange

        // Act
        var actual = JsonSerializer.Deserialize<RefundFailed>(Json);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Deserialize_refund_failed_response_using_custom_converter()
    {
        // Arrange
        var options = new JsonSerializerOptions(JsonSerializerOptions.Default);
        options.Converters.Add(new IWebhookConverter());
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(Json));

        // Act
        var actual = JsonSerializer.Deserialize<IWebhook<WebhookData>>(ref reader, options);
        var refundFailed = actual as RefundFailed;

        // Assert
        refundFailed.Should().NotBeNull().And.BeEquivalentTo(expected);
    }
}
