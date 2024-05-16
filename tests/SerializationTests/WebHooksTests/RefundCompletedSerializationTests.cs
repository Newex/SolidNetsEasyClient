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
public class RefundCompletedSerializationTests
{
    const string Json = """
    {
    "id": "458a4e068f454f768a40b9e576914820",
    "merchantId": 100017120,
    "timestamp": "2021-05-04T22:08:16.6623+02:00",
    "event": "payment.refund.completed",
    "data": {
        "refundId": "00fb000060923e006937598058c4e7f3",
        "invoiceDetails": {
            "distributionType": "Email",
            "invoiceDueDate": "2021-01-01T00:00:00",
            "invoiceNumber": "800328091K1"
        },
        "amount": {
            "amount": 5500,
            "currency": "SEK"
        },
        "paymentId": "012b000060923cf26937598058c4e7e6"
        }
    }
    """;

    private readonly RefundCompleted expected = new()
    {
        Id = new("458a4e068f454f768a40b9e576914820"),
        MerchantId = 100017120,
        Timestamp = DateTimeOffset.Parse("2021-05-04T22:08:16.6623+02:00", CultureInfo.InvariantCulture),
        Event = EventName.RefundCompleted,
        Data = new()
        {
            RefundId = new("00fb000060923e006937598058c4e7f3"),
            InvoiceDetails = new()
            {
                DistributionType = "Email",
                InvoiceDueDate = new DateOnly(2021, 01, 01),
                InvoiceNumber = "800328091K1"
            },
            Amount = new()
            {
                Amount = 5500,
                Currency = Currency.SEK
            },
            PaymentId = new("012b000060923cf26937598058c4e7e6")
        }
    };

    [Fact]
    public void Can_deserialize_expected_refund_completed_json_event_to_RefundCompleted_object()
    {
        // Arrange

        // Act
        var actual = JsonSerializer.Deserialize<RefundCompleted>(Json);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Deserialize_refund_completed_response()
    {
        // Arrange
        var options = new JsonSerializerOptions(JsonSerializerOptions.Default);
        options.Converters.Add(new IWebhookConverter());
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(Json));

        // Act
        var actual = JsonSerializer.Deserialize<IWebhook<WebhookData>>(ref reader, options);
        var refundCompleted = actual as RefundCompleted;

        // Assert
        refundCompleted.Should().NotBeNull().And.BeEquivalentTo(expected);
    }
}
