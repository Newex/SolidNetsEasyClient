using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;
using SolidNetsEasyClient.Extensions;
using SolidNetsEasyClient.Models.DTOs.Contacts;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Requests.Customers;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;
using SolidNetsEasyClient.Models.DTOs.Requests.Webhooks;

namespace SolidNetsEasyClient.Builder;

/// <summary>
/// A builder for creating a Nets payment request
/// </summary>
public sealed class NetsPaymentBuilder
{
    private readonly Order order;
    private Checkout checkout = new()
    {
        IntegrationType = Integration.EmbeddedCheckout
    };
    private Subscription? subscription;
    private UnscheduledSubscription? unscheduled;
    private readonly List<WebHook> webHooks = new(32);
    private readonly int minimumPayment;

    private NetsPaymentBuilder(Order order, int minimumPayment)
    {
        this.order = order;
        this.minimumPayment = minimumPayment;
    }

    /// <summary>
    /// The checkout page is on my own page. The alternative is to host the page on the Nets servers.
    /// </summary>
    /// <returns>A payment builder</returns>
    public NetsPaymentBuilder EmbedCheckoutOnMyPage()
    {
        checkout = checkout with
        {
            IntegrationType = Integration.EmbeddedCheckout
        };

        return this;
    }

    /// <summary>
    /// The checkout page is hosted on Nets servers. The alternative is to embedd the checkout on your own page.
    /// </summary>
    /// <returns>A payment builder</returns>
    public NetsPaymentBuilder HostCheckoutOnNetsServer()
    {
        checkout = checkout with
        {
            IntegrationType = Integration.HostedPaymentPage
        };

        return this;
    }

    /// <summary>
    /// The payment can be charged immidiately on creation, i.e. the payment will deduct money from the customer instantly
    /// </summary>
    /// <remarks>
    /// The alternative is to withdraw the payment at a later time, e.g. when you send the ordered goods to a customer
    /// </remarks>
    /// <param name="charge">True if the payment should be charged immidiately otherwise you must manually charge the payment at a later time</param>
    /// <returns>A payment builder</returns>
    public NetsPaymentBuilder ChargePaymentOnCreation(bool charge = true)
    {
        checkout = checkout with
        {
            Charge = charge
        };
        return this;
    }

    /// <summary>
    /// Set the checkout url for the embedded checkout
    /// </summary>
    /// <param name="url">The url for the checkout page</param>
    /// <returns>A payment builder</returns>
    public NetsPaymentBuilder SetCheckoutUrl(string url)
    {
        checkout = checkout with
        {
            Url = url
        };

        return this;
    }

    /// <summary>
    /// Set the return url after completing the checkout on the hosted nets servers
    /// </summary>
    /// <remarks>
    /// Not necessary to set the return url if using the embedded checkout
    /// </remarks>
    /// <param name="returnUrl">The return url</param>
    /// <returns>A payment builder</returns>
    public NetsPaymentBuilder SetReturnUrl(string returnUrl)
    {
        checkout = checkout with
        {
            ReturnUrl = returnUrl
        };

        return this;
    }

    /// <summary>
    /// Set the terms url
    /// </summary>
    /// <param name="termsUrl">The terms url</param>
    /// <returns>A payment builder</returns>
    public NetsPaymentBuilder SetTheTermsUrl(string termsUrl)
    {
        checkout = checkout with
        {
            TermsUrl = termsUrl
        };

        return this;
    }

    /// <summary>
    /// Set the cancellation url for when customer cancels the checkout on the hosted nets servers
    /// </summary>
    /// <remarks>
    /// Not necessary to set the cancellation url if using the embedded checkout page.
    /// </remarks>
    /// <param name="cancellationUrl">The cancellation url</param>
    /// <returns>A payment builder</returns>
    public NetsPaymentBuilder SetCancellationUrl(string cancellationUrl)
    {
        checkout = checkout with
        {
            CancelUrl = cancellationUrl
        };

        return this;
    }

