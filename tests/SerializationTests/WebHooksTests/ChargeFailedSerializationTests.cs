using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.Json;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests;

[UnitTest]
public class ChargeFailedSerializationTests
{
    const string Json = """
    {
        "id": "02a8000060923bcb6937598058c4e77a",
        "merchantId": 100017120,
        "timestamp": "2021-05-05T08:31:39.2481+02:00",
        "event": "payment.charge.failed",
        "data": {
            "error": {
                "code": "99",
                "message": "Auth Fin Failure",
                "source": "Internal"
            },
            "chargeId": "02a8000060923bcb6937598058c4e77a",
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
            "reservationId": "0527cb1dc5d14491824644a84d5ccf69",
            "amount": {
                "amount": 5500,
                "currency": "SEK"
            },
            "paymentId": "029b000060923a766937598058c4e6fa"
        }
    }
    """;

    private readonly ChargeFailed expected = new()
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
    public void Can_deserialize_expected_charge_failed_json_to_ChargeFailed_object()
    {
        // Arrange
        Debug.WriteLine(Json);

        // Act
        var actual = JsonSerializer.Deserialize<ChargeFailed>(Json);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Can_deserialize_using_custom_converter()
    {
        // Arrange
        var options = new JsonSerializerOptions(JsonSerializerOptions.Default);
        options.Converters.Add(new IWebhookConverter());
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(Json));

        // Act
        var actual = JsonSerializer.Deserialize<IWebhook<WebhookData>>(ref reader, options);
        var chargeFailed = actual as ChargeFailed;

        // Assert
        chargeFailed.Should().NotBeNull().And.BeEquivalentTo(expected);
    }
}
