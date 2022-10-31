using System;
using System.Collections.Generic;
using SolidNetsEasyClient.Models;
using SolidNetsEasyClient.Models.Requests;
using SolidNetsEasyClient.Tests.Tools;

namespace SolidNetsEasyClient.Tests.PaymentValidatorTests;

public static class Setup
{
    internal static PaymentRequest DefaultPayment(bool company = true, int webHooks = 2, int paymentConfigurations = 1, int paymentMethods = 1, int orderItems = 5)
    {
        return Fakes.RandomPayment(company, termsUrl: true, webHooks, paymentConfigurations, paymentMethods, orderItems);
    }

    internal static PaymentRequest WithCheckout(this PaymentRequest payment, Func<Checkout, Checkout> setCheckout)
    {
        return payment with
        {
            Checkout = setCheckout(payment.Checkout)
        };
    }

#nullable enable
    internal static PaymentRequest WithShippingAddress(this PaymentRequest payment, Func<ShippingAddress, ShippingAddress?> setShippingAddress)
    {
        var consumer = payment.Checkout.Consumer ?? Fakes.RandomConsumer(payment.Checkout.Consumer?.Company is not null);
        var shipping = consumer?.ShippingAddress;
        var result = setShippingAddress(shipping ?? Fakes.RandomShippingAddress());
        var paymentConsumer = consumer is not null ? consumer with { ShippingAddress = result } : null;
        return payment with
        {
            Checkout = payment.Checkout with
            {
                Consumer = paymentConsumer
            }
        };
    }

    internal static PaymentRequest WithConsumer(this PaymentRequest payment, Func<Consumer, Consumer?> setConsumer)
    {
        var consumer = payment.Checkout.Consumer ?? Fakes.RandomConsumer(payment.Checkout.Consumer?.Company is not null);
        return payment with
        {
            Checkout = payment.Checkout with
            {
                Consumer = setConsumer(consumer)
            }
        };
    }

    internal static PaymentRequest WithNotifications(this PaymentRequest payment, int webHooks, Func<Notification, Notification?>? setNotification = null)
    {
        var defaultSetter = setNotification ?? ((c) => c);
        var notification = Fakes.RandomNotification(webHooks);
        return payment with
        {
            Notifications = defaultSetter(notification)
        };
    }

    internal static PaymentRequest WithPaymentConfigurations(this PaymentRequest payment, Func<IEnumerable<PaymentMethodConfiguration>, IEnumerable<PaymentMethodConfiguration>> setPaymentConfigurations)
    {
        var empty = new List<PaymentMethodConfiguration>();
        var configurations = payment.PaymentMethodsConfiguration ?? empty;
        var result = setPaymentConfigurations(configurations);
        return payment with
        {
            PaymentMethodsConfiguration = result
        };
    }
#nullable disable
}
