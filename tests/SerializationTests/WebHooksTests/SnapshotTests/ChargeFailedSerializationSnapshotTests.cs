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
public class ChargeFailedSerializationSnapshotTests
{
    private readonly ChargeFailed chargeFailed = new()
    {
        Id = new("02a8000060923bcb6937598058c4e77a"),
        MerchantId = 100017120,
        Timestamp = DateTimeOffset.Parse("2021-05-05T08:31:39.2481+02:00", CultureInfo.InvariantCulture),
        Event = EventName.ChargeFailed,
        Data = new()
        {
            Error = new()
            {
                Code = "99",
                Message = "Auth Fin Failure",
                Source = "Internal"
            },
            ChargeId = new("02a8000060923bcb6937598058c4e77a"),
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
            ReservationId = new("0527cb1dc5d14491824644a84d5ccf69"),
            Amount = new()
            {
                Amount = 5500,
                Currency = Currency.SEK
            },
            PaymentId = new("029b000060923a766937598058c4e6fa")
        }
    };

    [Fact]
    public Task Serialize_ChargeFailed()
    {
        // Arrange
        var options = new JsonSerializerOptions(JsonSerializerOptions.Default);
        options.Converters.Add(new IWebhookConverter());
        var memoryStream = new MemoryStream();
        var writer = new Utf8JsonWriter(memoryStream);

        // Act
        JsonSerializer.Serialize<IWebhook<WebhookData>>(writer, chargeFailed, options);

        // Assert
        var bytes = memoryStream.ToArray();
        var jsonString = Encoding.UTF8.GetString(bytes);
        return VerifyJson(jsonString, SnapshotSettings.Settings);
    }
}
