using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
public class ReservationCreatedV2SerializationSnapshotTests
{
    private readonly ReservationCreatedV2 expected2 = new()
    {
        Id = new("c25459e92ba54be1925493f987fb05a7"),
        Timestamp = DateTimeOffset.Parse("2021-05-04T22:09:08.4342+02:00", CultureInfo.InvariantCulture),
        MerchantNumber = 100017120,
        Event = EventName.ReservationCreatedV2,
        Data = new()
        {
            PaymentMethod = PaymentMethodEnum.Visa,
            PaymentType = PaymentTypeEnum.Card,
            Amount = new()
            {
                Amount = 55_00,
                Currency = Currency.SEK
            },
            PaymentId = new("02a900006091a9a96937598058c4e474")
        }
    };

    [Fact]
    public Task Serialize_ReservationCreated()
    {
        // Arrange
        var options = new JsonSerializerOptions(JsonSerializerOptions.Default);
        options.Converters.Add(new IWebhookConverter());
        var memoryStream = new MemoryStream();
        var writer = new Utf8JsonWriter(memoryStream);

        // Act
        JsonSerializer.Serialize<IWebhook<WebhookData>>(writer, expected2, options);

        // Assert
        var bytes = memoryStream.ToArray();
        var jsonString = Encoding.UTF8.GetString(bytes);
        return VerifyJson(jsonString, SnapshotSettings.Settings);
    }
}
