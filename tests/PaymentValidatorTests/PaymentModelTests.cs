using System.Collections.Generic;
using System.Linq;
using SolidNetsEasyClient.Models;
using SolidNetsEasyClient.Tests.Tools;
using SolidNetsEasyClient.Validators;

namespace SolidNetsEasyClient.Tests.PaymentValidatorTests;

[UnitTest, Feature("ModelTests")]
public class PaymentModelTests
{
    [Fact]
    public void Payment_without_order_items_is_invalid()
    {
        // Arrange
        var payment = Setup.DefaultPayment(orderItems: 0);

        // Act
        var result = PaymentValidator.MustHaveAtLeastOneOrderItem(payment);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Payment_with_at_least_1_order_item_is_valid()
    {
        // Arrange
        var payment = Setup.DefaultPayment(orderItems: 1);

        // Act
        var result = PaymentValidator.MustHaveAtLeastOneOrderItem(payment);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Payment_without_a_terms_url_is_invalid()
    {
        // Arrange
        var payment = Setup.DefaultPayment(orderItems: 1).WithCheckout(c => c with
        {
            TermsUrl = null!
        });

        // Act
        var result = PaymentValidator.HasTermsUrl(payment);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Payment_with_an_empty_terms_url_is_invalid()
    {
        // Arrange
        var payment = Setup.DefaultPayment(orderItems: 1).WithCheckout(c => c with
        {
            TermsUrl = string.Empty
        });

        // Act
        var result = PaymentValidator.HasTermsUrl(payment);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Payment_with_a_non_empty_terms_url_is_valid()
    {
        // Arrange
        var payment = Setup.DefaultPayment();

        // Act
        var result = PaymentValidator.HasTermsUrl(payment);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Payment_with_a_country_shipping_address_that_is_not_iso_3166_compliant_is_invalid()
    {
        // Arrange
        var payment = Setup
            .DefaultPayment()
            .WithShippingAddress(ship => ship with
            {
                Country = "NotAThreeLetterCountryCode"
            });

        // Act
        var result = PaymentValidator.ShippingCountryCodeMustBeISO3166(payment);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Payment_with_a_country_shipping_address_that_is_iso_3166_compliant_is_valid()
    {
        // Arrange
        var payment = Setup
            .DefaultPayment()
            .WithShippingAddress(ship => ship with
            {
                Country = "dnk" // Danmark lower case
            });

        // Act
        var result = PaymentValidator.ShippingCountryCodeMustBeISO3166(payment);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Payment_with_a_shipping_country_code_is_case_insensitive()
    {
        // Arrange
        var payment = Setup
            .DefaultPayment()
            .WithShippingAddress(ship => ship with
            {
                Country = "dNk" // Danmark mixed casing
            });

        // Act
        var result = PaymentValidator.ShippingCountryCodeMustBeISO3166(payment);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Payment_where_merchant_handles_consumer_data_should_be_invalid_if_consumer_type_is_defined()
    {
        // Arrange
        var payment = Setup
            .DefaultPayment()
            .WithCheckout(checkout => checkout with
            {
                MerchantHandlesConsumerData = true,
                ConsumerType = Fakes.RandomConsumerType()
            });

        // Act
        var result = PaymentValidator.HasMerchantConsumerDataAndNoConsumerType(payment);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Payment_where_merchant_handles_consumer_data_should_be_valid_if_consumer_type_is_not_defined_and_have_a_consumer()
    {
        // Arrange
        var payment = Setup
            .DefaultPayment()
            .WithCheckout(checkout => checkout with
            {
                MerchantHandlesConsumerData = true,
                ConsumerType = null
            })
            .WithConsumer(consumer => consumer);

        // Act
        var result = PaymentValidator.HasMerchantConsumerDataAndNoConsumerType(payment);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Payment_where_merchant_handles_consumer_data_should_be_invalid_if_consumer_type_is_not_defined_and_does_not_have_a_consumer()
    {
        // Arrange
        var payment = Setup
            .DefaultPayment()
            .WithCheckout(checkout => checkout with
            {
                MerchantHandlesConsumerData = true,
                ConsumerType = null
            })
            .WithConsumer(consumer => null);

        // Act
        var result = PaymentValidator.HasMerchantConsumerDataAndNoConsumerType(payment);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Payment_that_does_not_have_a_checkout_country_code_in_iso3166_is_invalid()
    {
        // Arrange
        var payment = Setup
            .DefaultPayment()
            .WithCheckout(checkout => checkout with
            {
                CountryCode = "GRR" // Country code does not exist
            });

        // Act
        var result = PaymentValidator.HasCountryCode(payment);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Payment_that_does_have_a_checkout_country_code_in_iso3166_is_valid()
    {
        // Arrange
        var payment = Setup
            .DefaultPayment()
            .WithCheckout(checkout => checkout with
            {
                CountryCode = "gRL" // Greenland mixed casing
            });

        // Act
        var result = PaymentValidator.HasCountryCode(payment);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Payment_that_does_have_a_checkout_country_code_is_valid()
    {
        // Arrange
        var payment = Setup
            .DefaultPayment()
            .WithCheckout(checkout => checkout with
            {
                CountryCode = null
            });

        // Act
        var result = PaymentValidator.HasCountryCode(payment);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Payment_over_32_webhooks_is_invalid()
    {
        // Arrange
        var payment = Setup
            .DefaultPayment()
            .WithNotifications(webHooks: 33);

        // Act
        var result = PaymentValidator.Below32WebHooks(payment);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Payment_with_32_webhooks_is_valid()
    {
        // Arrange
        var payment = Setup
            .DefaultPayment()
            .WithNotifications(webHooks: 32);

        // Act
        var result = PaymentValidator.Below32WebHooks(payment);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Payment_below_32_webhooks_is_valid()
    {
        // Arrange
        var payment = Setup
            .DefaultPayment()
            .WithNotifications(webHooks: 12);

        // Act
        var result = PaymentValidator.Below32WebHooks(payment);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Payment_without_any_webhooks_is_valid()
    {
        // Arrange
        var payment = Setup
            .DefaultPayment()
            .WithNotifications(webHooks: 0, c => null);

        // Act
        var result = PaymentValidator.Below32WebHooks(payment);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Payment_webhooks_must_have_a_max_authorization_size_of_32_and_use_https_valid()
    {
        // Arrange
        var payment = Setup
            .DefaultPayment()
            .WithNotifications(1, n => n with
            {
                WebHooks = new List<WebHook>
                {
                    Fakes.RandomWebHook() with
                    {
                        Authorization = "this_is_a_32_length_string_authz",
                        Url = "https://somesite.org/callback/endpoint"
                    }
                }.AsEnumerable()
            });

        // Act
        var result = PaymentValidator.CheckWebHooks(payment);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Payment_webhooks_must_have_a_max_authorization_size_of_32_and_use_https_invalid()
    {
        // Arrange
        var payment = Setup
            .DefaultPayment()
            .WithNotifications(1, n => n with
            {
                WebHooks = new List<WebHook>
                {
                    Fakes.RandomWebHook() with
                    {
                        Authorization = "this_is_a_33_length_string_authz_",
                        Url = "https://somesite.org/callback/endpoint"
                    }
                }.AsEnumerable()
            });

        // Act
        var result = PaymentValidator.CheckWebHooks(payment);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Payment_with_only_all_methods_in_configuration_is_valid()
    {
        // Arrange
        var payment = Setup
            .DefaultPayment(paymentConfigurations: 2)
            .WithPaymentConfigurations(_ =>
            new List<PaymentMethodConfiguration>
            {
                new()
                {
                    Enabled = true,
                    Name = PaymentMethodConfigurationType.Methods.MasterCard
                },
                new()
                {
                    Enabled = true,
                    Name = PaymentMethodConfigurationType.Methods.Visa
                },
            });

        // Act
        var result = PaymentValidator.PaymentConfigurationAllMethodOrAllType(payment);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Payment_with_only_all_types_in_configuration_is_valid()
    {
        // Arrange
        var payment = Setup
            .DefaultPayment(paymentConfigurations: 2)
            .WithPaymentConfigurations(_ =>
            new List<PaymentMethodConfiguration>
            {
                new()
                {
                    Enabled = true,
                    Name = PaymentMethodConfigurationType.Types.Invoice
                },
                new()
                {
                    Enabled = true,
                    Name = PaymentMethodConfigurationType.Types.Card
                },
            });

        // Act
        var result = PaymentValidator.PaymentConfigurationAllMethodOrAllType(payment);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Payment_with_mixed_payment_methods_and_types_is_invalid()
    {
        // Arrange
        var payment = Setup
            .DefaultPayment(paymentConfigurations: 2)
            .WithPaymentConfigurations(_ =>
            new List<PaymentMethodConfiguration>
            {
                new()
                {
                    Enabled = true,
                    Name = PaymentMethodConfigurationType.Methods.PayPal
                },
                new()
                {
                    Enabled = true,
                    Name = PaymentMethodConfigurationType.Types.Card
                },
            });

        // Act
        var result = PaymentValidator.PaymentConfigurationAllMethodOrAllType(payment);

        // Assert
        Assert.False(result);
    }
}
