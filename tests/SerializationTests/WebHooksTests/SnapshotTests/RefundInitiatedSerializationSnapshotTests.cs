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
public class RefundInitiatedSerializationSnapshotTests
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
    public Task Serialize_RefundInitiated()
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