    /// <summary>
    /// Set the privacy and cookie policy url
    /// </summary>
    /// <param name="merchantTermsUrl">The url for the privacy and cookie policy</param>
    /// <returns>A payment builder</returns>
    public NetsPaymentBuilder SetThePrivacyAndCookiePolicyUrl(string merchantTermsUrl)
    {
        checkout = checkout with
        {
            MerchantTermsUrl = merchantTermsUrl
        };
        return this;
    }

    /// <summary>
    /// Set the end user as a private natural person
    /// </summary>
    /// <param name="customerId">The user id of the customer</param>
    /// <param name="firstName">The first given name of the customer</param>
    /// <param name="lastName">The last family name of the customer</param>
    /// <param name="email">The customer email</param>
    /// <param name="phone">The customer phone number</param>
    /// <param name="shippingAddress">The customer shipping address</param>
    /// <param name="retypeCostumerData">True if you want the customer to retype their details on the checkout page otherwise false</param>
    /// <returns>A payment builder</returns>
    public NetsPaymentBuilder WithPrivateCustomer(string? customerId, string? firstName, string? lastName, string? email, PhoneNumber? phone = null, ShippingAddress? shippingAddress = null, bool retypeCostumerData = false)
    {
        checkout = checkout with
        {
            Consumer = new()
            {
                Reference = customerId,
                Email = email,
                ShippingAddress = shippingAddress,
                PrivatePerson = new()
                {
                    FirstName = firstName,
                    LastName = lastName
                },
                PhoneNumber = phone
            },
            MerchantHandlesConsumerData = !retypeCostumerData,
            ConsumerType = !retypeCostumerData ? null : new()
            {
                Default = ConsumerTypeEnum.B2C
            }
        };

        return this;
    }

    /// <summary>
    /// Set the end user as a business customer
    /// </summary>
    /// <param name="customerId">The business customer id</param>
    /// <param name="company">The company details</param>
    /// <param name="email">The business email</param>
    /// <param name="phone">The business phone number</param>
    /// <param name="shippingAddress">The business shipping address</param>
    /// <param name="retypeCostumerData">True if you want the customer to retype their details on the checkout page otherwise false</param>
    /// <returns>A payment builder</returns>
    public NetsPaymentBuilder WithBusinessCustomer(string? customerId, Company company, string? email, PhoneNumber? phone = null, ShippingAddress? shippingAddress = null, bool retypeCostumerData = false)
    {
        checkout = checkout with
        {
            Consumer = new()
            {
                Reference = customerId,
                Email = email,
                ShippingAddress = shippingAddress,
                PhoneNumber = phone,
                Company = company
            },
            MerchantHandlesConsumerData = !retypeCostumerData,
            ConsumerType = !retypeCostumerData ? null : new()
            {
                Default = ConsumerTypeEnum.B2B
            }
        };

        return this;
    }

    /// <summary>
    /// Create the payment as part of a regular subscription
    /// </summary>
    /// <param name="interval">The minimum number of days between charges, an interval of zero means no restriction. This interval commences from either the day the subscription was created or the most recent subscription charge, whichever is later</param>
    /// <param name="years">The number of years the subscription should last</param>
    /// <param name="months">The number of months the subscription should last</param>
    /// <param name="days">The number of days the subscription should last</param>
    /// <param name="onTheEndOfTheMonth">True if the end date should fall on the last day of the month otherwise false</param>
    /// <param name="onMidnight">True if the end date should fall on midnight otherwise false</param>
    /// <returns>A payment builder</returns>
    public NetsPaymentBuilder AsRegularSubscription(int interval, int years, int months = 0, int days = 0, bool onTheEndOfTheMonth = false, bool onMidnight = false)
    {
        // Must have end date if creating subscription
        var endDate = DateTimeOffset.UtcNow.AddYears(years).AddMonths(months).AddDays(days);
        var date = onTheEndOfTheMonth ? endDate.AtTheEndOfTheMonth() : endDate;
        var maybeMidnight = onMidnight ? date.AtMidnight() : date;
        unscheduled = null;
        subscription = new()
        {
            Interval = interval,
            EndDate = maybeMidnight
        };
        return this;
    }

