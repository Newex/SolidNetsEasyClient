using System;
using SolidNetsEasyClient.Models;

namespace SolidNetsEasyClient.Builder;

public sealed class NetsPaymentBuilder
{
    private readonly Order order;

    private Checkout checkout = new();

    private NetsPaymentBuilder(Order order)
    {
        this.order = order;
    }

    /// <summary>
    /// The checkout page is on my own page. The alternative is to host the page on the Nets servers.
    /// </summary>
    /// <returns>A payment builder</returns>
    public NetsPaymentBuilder EmbedOnMyPage()
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
    public NetsPaymentBuilder HostOnNetsServer()
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
    /// <exception cref="InvalidOperationException">Thrown if the payment is hosted on Nets</exception>
    public NetsPaymentBuilder SetCheckoutUrl(string url)
    {
        if (checkout.IntegrationType == Integration.HostedPaymentPage)
        {
            throw new InvalidOperationException("Cannot set checkout url for a checkout hosted on Nets servers. Use the embedded integration to set the checkout url");
        }

        checkout = checkout with
        {
            Url = url
        };
        return this;
    }

    public NetsPaymentBuilder SetReturnUrl(string returnUrl)
    {
        if (checkout.IntegrationType == Integration.EmbeddedCheckout)
        {
            throw new InvalidOperationException("Cannot set the return url for a checkout embedded on your own server. Use the hosted integration to set the return url");
        }

        checkout = checkout with
        {
            ReturnUrl = returnUrl
        };

        return this;
    }

    public NetsPaymentBuilder SetTheTermsUrl(string termsUrl)
    {
        checkout = checkout with
        {
            TermsUrl = termsUrl
        };

        return this;
    }

    public NetsPaymentBuilder SetCancellationUrl(string cancellationUrl)
    {
        if (checkout.IntegrationType == Integration.EmbeddedCheckout)
        {
            throw new InvalidOperationException("Cannot set the cancellation url for a checkout embedded on your own server. Use the hosted integration to set the cancellation url");
        }

        checkout = checkout with
        {
            CancelUrl = cancellationUrl
        };

        return this;
    }

    public NetsPaymentBuilder SetThePrivacyAndCookiePolicyUrl(string merchantTermsUrl)
    {
        checkout = checkout with
        {
            MerchantTermsUrl = merchantTermsUrl
        };
        return this;
    }

    public NetsPaymentBuilder WithPrivateCustomer(string? customerId, Person person, string? email, PhoneNumber? phone = null, ShippingAddress? shippingAddress = null, bool retypeCostumerData = false)
    {
        checkout = checkout with
        {
            Consumer = new()
            {
                Reference = customerId,
                Email = email,
                ShippingAddress = shippingAddress,
                PrivatePerson = person,
                PhoneNumber = phone
            },
            MerchantHandlesConsumerData = !retypeCostumerData,
            ConsumerType = new()
            {
                Default = ConsumerEnumType.B2C
            }
        };

        return this;
    }

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
            ConsumerType = new()
            {
                Default = ConsumerEnumType.B2B
            }
        };

        return this;
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
