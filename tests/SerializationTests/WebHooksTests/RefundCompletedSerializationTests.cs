using System;
using System.Globalization;
using System.Text.Json;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

namespace SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests;

[UnitTest]
public class RefundCompletedSerializationTests
{
    [Fact]
    public void Can_deserialize_expected_refund_completed_json_event_to_RefundCompleted_object()
    {
        // Arrange
        const string json = "{\n" +
        "\"id\": \"458a4e068f454f768a40b9e576914820\",\n" +
        "\"merchantId\": 100017120,\n" +
        "\"timestamp\": \"2021-05-04T22:08:16.6623+02:00\",\n" +
        "\"event\": \"payment.refund.completed\",\n" +
        "\"data\": {\n" +
            "\"refundId\": \"00fb000060923e006937598058c4e7f3\",\n" +
            "\"invoiceDetails\": {\n" +
                "\"distributionType\": \"Email\",\n" +
                "\"invoiceDueDate\": \"2021-01-01T00:00:00\",\n" +
                "\"invoiceNumber\": \"800328091K1\"\n" +
            "},\n" +
            "\"amount\": {\n" +
                "\"amount\": 5500,\n" +
                "\"currency\": \"SEK\"\n" +
            "},\n" +
            "\"paymentId\": \"012b000060923cf26937598058c4e7e6\"\n" +
            "}\n" +
        "}\n";
        var expected = new RefundCompleted()
        {
            Id = new("458a4e068f454f768a40b9e576914820"),
            MerchantId = 100017120,
            Timestamp = DateTimeOffset.Parse("2021-05-04T22:08:16.6623+02:00", CultureInfo.InvariantCulture),
            Event = EventName.RefundCompleted,
            Data = new()
            {
                RefundId = new("00fb000060923e006937598058c4e7f3"),
                InvoiceDetails = new()
                {
                    DistributionType = "Email",
                    InvoiceDueDate = new DateOnly(2021, 01, 01),
                    InvoiceNumber = "800328091K1"
                },
                Amount = new()
                {
                    Amount = 5500,
                    Currency = Currency.SEK
                },
                PaymentId = new("012b000060923cf26937598058c4e7e6")
            }
        };

        // Act
        var actual = JsonSerializer.Deserialize<RefundCompleted>(json);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