    /// <summary>
    /// Create the payment as part of a regular subscription
    /// </summary>
    /// <param name="interval">The minimum number of days between charges, an interval of zero means no restriction. This interval commences from either the day the subscription was created or the most recent subscription charge, whichever is later</param>
    /// <param name="endDate">The date and time when the subscription expires. It is not possible to charge this subscription after this date</param>
    /// <returns>A payment builder</returns>
    public NetsPaymentBuilder AsRegularSubscription(int interval, DateTimeOffset endDate)
    {
        // Must have end date if creating subscription
        unscheduled = null;
        subscription = new()
        {
            Interval = interval,
            EndDate = endDate
        };
        return this;
    }

    /// <summary>
    /// Create the payment as part of an unscheduled subscription.
    /// </summary>
    /// <remarks>
    /// The customer will be registering for a service.
    /// </remarks>
    /// <param name="create">True if a unscheduled card on file (uCoF) should be created at NETS, can be false if updating existing unscheduled subscription id</param>
    /// <param name="unscheduledSubscriptionId">The existing unscheduled subscription id</param>
    /// <returns>A payment builder</returns>
    /// <exception cref="ArgumentException">Thrown if create is false and unscheduled subscription id is missing</exception>
    public NetsPaymentBuilder AsUnscheduledSubscription(bool create, Guid? unscheduledSubscriptionId = null)
    {
        if (!create && unscheduledSubscriptionId.GetValueOrDefault() == Guid.Empty)
        {
            throw new ArgumentException("If not creating a new unscheduled subscription you must include the ID which can be found using the retrieve payment method in PaymentClient", nameof(unscheduledSubscriptionId));
        }

        subscription = null;
        unscheduled = new()
        {
            Create = create,
            UnscheduledSubscriptionId = unscheduledSubscriptionId
        };

        return this;
    }

    /// <summary>
    /// Subscribe to an event
    /// </summary>
    /// <remarks>
    /// The callback url must be an https endpoint and be acknowledged with a 200 OK status.
    /// This builder appends a query parameter with the order reference i.e. https://webhook.url/callback?orderId=my-order-id
    /// </remarks>
    /// <param name="eventName">The event name</param>
    /// <param name="callbackUrl">The callback url</param>
    /// <param name="authorization">The authorization credentials</param>
    /// <param name="append">Append the order reference id as query parameter to the url</param>
    /// <returns>A payment builder</returns>
    public NetsPaymentBuilder SubscribeToEvent(EventName eventName, string callbackUrl, string? authorization = null, bool append = true)
    {
        var url = append ? QueryHelpers.AddQueryString(callbackUrl, "orderId", order.Reference ?? "none") : callbackUrl;
        webHooks.Add(new()
        {
            Authorization = authorization,
            EventName = eventName,
            Url = url
        });

        return this;
    }

    /// <summary>
    /// Construct a payment request
    /// </summary>
    /// <returns>A payment request</returns>
    /// <exception cref="InvalidOperationException">Thrown when <see cref="Order.Amount"/> is zero or less than <see cref="minimumPayment"/></exception>
    public PaymentRequest BuildPaymentRequest()
    {
        if (unscheduled is not null && order.Amount < minimumPayment && order.Amount != 0)
        {
            throw new InvalidOperationException($"Order amount for an unscheduled subscription must be zero or more than {minimumPayment}");
        }

        return new()
        {
            Order = order,
            Checkout = checkout,
            Notifications = new()
            {
                WebHooks = webHooks
            },
            Subscription = subscription,
            UnscheduledSubscription = unscheduled
        };
    }

    /// <summary>
    /// Create a payment builder
    /// </summary>
    /// <remarks>
    /// The minimum order amount for an unscheduled subscription charge is determined by each individual provider Visa, MasterCard etc. and will be rejected if below a certain amount.
    /// </remarks>
    /// <param name="order">The order</param>
    /// <param name="minimumPayment">The minimum payment that an unscheduled subscription should contain</param>
    /// <returns>A payment builder</returns>
    public static NetsPaymentBuilder CreatePayment(Order order, int minimumPayment = 5_00)
    {
        return new NetsPaymentBuilder(order, minimumPayment);
    }
}
