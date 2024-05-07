using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;
using static VerifyXunit.Verifier;

namespace SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests.SnapshotTests;

[SnapshotTest]
public class RefundFailedSerializationSnapshotTests
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
    public Task Serialize_RefundFailed()
    {
        // Arrange
        var options = new JsonSerializerOptions(JsonSerializerOptions.Default)
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
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
