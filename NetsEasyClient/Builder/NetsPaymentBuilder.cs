using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using SolidNetsEasyClient.Extensions;
using SolidNetsEasyClient.Helpers;
using SolidNetsEasyClient.Helpers.Encryption;
using SolidNetsEasyClient.Helpers.Encryption.Encodings;
using SolidNetsEasyClient.Helpers.Encryption.Flows;
using SolidNetsEasyClient.Helpers.Invariants;
using SolidNetsEasyClient.Helpers.WebhookAttributes;
using SolidNetsEasyClient.Models.DTOs.Contacts;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Requests.Customers;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;
using SolidNetsEasyClient.Models.DTOs.Requests.Webhooks;
using SolidNetsEasyClient.Models.Options;
using SolidNetsEasyClient.Validators;

namespace SolidNetsEasyClient.Builder;

// TODO: Make this injectable from DI and register it
/// <summary>
/// A builder for creating a Nets payment request
/// </summary>
public sealed class NetsPaymentBuilder
{
    private readonly Order order;
    private Integration integration;
    private bool charge;
    private string? checkoutUrl;
    private string? returnUrl;
    private string termsUrl = string.Empty;
    private string? cancellationUrl;
    private string? merchantTermsUrl;
    private Consumer? consumer;
    private bool? merchantHandlesConsumerData;
    private ConsumerType? consumerType;
    private Subscription? subscription;
    private UnscheduledSubscription? unscheduled;
    private readonly List<WebHook> webHooks = new(32);
    private readonly int minimumPayment;
    private readonly string baseUrl;
    private readonly string complementName;
    private readonly string nonceName;
    private readonly IHasher hasher;
    private readonly byte[] key;
    private readonly int nonceLength;

    internal NetsPaymentBuilder(string baseUrl, string complementName, string nonceName, IHasher hasher, byte[] key, int nonceLength, Order order, int minimumPayment)
    {
        this.order = order;
        this.minimumPayment = minimumPayment;
        this.baseUrl = baseUrl.TrimEnd('/');
        this.complementName = complementName;
        this.nonceName = nonceName;
        this.hasher = hasher;
        this.key = key;
        if (nonceLength > 256)
        {
            throw new ArgumentOutOfRangeException(nameof(nonceLength));
        }

        this.nonceLength = nonceLength;
    }

    /// <summary>
    /// The checkout page is on my own page. The alternative is to host the page on the Nets servers.
    /// </summary>
    /// <returns>A payment builder</returns>
    public NetsPaymentBuilder EmbedCheckoutOnMyPage()
    {
        integration = Integration.EmbeddedCheckout;

        return this;
    }

    /// <summary>
    /// The checkout page is hosted on Nets servers. The alternative is to embedd the checkout on your own page.
    /// </summary>
    /// <returns>A payment builder</returns>
    public NetsPaymentBuilder HostCheckoutOnNetsServer()
    {
        integration = Integration.HostedPaymentPage;

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
        this.charge = charge;
        return this;
    }

    /// <summary>
    /// Set the checkout url for the embedded checkout
    /// </summary>
    /// <param name="url">The url for the checkout page</param>
    /// <returns>A payment builder</returns>
    public NetsPaymentBuilder SetCheckoutUrl(string url)
    {
        checkoutUrl = url;

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
        this.returnUrl = returnUrl;
        return this;
    }

    /// <summary>
    /// Set the terms url
    /// </summary>
    /// <param name="termsUrl">The terms url</param>
    /// <returns>A payment builder</returns>
    public NetsPaymentBuilder SetTheTermsUrl(string termsUrl)
    {
        this.termsUrl = termsUrl;
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
        this.cancellationUrl = cancellationUrl;
        return this;
    }

    /// <summary>
    /// Set the privacy and cookie policy url
    /// </summary>
    /// <param name="merchantTermsUrl">The url for the privacy and cookie policy</param>
    /// <returns>A payment builder</returns>
    public NetsPaymentBuilder SetThePrivacyAndCookiePolicyUrl(string merchantTermsUrl)
    {
        this.merchantTermsUrl = merchantTermsUrl;
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
        consumer = new()
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
        };
        merchantHandlesConsumerData = !retypeCostumerData;
        consumerType = !retypeCostumerData ? null : new()
        {
            Default = ConsumerTypeEnum.B2C
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
        consumer = new()
        {
            Reference = customerId,
            Email = email,
            ShippingAddress = shippingAddress,
            PhoneNumber = phone,
            Company = company
        };
        merchantHandlesConsumerData = !retypeCostumerData;
        consumerType = !retypeCostumerData ? null : new()
        {
            Default = ConsumerTypeEnum.B2B
        };
        return this;
    }

