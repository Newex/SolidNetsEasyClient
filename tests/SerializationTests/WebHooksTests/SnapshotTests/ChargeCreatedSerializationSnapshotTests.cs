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

public class ChargeCreatedSerializationSnapshotTests
{
    private readonly ChargeCreated chargeCreated = new()
    {
        Id = new("01ee00006091b2196937598058c4e488"),
        Timestamp = DateTimeOffset.Parse("2021-05-04T22:44:10.1185+02:00", CultureInfo.InvariantCulture),
        MerchantNumber = 100017120,
        Event = EventName.ChargeCreated,
        Data = new()
        {
            ChargeId = new("01ee00006091b2196937598058c4e488"),
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
            PaymentMethod = PaymentMethodEnum.Visa,
            PaymentType = PaymentTypeEnum.PrepaidInvoice,
            Amount = new()
            {
                Amount = 5500,
                Currency = Currency.SEK
            },
            PaymentId = new("025400006091b1ef6937598058c4e487")
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
        JsonSerializer.Serialize<IWebhook<WebhookData>>(writer, chargeCreated, options);

        // Assert
        var bytes = memoryStream.ToArray();
        var jsonString = Encoding.UTF8.GetString(bytes);
        return VerifyJson(jsonString, SnapshotSettings.Settings);
    }
}
