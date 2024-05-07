using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;
using static VerifyXunit.Verifier;

namespace SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests.SnapshotTests;

[SnapshotTest]
public class RefundCompletedSerializationSnapshotTests
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
    public Task Serialize_RefundCompleted()
    {
        // Arrange
        var options = new JsonSerializerOptions(JsonSerializerOptions.Default);
        options.Converters.Add(new IWebhookConverter());
        var memoryStream = new MemoryStream();
        var writer = new Utf8JsonWriter(memoryStream);

        // Act
        JsonSerializer.Serialize<IWebhook<WebhookData>>(writer, expected, options);

        // Assert
        var bytes = memoryStream.ToArray();
        var jsonString = Encoding.UTF8.GetString(bytes);
        return VerifyJson(jsonString, SnapshotSettings.Settings);
    }
}
