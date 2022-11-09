using System;
using System.Collections.Generic;
using System.Threading;
using SolidNetsEasyClient.Tests.Tools;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;
using T = SolidNetsEasyClient.Models.DTOs.Requests.Payments.Integration;

namespace SolidNetsEasyClient.Tests.ClientTests.Integration;

[IntegrationTest]
public class NetsTests
{
    [Fact]
    public async void Get()
    {
        var nets = Setup.PaymentClient();
        var cancel = CancellationToken.None;
        var order = Fakes.MinimalOrderExample;

        var create = await nets.CreatePaymentAsync(order, T.EmbeddedCheckout, cancel);
        Assert.True(create.PaymentId != Guid.Empty);

        var status = await nets.GetPaymentStatusAsync(create.PaymentId, cancel);
        Assert.Equal(create.PaymentId, status!.Payment.PaymentId);

        var updatedReferences = await nets.UpdateReferences(status.Payment, status.Payment.Checkout.Url + "/a-new-path", "my-ref", cancel);
        Assert.True(updatedReferences);

        var updates = new OrderUpdate
        {
            Amount = 1000_00,
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
                        Quantity = 12,
                        UnitPrice = 1000_00,
                        Name = "Upgraded item",
                        Reference = "xupgrade_refx",
                        Unit = "monthly",
                        TaxRate = null
                    }
                }
            }
        };
        var updatedOrder = await nets.UpdateOrderAsync(create.PaymentId, updates, cancel);
        Assert.True(updatedOrder);

        var cancelPayment = await nets.CancelPaymentAsync(create.PaymentId, order, cancel);

        var terminated = await nets.TerminatePaymentAsync(create.PaymentId, cancel);
        Assert.True(terminated);
    }
}
