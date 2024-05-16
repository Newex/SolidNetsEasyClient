using System;
using System.Globalization;
using System.Text;
using System.Text.Json;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;
using SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests.ActualResponses;

namespace SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests;

[UnitTest]
public class RefundInitiatedSerializationTests
{
    const string Json = """
    {
    "id": "00fb000060923e006937598058c4e7f3",
    "timestamp": "2021-05-05T08:41:04.6081+02:00",
    "merchantNumber": 100017120,
    "event": "payment.refund.initiated.v2",
    "data": {
        "refundId": "00fb000060923e006937598058c4e7f3",
        "chargeId": "0107000060923dde6937598058c4e7ee",
        "amount": {
            "amount": 5500,
            "currency": "SEK"
        },
        "paymentId": "012b000060923cf26937598058c4e7e6"
        }
    }
    """;
    private readonly RefundInitiated expected = new()
    {
        Id = new("00fb000060923e006937598058c4e7f3"),
        Timestamp = DateTimeOffset.Parse("2021-05-05T08:41:04.6081+02:00", CultureInfo.InvariantCulture),
        MerchantNumber = 100017120,
        Event = EventName.RefundInitiated,
        Data = new()
        {
            RefundId = new("00fb000060923e006937598058c4e7f3"),
            ChargeId = new("0107000060923dde6937598058c4e7ee"),
            Amount = new()
            {
                Amount = 5500,
                Currency = Currency.SEK
            },
            PaymentId = new("012b000060923cf26937598058c4e7e6")
        }
    };

    [Fact]
    public void Can_deserialize_expected_refund_initiated_json_to_RefundInitiated_object()
    {
        // Arrange

        // Act
        var actual = JsonSerializer.Deserialize<RefundInitiated>(Json);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Can_deserialize_actual_refund_initiated_response_to_RefundInitiated_object()
    {
        // Arrange
        const string response = CleanedResponses.RefundInitiated;
        var expected = new RefundInitiated
        {
            Id = new("0092000063700099f0284eb68c580030"),
            Timestamp = DateTimeOffset.Parse("2022-11-12T21:22:50.0230+01:00", CultureInfo.InvariantCulture),
            MerchantNumber = 123456,
            Event = EventName.RefundInitiated,
            Data = new()
            {
                RefundId = new("0092000063700099f0284eb68c580030"),
                ChargeId = new("01f8000063700078f0284eb68c58002f"),
                Amount = new()
                {
                    Amount = 40_00,
                    Currency = Currency.DKK
                },
                PaymentId = new("02b900006370000ff0284eb68c58002b")
            }
        };

        // Act
        var actual = JsonSerializer.Deserialize<RefundInitiated>(response);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Deserialize_refund_initiated_response_using_custom_converter()
    {
        // Arrange
        var options = new JsonSerializerOptions(JsonSerializerOptions.Default);
        options.Converters.Add(new IWebhookConverter());
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(Json));

        // Act
        var actual = JsonSerializer.Deserialize<IWebhook<WebhookData>>(ref reader, options);
        var refundInitiated = actual as RefundInitiated;

        // Assert
        refundInitiated.Should().NotBeNull().And.BeEquivalentTo(expected);
    }
}
