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
public class CheckoutCompletedSerializationSnapshotTests
{
    private readonly CheckoutCompleted expected = new()
    {
        Id = new("36ce3ff4a896450ea2b70f3263554772"),
        MerchantId = 100017120,
        Timestamp = DateTimeOffset.Parse("2021-05-04T22:09:08.4342+02:00", CultureInfo.InvariantCulture),
        Event = EventName.CheckoutCompleted,
        Data = new()
        {
            Order = new()
            {
                Amount = new()
                {
                    Amount = 55_00,
                    Currency = Currency.SEK
                },
                Reference = "Hosted Demo Order",
                OrderItems =
                    [
                        new()
                        {
                            Reference = "Sneaky NE2816-82",
                            Name = "Sneaky",
                            Quantity = 2,
                            Unit = "pcs",
                            UnitPrice = 25_00,
                            TaxRate = 10_00,
                        }
                    ],
            },
            Consumer = new()
            {
                FirstName = "John",
                LastName = "Doe",
                BillingAddress = new()
                {
                    AddressLine1 = "Solgatan 4",
                    AddressLine2 = "",
                    City = "STOCKHOLM",
                    Country = "SWE",
                    PostCode = "11522",
                    ReceiverLine = "John doe"
                },
                Country = "SWE",
                Email = "john.doe@example.com",
                IP = "192.230.114.3",
                PhoneNumber = new()
                {
                    Prefix = "+46",
                    Number = "12345678",
                },
                ShippingAddress = new()
                {
                    AddressLine1 = "Solgatan 4",
                    AddressLine2 = "",
                    City = "STOCKHOLM",
                    Country = "SWE",
                    PostCode = "11522",
                    ReceiverLine = "John Doe"
                }
            },
            PaymentId = new("02a900006091a9a96937598058c4e474")
        }
    };

    [Fact]
    public Task Serialize_PaymentCreated()
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