    /// <summary>
    /// Create the payment as a one time payment
    /// </summary>
    /// <returns>A payment builder</returns>
    public NetsPaymentBuilder AsSinglePayment()
    {
        unscheduled = null;
        subscription = null;
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
    /// <param name="eventName">The event name</param>
    /// <param name="callbackUrl">The callback url</param>
    /// <param name="authorization">The credentials that will be sent in the HTTP Authorization request header of the callback. Must be between 8 and 32 characters long and contain alphanumeric characters.</param>
    /// <returns>A payment builder</returns>
    /// <exception cref="ArgumentException">Thrown when invalid authorization</exception>
    public NetsPaymentBuilder SubscribeToEvent(EventName eventName, string callbackUrl, string authorization)
    {
        var validAuthorization = PaymentValidator.ProperAuthorization(authorization);
        if (!validAuthorization)
        {
            throw new ArgumentException("Authorization must be between 8 and 32 long and contain alphanumeric characters");
        }
        webHooks.Add(new()
        {
            Authorization = authorization,
            EventName = eventName,
            Url = callbackUrl
        });

        return this;
    }

    /// <summary>
    /// Subscribe to an event. The webhook url is calculated by using attribute e.g. <see cref="SolidNetsEasyPaymentCreatedAttribute"/> for the payment created event and the <see cref="PlatformPaymentOptions.BaseUrl"/>
    /// </summary>
    /// <param name="eventName">The event name</param>
    /// <param name="urlHelper">The url helper</param>
    /// <param name="routeValues">The additional route values for the webhook endpoint</param>
    /// <param name="withNonce">True if nonce should be added otherwise false</param>
    /// <returns>A payment builder</returns>
    /// <exception cref="InvalidOperationException">Thrown when invalid <see cref="Order.Reference"/> or webhook endpoint url</exception>
    public NetsPaymentBuilder SubscribeToEvent(EventName eventName, IUrlHelper urlHelper, object? routeValues = null, bool withNonce = true)
    {
        if (string.IsNullOrWhiteSpace(order.Reference))
        {
            throw new InvalidOperationException("Order reference must be set to use the in-built webhook creator");
        }

        var routeName = RouteNamesForAttributes.GetRouteNameByEvent(eventName);
        var webhookUrl = urlHelper.RouteUrl(routeName, routeValues)?.TrimStart('/');
        if (webhookUrl is null)
        {
            throw new InvalidOperationException("Could not create webhook url endpoint. Ensure you have marked an action with the attribute of SolidNetsEasy_{EventName}_Attribute");
        }

        string? nonce = null;
        if (withNonce)
        {
            var nonceSource = CustomBase62Converter.Encode(RandomNumberGenerator.GetBytes(256));
            nonce = nonceSource[..nonceLength];
        }

        (var authorization, var complement) = PaymentCreatedFlow.CreateAuthorization(hasher, key, new PaymentCreatedInvariant
        {
            OrderReference = order.Reference,
            OrderItems = order.Items,
            Amount = order.Amount,
            Nonce = nonce
        });
        var url = UrlQueryHelpers.AddQuery($"{baseUrl}/{webhookUrl}", (complementName, complement), (nonceName, nonce));

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
    /// <exception cref="InvalidOperationException">Thrown if the subscription has a total of what is less than allowed</exception>
    public PaymentRequest BuildPaymentRequest()
    {
        if (unscheduled is not null && order.Amount < minimumPayment && order.Amount != 0)
        {
            throw new InvalidOperationException($"Order amount for an unscheduled subscription must be zero or more than {minimumPayment}");
        }

        return new()
        {
            Order = order,
            Checkout = new Checkout()
            {
                Url = checkoutUrl,
                IntegrationType = integration,
                ReturnUrl = returnUrl,
                CancelUrl = cancellationUrl,
                Consumer = consumer,
                TermsUrl = termsUrl,
                MerchantTermsUrl = merchantTermsUrl,
                ConsumerType = consumerType,
                MerchantHandlesConsumerData = merchantHandlesConsumerData,
                Charge = charge
            },
            Notifications = new()
            {
                WebHooks = webHooks
            },
            Subscription = subscription,
            UnscheduledSubscription = unscheduled
        };
    }
}
