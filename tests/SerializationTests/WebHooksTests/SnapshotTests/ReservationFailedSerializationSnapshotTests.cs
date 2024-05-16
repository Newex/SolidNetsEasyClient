using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;
using static VerifyXunit.Verifier;

namespace SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests.SnapshotTests;

[SnapshotTest]
public class ReservationFailedSerializationSnapshotTests
{
    private readonly ReservationFailed expected = new()
    {
        Id = new("ef0f698086ac4e7493439ab4290695da"),
        MerchantId = 100008172,
        Timestamp = DateTimeOffset.Parse("2022-06-30T10:54:07.7765+02:00", CultureInfo.InvariantCulture),
        Event = EventName.ReservationFailed,
        Data = new()
        {
            Error = new()
            {
                Code = "33",
                Message = "Direct charge failed for payment id: 020b000062bd64ae0a5e7c95f6055f66. ErrorMessage: Refused by issuer",
                Source = "Issuer"
            },
            OrderItems =
                [
                    new()
                    {
                        Name = "NameBulkCharge1",
                        Quantity = 1,
                        Reference = "bulk123",
                        TaxRate = 0,
                        Unit = "quantity",
                        UnitPrice = 133
                    }
                ],
            Amount = new()
            {
                Amount = 133,
                Currency = Currency.DKK
            },
            PaymentId = new("020b000062bd64ae0a5e7c95f6055f66")
        }
    };

    [Fact]
    public Task Serialize_ReservationFailed()
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
