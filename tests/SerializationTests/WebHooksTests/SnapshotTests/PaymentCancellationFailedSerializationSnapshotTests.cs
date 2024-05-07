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
public class PaymentCancellationFailedSerializationSnapshotTests
{
    const string Json = """
    {
        "id": "df7f9346097842bdb90c869b5c9ccfa9",
        "merchantId": 100017120,
        "timestamp": "2021-05-06T11:37:30.1114+02:00",
        "event": "payment.cancel.failed",
        "data": {
            "error": {
                "code": "25",
                "message": "Trans not found",
                "source": "Internal"
            },
            "cancelId": "df7f9346097842bdb90c869b5c9ccfa9",
            "orderItems": [
                {
                    "reference": "Sneaky NE2816-82",
                    "name": "Sneaky",
                    "quantity": 2,
                    "unit": "pcs",
                    "unitPrice": 2500,
                    "taxRate": 1000,
                    "taxAmount": 500,
                    "netTotalAmount": 5000,
                    "grossTotalAmount": 5500
                }
            ],
            "amount": {
                "amount": 5500,
                "currency": "SEK"
            },
            "paymentId": "023a00005ea744ed368812223c86c299"
        }
    }
    """;

    private readonly PaymentCancellationFailed expected = new()
    {
        Id = new("df7f9346097842bdb90c869b5c9ccfa9"),
        MerchantId = 100017120,
        Timestamp = DateTimeOffset.Parse("2021-05-06T11:37:30.1114+02:00", CultureInfo.InvariantCulture),
        Event = EventName.PaymentCancellationFailed,
        Data = new()
        {
            Error = new()
            {
                Code = "25",
                Message = "Trans not found",
                Source = "Internal"
            },
            CancelId = new("df7f9346097842bdb90c869b5c9ccfa9"),
            OrderItems =
                [
                    new()
                    {
                        Reference = "Sneaky NE2816-82",
                        Name = "Sneaky",
                        Quantity = 2,
                        Unit = "pcs",
                        UnitPrice = 2500,
                        TaxRate = 1000,
                    }
                ],
            Amount = new()
            {
                Amount = 5500,
                Currency = Currency.SEK
            },
            PaymentId = new("023a00005ea744ed368812223c86c299")
        }
    };

    [Fact]
    public Task Serialize_PaymentCancellationFailed()
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
