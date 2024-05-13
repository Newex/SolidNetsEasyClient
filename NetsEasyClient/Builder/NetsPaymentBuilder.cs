using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using SolidNetsEasyClient.Models.DTOs.Contacts;
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

    // TODO: Add builder for subscriptions...

    /// <summary>
    /// Construct a payment request builder.
    /// </summary>
    /// <param name="order">The order</param>
    /// <param name="myReference">My payment reference</param>
    /// <returns>The builder</returns>
    public PaymentRequestBuilder CreatePayment(Order order, string? myReference = null)
    {
        return PaymentRequestBuilder.Create(options, order).SetMyPaymentReference(myReference);
    }

    /// <summary>
    /// A payment request builder
    /// </summary>
    public class PaymentRequestBuilder
    {
        private readonly NetsEasyOptions options;
        private readonly List<WebHook> webHooks = [];
        private readonly Order order;
        private string? myReference = null;
        private Consumer? customer;
        private ConsumerTypeEnum? defaultCostumerType;
        private List<ConsumerTypeEnum>? supportedCostumerTypes;
        private bool? chargeImmidiately;
        private bool? publicDevice;
        private bool? merchantHandlesConsumerData;
        private CheckoutAppearance? appearance;
        private Shipping? shipping;
        private List<PaymentMethod>? paymentMethods = null;

        private PaymentRequestBuilder(NetsEasyOptions options, Order order)
        {
            this.options = options;
            this.order = order;
        }

        /// <summary>
        /// Set my payment reference
        /// </summary>
        /// <param name="myReference">The payment reference</param>
        /// <returns>A payment builder</returns>
        public PaymentRequestBuilder SetMyPaymentReference(string? myReference)
        {
            this.myReference = myReference;
            return this;
        }

        /// <summary>
        /// Set the customer id reference. 
        /// If Nets should prefill customer data, you must supply that data. 
        /// Either as a (B2C) natural private person customer or as a (B2B) 
        /// company customer. Furthermore, merchant (you) will be responsible 
        /// for the customer data.
        /// </summary>
        /// <param name="customerId">The customer id reference.</param>
        /// <param name="prefillCustomerData">True if Nets should prefill
        /// customer info. Otherwise false. If false, the customer must repeat
        /// their information, on the checkout page.</param>
        /// <returns>A payment builder</returns>
        public ConsumerBuilder SetCustomerId(string customerId, bool prefillCustomerData = false)
        {
            merchantHandlesConsumerData = prefillCustomerData;
            return ConsumerBuilder.Create(this, customerId);
        }

        internal PaymentRequestBuilder SetCustomerInfo(Consumer customer)
        {
            this.customer = customer;
            return this;
        }

        /// <summary>
        /// Set the default checkout type used on the checkout page.
        /// </summary>
        /// <param name="defaultCostumerType">The default customer type</param>
        /// <returns>A payment builder</returns>
        public PaymentRequestBuilder SetDefaultCostumerTypeOnCheckout(ConsumerTypeEnum defaultCostumerType)
        {
            this.defaultCostumerType = defaultCostumerType;
            return this;
        }

        /// <summary>
        /// Add support for other customer types on the checkout page. This will
        /// enable the customer to change their info, to either a B2B or B2C
        /// customer.
        /// </summary>
        /// <param name="customerType">The additional supported customer type</param>
        /// <returns>A payment builder</returns>
        public PaymentRequestBuilder AddSupportForCustomerTypeOnCheckout(ConsumerTypeEnum customerType)
        {
            supportedCostumerTypes ??= [];
            supportedCostumerTypes.Add(customerType);
            return this;
        }

        /// <summary>
        /// If the payment request should be charged immidiately, upon reservation.
        /// Normally the merchant (you) must send a charge request to cash in the payment request.
        /// </summary>
        /// <returns>A payment builder</returns>
        public PaymentRequestBuilder ChargeImmidiately()
        {
            chargeImmidiately = true;
            return this;
        }

        /// <summary>
        /// This will ensure that the checkout does not save the customer data on the device that the customer uses.
        /// This could be due to the device being a shared public device such as in a library.
        /// If the customer returns to the page, and their data is saved, Nets
        /// will display the customer info, so that the customer does not have
        /// to reenter their details.
        /// </summary>
        /// <remarks>
        /// Sets the 'checkout.publicDevice' to true.
        /// </remarks>
        /// <returns>A payment builder</returns>
        public PaymentRequestBuilder DoNotSaveAnyCustomerDataOnDevice()
        {
            publicDevice = true;
            return this;
        }

        /// <summary>
        /// Include shipping to the customer.
        /// </summary>
        /// <returns></returns>
        public ShippingBuilder WithShipping()
        {
            return ShippingBuilder.Create(this);
        }

        internal PaymentRequestBuilder AddShipping(Shipping shipping)
        {
            this.shipping = shipping;
            return this;
        }

        /// <summary>
        /// Customize the checkout appearance.
        /// </summary>
        /// <param name="appearance">The checkout appearance.</param>
        /// <returns>A payment builder</returns>
        public PaymentRequestBuilder SetCheckoutAppearance(CheckoutAppearance appearance)
        {
            this.appearance = appearance;
            return this;
        }

        /// <summary>
        /// Add a payment method.
        /// </summary>
        /// <param name="paymentMethod">The payment method</param>
        /// <returns>A payment builder</returns>
        public PaymentRequestBuilder AddPaymentMethod(PaymentMethod paymentMethod)
        {
            paymentMethods ??= [];
            paymentMethods.Add(paymentMethod);
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
        /// <returns>A payment builder</returns>
        public PaymentRequestBuilder AddWebhook(string webHookUrl, EventName notification, string? authorization = null)
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
        /// Build the payment request
        /// </summary>
        /// <returns>The constructed payment request</returns>
        /// <exception cref="NotImplementedException"></exception>
        public PaymentRequest Build()
        {
            ConsumerType? consumerType;
            if (defaultCostumerType is not null && supportedCostumerTypes is not null)
            {
                consumerType = new()
                {
                    Default = defaultCostumerType,
                    SupportedTypes = supportedCostumerTypes
                };
            }
            else
            {
                consumerType = null;
            }
            var checkout = new Checkout
            {
                TermsUrl = options.TermsUrl,
                IntegrationType = options.IntegrationType,
                Appearance = appearance,
                CancelUrl = options.CancelUrl,
                Charge = chargeImmidiately,
                Consumer = customer,
                ConsumerType = consumerType,
                CountryCode = options.CountryCode,
                MerchantHandlesConsumerData = merchantHandlesConsumerData,
                MerchantTermsUrl = options.PrivacyPolicyUrl,
                PublicDevice = publicDevice.GetValueOrDefault(),
                ReturnUrl = options.ReturnUrl,
                Shipping = shipping,
                ShippingCountries = shipping?.Countries,
                Url = options.CheckoutUrl,
            };

            Notification? notification = webHooks.Count == 0
                ? null
                : new()
                {
                    WebHooks = webHooks
                };

            // Note: Subscriptions not included...
            return new PaymentRequest()
            {
                Order = order,
                Checkout = checkout,
                MerchantNumber = options.NetsPartnerMerchantNumber,
                Notifications = notification,
                MyReference = myReference,
                PaymentMethods = paymentMethods,
                PaymentMethodsConfiguration = options.PaymentMethodsConfiguration,
            };
        }

        internal static PaymentRequestBuilder Create(NetsEasyOptions options, Order order)
        {
            return new(options, order);
        }
    }

    /// <summary>
    /// A customer/consumer builder
    /// </summary>
    public class ConsumerBuilder
    {
        private readonly string consumerId;
        private readonly PaymentRequestBuilder paymentRequestBuilder;
        private string? email;
        private Person? person;
        private Company? company;
        private PhoneNumber? phoneNumber;
        private ShippingAddress? address;

        private ConsumerBuilder(PaymentRequestBuilder paymentRequestBuilder, string consumerId)
        {
            this.paymentRequestBuilder = paymentRequestBuilder;
            this.consumerId = consumerId;
        }

        /// <summary>
        /// Set the email for the costumer
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>A consumer builder</returns>
        public ConsumerBuilder SetEmail(string? email)
        {
            this.email = email;
            return this;
        }

        /// <summary>
        /// The costumer is a private person.
        /// This or <see cref="AsBusinessCustomer(Company)"/> must be set if you want Nets to prefill the costumer fields.
        /// </summary>
        /// <param name="person">The personal details.</param>
        /// <returns>A consumer builder</returns>
        public ConsumerBuilder AsPrivatePersonCustomer(Person person)
        {
            company = null;
            this.person = person;
            return this;
        }

        /// <summary>
        /// The customer is a business.
        /// This or <see cref="AsPrivatePersonCustomer(Person)"/> must be set if you want Nets to prefill the costumer fields.
        /// </summary>
        /// <param name="company">The business details.</param>
        /// <returns>A consumer builder</returns>
        public ConsumerBuilder AsBusinessCustomer(Company company)
        {
            person = null;
            this.company = company;
            return this;
        }

        /// <summary>
        /// Set the phone number for the customer.
        /// </summary>
        /// <param name="prefix">The prefix, such as the country code.</param>
        /// <param name="number">The number.</param>
        /// <returns>A consumer builder</returns>
        public ConsumerBuilder SetPhoneNumber(string prefix, string number)
        {
            phoneNumber = new()
            {
                Prefix = prefix,
                Number = number
            };
            return this;
        }

        /// <summary>
        /// Set the shipping address for the customer.
        /// </summary>
        /// <param name="address">The shipping address.</param>
        /// <returns>A consumer builder</returns>
        public ConsumerBuilder SetShippingAddress(ShippingAddress address)
        {
            this.address = address;
            return this;
        }

        /// <summary>
        /// Finish the customer details and add it to the payment request.
        /// </summary>
        /// <returns>A payment request builder</returns>
        public PaymentRequestBuilder AddCostumer()
        {
            return paymentRequestBuilder.SetCustomerInfo(new()
            {
                Reference = consumerId,
                Email = email,
                ShippingAddress = address,
                PhoneNumber = phoneNumber,
                PrivatePerson = person,
                Company = company
            });
        }

        internal static ConsumerBuilder Create(PaymentRequestBuilder paymentRequestBuilder, string consumerId)
        {
            return new(paymentRequestBuilder, consumerId);
        }
    }

    /// <summary>
    /// Shipping builder
    /// </summary>
    public class ShippingBuilder
    {
        private readonly PaymentRequestBuilder paymentRequestBuilder;
        private bool merchantHandlesShippingCost;
        private bool separateBillingAddress;
        private readonly List<ShippingCountry> countries = [];

        private ShippingBuilder(PaymentRequestBuilder paymentRequestBuilder)
        {
            this.paymentRequestBuilder = paymentRequestBuilder;
        }

        /// <summary>
        /// Add countries to ship to. Do not specify any countries, if you don't want to limit any country.
        /// If not specified this will use all the countries that Nets supports.
        /// </summary>
        /// <param name="countryCode">The 3 letter country code ISO-3166.</param>
        /// <returns>A shipping builder</returns>
        public ShippingBuilder AddShipToCountry(string countryCode)
        {
            countries.Add(new()
            {
                CountryCode = countryCode
            });
            return this;
        }

        /// <summary>
        /// If the shipping cost is included in the payment. 
        /// This means that the merchant (you) can update this order later, to
        /// calculate the actual shipping cost depending on the shipping address
        /// provided by the customer.
        /// </summary>
        /// <remarks>
        /// Sets the 'checkout.shipping.merchantHandlesShippingCost' to true.
        /// </remarks>
        /// <returns>A shipping builder</returns>
        public ShippingBuilder IncludeShippingCostInPayment()
        {
            merchantHandlesShippingCost = true;
            return this;
        }

        /// <summary>
        /// If enabled, the customer will be provided an option to specify
        /// separate addresses for billing and shipping on the checkout page.
        /// </summary>
        /// <returns>A shipping builder</returns>
        public ShippingBuilder SeparateBillingAndShippingAddress()
        {
            separateBillingAddress = true;
            return this;
        }

        /// <summary>
        /// Add shipment configuration to payment checkout.
        /// </summary>
        /// <returns>A payment request builder</returns>
        public PaymentRequestBuilder FinishShippingDetails()
        {
            return paymentRequestBuilder.AddShipping(new()
            {
                Countries = countries,
                MerchantHandlesShippingCost = merchantHandlesShippingCost,
                EnableBillingAddress = separateBillingAddress
            });
        }

        internal static ShippingBuilder Create(PaymentRequestBuilder paymentRequestBuilder)
        {
            return new(paymentRequestBuilder);
        }
    }
}
