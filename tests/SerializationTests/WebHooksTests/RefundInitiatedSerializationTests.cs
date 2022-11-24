using System;
using System.Diagnostics;
using System.Text.Json;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests.ActualResponses;

namespace SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests;

[UnitTest]
public class RefundInitiatedSerializationTests
{
    [Fact]
    public void Can_deserialize_expected_refund_initiated_json_to_RefundInitiated_object()
    {
        // Arrange
        const string json = "{\n " +
        "\"id\": \"00fb000060923e006937598058c4e7f3\",\n" +
        "\"timestamp\": \"2021-05-05T08:41:04.6081+02:00\",\n" +
        "\"merchantNumber\": 100017120,\n" +
        "\"event\": \"payment.refund.initiated.v2\",\n" +
        "\"data\": {\n" +
            "\"refundId\": \"00fb000060923e006937598058c4e7f3\",\n" +
            "\"chargeId\": \"0107000060923dde6937598058c4e7ee\",\n" +
            "\"amount\": {\n" +
                "\"amount\": 5500,\n" +
                "\"currency\": \"SEK\"\n" +
            "},\n" +
            "\"paymentId\": \"012b000060923cf26937598058c4e7e6\"\n" +
            "}\n" +
        "}\n";
        var expected = new RefundInitiated
        {
            Id = new("00fb000060923e006937598058c4e7f3"),
            Timestamp = DateTimeOffset.Parse("2021-05-05T08:41:04.6081+02:00"),
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

        // Act
        var actual = JsonSerializer.Deserialize<RefundInitiated>(json);

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
            Timestamp = DateTimeOffset.Parse("2022-11-12T21:22:50.0230+01:00"),
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
}
