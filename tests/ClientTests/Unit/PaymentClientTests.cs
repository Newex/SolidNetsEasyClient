using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Models.DTOs;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;
using SolidNetsEasyClient.Tests.Tools;
using T = SolidNetsEasyClient.Models.DTOs.Requests.Payments.Integration;

namespace SolidNetsEasyClient.Tests.ClientTests.Unit;

[UnitTest("PaymentClient")]
public class PaymentClientTests
{
    [Fact]
    public async void A_proper_payment_returns_payment_with_id()
    {
        // Arrange
        const string responseJson = TestResponses.CreatePaymentResponse;
        var client = Setup.PaymentClient(HttpMethod.Post, NetsEndpoints.Relative.Payment, HttpStatusCode.Created, responseJson);
        var payment = Fakes.MinimalOrderExample;

        // Act
        var create = await client.CreatePaymentAsync(payment, T.EmbeddedCheckout, CancellationToken.None);
        var result = create.PaymentId != Guid.Empty;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async void An_error_throws_exception()
    {
        // Arrange
        var responseJson = string.Empty;
        var client = Setup.PaymentClient(HttpMethod.Post, NetsEndpoints.Relative.Payment, HttpStatusCode.BadRequest, responseJson);
        var payment = Fakes.MinimalOrderExample;

        // Act
        var ex = async () => await client.CreatePaymentAsync(payment, T.EmbeddedCheckout, CancellationToken.None);

        // Assert
        _ = await Assert.ThrowsAsync<HttpRequestException>(ex);
    }

    [Fact]
    public async void Can_get_the_status_of_an_existing_payment()
    {
        // Arrange
        const string responseJson = TestResponses.PaymentStatusResponseJson;
        var paymentID = Guid.NewGuid();
        var client = Setup.PaymentClient(HttpMethod.Get, NetsEndpoints.Relative.Payment + $"/{paymentID}", HttpStatusCode.OK, responseJson);

        // Act
        var status = await client.RetrievePaymentAsync(paymentID, CancellationToken.None);
        var result = status!.Payment.PaymentId != Guid.Empty;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async void Can_update_order_if_it_uses_easy_invoice_method_and_have_proper_fees()
    {
        // Arrange
        var paymentID = Guid.NewGuid();
        var client = Setup.PaymentClient(HttpMethod.Put, NetsEndpoints.Relative.Payment + $"/{paymentID}/orderitems", HttpStatusCode.NoContent, string.Empty);
        var updates = new OrderUpdate
        {
            Amount = 100000,
            Items = new List<Item>
            {
                new()
                {
                    Name = "Upgraded item",
                    Quantity = 1,
                    Reference = "upgrade_ref",
                    Unit = "monthly",
                    UnitPrice = 1000_00,
                    TaxRate = null
                }
            },
            PaymentMethods = new List<PaymentMethod>()
            {
                new()
                {
                    Fee = new()
                    {
                        Quantity = 1,
                        UnitPrice = 1000_00,
                        Name = "Upgraded item",
                        Reference = "upgrade_ref",
                        Unit = "monthly",
                        TaxRate = null
                    }
                }
            }
        };

        // Act
        var updated = await client.UpdateOrderAsync(paymentID, updates, CancellationToken.None);

        // Assert
        Assert.True(updated);
    }

    [Fact]
    public async void Can_update_order_without_any_payment_method()
    {
        // Arrange
        var paymentID = Guid.NewGuid();
        var client = Setup.PaymentClient(HttpMethod.Put, NetsEndpoints.Relative.Payment + $"/{paymentID}/orderitems", HttpStatusCode.NoContent, string.Empty);
        var updates = new OrderUpdate
        {
            Amount = 100000,
            Items = new List<Item>
            {
                new()
                {
                    Name = "Upgraded item",
                    Quantity = 1,
                    Reference = "upgrade_ref",
                    Unit = "monthly",
                    UnitPrice = 1000_00,
                    TaxRate = null
                }
            },
        };

        // Act
        var updated = await client.UpdateOrderAsync(paymentID, updates, CancellationToken.None);

        // Assert
        Assert.True(updated);
    }
}
