using System.Collections.Generic;
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
    private Checkout checkout = new();
    private readonly List<WebHook> webHooks = new();

    private NetsPaymentBuilder(Order order)
    {
        this.order = order;
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
    /// <returns></returns>
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
                Default = ConsumerTypeEnum.B2C
            }
        };

        return this;
    }

    /// <summary>
    /// Subscribe to an event
    /// </summary>
    /// <remarks>
    /// The callback url must be a https endpoint and be acknowledged with a 200 OK status.
    /// </remarks>
    /// <param name="eventName">The event name</param>
    /// <param name="callbackUrl">The callback url</param>
    /// <param name="authorization">The authorization credentials</param>
    /// <returns>A payment builder</returns>
    public NetsPaymentBuilder SubscribeToEvent(EventNames eventName, string callbackUrl, string authorization)
    {
        webHooks.Add(new()
        {
            Authorization = authorization,
            EventName = eventName,
            Url = callbackUrl
        });

        return this;
    }

    /// <summary>
    /// Construct a payment request
    /// </summary>
    /// <returns>A payment request</returns>
    public PaymentRequest BuildPaymentRequest()
    {
        return new()
        {
            Order = order,
            Checkout = checkout,
            Notifications = new()
            {
                WebHooks = webHooks
            }
        };
    }

    /// <summary>
    /// Create a payment builder
    /// </summary>
    /// <param name="order">The order</param>
    /// <returns>A payment builder</returns>
    public static NetsPaymentBuilder CreatePayment(Order order)
    {
        return new NetsPaymentBuilder(order);
    }
}
