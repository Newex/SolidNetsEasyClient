using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Requests.Customers;
using SolidNetsEasyClient.Models.DTOs.Requests.Customers.Addresses;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;
using SolidNetsEasyClient.Models.DTOs.Requests.Styles;
using SolidNetsEasyClient.Models.DTOs.Requests.Webhooks;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.Builder;

/// <summary>
/// Nets payment builder
/// </summary>
/// <param name="options">The nets payment options</param>
public sealed class NetsPaymentBuilder(
    IOptions<NetsEasyOptions> options
)
{
    private readonly NetsEasyOptions options = options.Value;
    private PaymentRequest? paymentRequest;
    private readonly List<WebHook> webHooks = [];

    /// <summary>
    /// Construct a payment request.
    /// </summary>
    /// <param name="order">The order</param>
    /// <param name="myReference">Merchants (your) own reference identifier for the order.</param>
    /// <param name="handleCustomerData">Merchant (you) handle the customer data 
    /// info. Such that the customer does not have to reenter info on 
    /// Nets.</param>
    /// <param name="customer">The customer</param>
    /// <param name="customerType">The customer type</param>
    /// <param name="shippingDetails">The shipping details</param>
    /// <param name="chargeImmidiately">Charge the payment immidiately upon reservation. Default false.</param>
    /// <param name="rememberCustomer">True if the customer data should be persisted on the client. Careful of leaking private data. Opposite of <c>publicDevice</c> setting.</param>
    /// <param name="paymentMethod">Payment method. Only 'Easy-Invoice' is supported.</param>
    /// <param name="appearance">The checkout appearance</param>
    /// <returns>The builder</returns>
    public NetsPaymentBuilder CreateSinglePayment(Order order,
                                              string? myReference = null,
                                              bool? handleCustomerData = null,
                                              Consumer? customer = null,
                                              ConsumerTypeEnum? customerType = null,
                                              Shipping? shippingDetails = null,
                                              bool? chargeImmidiately = null,
                                              bool? rememberCustomer = null,
                                              PaymentMethod? paymentMethod = null,
                                              CheckoutAppearance? appearance = null)
    {
        var checkout = new Checkout
        {
            TermsUrl = options.TermsUrl,
            Appearance = appearance,
            CancelUrl = options.CancelUrl,
            Charge = chargeImmidiately,
            Consumer = customer,
            ConsumerType = CalculateCustomerType(customer, customerType, handleCustomerData),
            CountryCode = options.CountryCode,
            IntegrationType = options.IntegrationType,
            MerchantHandlesConsumerData = handleCustomerData ?? options.MerchantHandlesConsumerData,
            MerchantTermsUrl = options.PrivacyPolicyUrl,
            PublicDevice = rememberCustomer.HasValue && !rememberCustomer.GetValueOrDefault(),
            ReturnUrl = options.ReturnUrl,
            Shipping = shippingDetails,
            ShippingCountries = ConstructShippingCountries(),
            Url = options.IntegrationType.HasValue && options.IntegrationType.GetValueOrDefault() == Integration.EmbeddedCheckout ? options.CheckoutUrl : null,
        };
        var payment = new PaymentRequest()
        {
            Checkout = checkout,
            Order = order,
            MerchantNumber = options.NetsPartnerMerchantNumber,
            PaymentMethodsConfiguration = options.PaymentMethodsConfiguration,
            MyReference = myReference,
            PaymentMethods = paymentMethod is not null ? [paymentMethod] : null,
        };

        paymentRequest = payment;
        return this;
    }

    /// <summary>
    /// This is used to track the state changes of the payment as it is processed on NETS.
    /// Add a callback webhook notification for the payment.
    /// </summary>
    /// <remarks>
    /// Recommended to add webhooks to each payment request.
    /// NETS will then call the endpoint.
    /// Use <see cref="Extensions.RouteExtensions.MapNetsWebhook(Microsoft.AspNetCore.Builder.WebApplication, string, Delegate)"/>.
    /// </remarks>
    /// <param name="webHookUrl">The callback url. Must be https.</param>
    /// <param name="notification">The event to subscribe to.</param>
    /// <param name="authorization">The authorization to use on callback.</param>
    /// <returns>A builder</returns>
    public NetsPaymentBuilder AddWebhook(string webHookUrl, EventName notification, string? authorization = null)
    {
        webHooks.Add(new WebHook
        {
            Authorization = authorization ?? options.WebhookAuthorization,
            EventName = notification,
            Url = webHookUrl
        });
        return this;
    }

    /// <summary>
    /// Construct the payment request
    /// </summary>
    /// <returns>A payment request</returns>
    /// <exception cref="InvalidOperationException">Thrown if no payment request to build</exception>
    public PaymentRequest Build()
    {
        if (paymentRequest is null)
        {
            throw new InvalidOperationException("Must create payment before building payment request");
        }

        if (webHooks.Count > 0)
        {
            return paymentRequest with
            {
                Notifications = new()
                {
                    WebHooks = webHooks
                }
            };
        }

        return paymentRequest;
    }

    private ConsumerType? CalculateCustomerType(Consumer? customer, ConsumerTypeEnum? customerType, bool? handleCustomerData)
    {
        if (handleCustomerData ?? options.MerchantHandlesConsumerData.GetValueOrDefault())
        {
            // Should not handle customer on NETS, merchant handles data
            return null;
        }

        if (customer is null)
        {
            return new()
            {
                Default = customerType ?? options.DefaultCostumerType,
                SupportedTypes = options.SupportedTypes
            };
        }

        if (customer.PrivatePerson is not null && customer.Company is not null)
        {
            if (customerType is not null)
            {
                return new()
                {
                    Default = customerType,
                    SupportedTypes = options.SupportedTypes
                };
            }
        }
        else if (customer.PrivatePerson is not null)
        {
            return new()
            {
                Default = ConsumerTypeEnum.B2C,
                SupportedTypes = options.SupportedTypes
            };
        }
        else if (customer.Company is not null)
        {
            return new()
            {
                Default = ConsumerTypeEnum.B2B,
                SupportedTypes = options.SupportedTypes
            };
        }

        // Default to B2C
        return new()
        {
            Default = customerType ?? options.DefaultCostumerType ?? ConsumerTypeEnum.B2C,
            SupportedTypes = options.SupportedTypes
        };
    }

    private List<ShippingCountry>? ConstructShippingCountries()
    {
        if (options.SupportedShippingCountries is null)
        {
            return null;
        }

        List<ShippingCountry> shippingCountries = [];
        foreach (var country in options.SupportedShippingCountries)
        {
            shippingCountries.Add(new()
            {
                CountryCode = country
            });
        }

        return shippingCountries;
    }

}
