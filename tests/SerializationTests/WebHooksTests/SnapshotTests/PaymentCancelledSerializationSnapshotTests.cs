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
public class PaymentCancelledSerializationSnapshotTests
{
    private readonly PaymentCancelled paymentCancelled = new()
    {
        Id = new("df7f9346097842bdb90c869b5c9ccfa9"),
        MerchantId = 100017120,
        Timestamp = DateTimeOffset.Parse("2021-05-04T22:33:33.5969+02:00", CultureInfo.InvariantCulture),
        Event = EventName.PaymentCancelled,
        Data = new()
        {
            CancelId = new("df7f9346097842bdb90c869b5c9ccfa9"),
            OrderItems =
                [
                    new()
                    {
                        Reference = "Sneaky NE2816-82",
                        Name = "Sneaky",
                        Quantity = 2,
                        Unit = "pcs",
                        UnitPrice = 25_00,
                        TaxRate = 1000,
                    }
                ],
            Amount = new()
            {
                Amount = 5500,
                Currency = Currency.SEK
            },
            PaymentId = new("006400006091abfe6937598058c4e47e")
        },
    };

    [Fact]
    public Task Deserialize_payment_created_json()
    {
        // Arrange
        var options = new JsonSerializerOptions(JsonSerializerOptions.Default);
        options.Converters.Add(new IWebhookConverter());
        var memoryStream = new MemoryStream();
        var writer = new Utf8JsonWriter(memoryStream);

        // Act
        JsonSerializer.Serialize<IWebhook<WebhookData>>(writer, paymentCancelled, options);

        // Assert
        var bytes = memoryStream.ToArray();
        var jsonString = Encoding.UTF8.GetString(bytes);
        return VerifyJson(jsonString, SnapshotSettings.Settings);
    }
}
