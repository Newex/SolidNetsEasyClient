using System;
using System.Linq;
using ISO3166;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;

namespace SolidNetsEasyClient.Validators;

/// <summary>
/// A payment validator
/// </summary>
internal static class PaymentValidator
{
    /// <summary>
    /// Checks if a payment is valid
    /// </summary>
    /// <param name="payment">The payment</param>
    /// <returns>True if valid otherwise false</returns>
    internal static bool IsValidPaymentObject(PaymentRequest payment)
    {
        // Checkout URL must not be empty
        if (!EmbeddedCheckoutHasCheckoutUrl(payment))
        {
            return false;
        }

        if (!MustHaveAtLeastOneOrderItem(payment))
        {
            return false;
        }

        if (!HasTermsUrl(payment))
        {
            return false;
        }

        if (!HasHostedReturnUrl(payment))
        {
            return false;
        }

        if (!ShippingCountryCodeMustBeISO3166(payment))
        {
            return false;
        }

        if (!HasMerchantConsumerDataAndNoConsumerType(payment))
        {
            return false;
        }

        if (!HasCountryCode(payment))
        {
            return false;
        }

        if (!Below33WebHooks(payment))
        {
            return false;
        }

        if (!CheckWebHooks(payment))
        {
            return false;
        }

        if (!PaymentConfigurationAllMethodOrAllType(payment))
        {
            return false;
        }

        return true;
    }

    internal static bool EmbeddedCheckoutHasCheckoutUrl(PaymentRequest payment)
    {
        return payment.Checkout.IntegrationType switch
        {
            Integration.EmbeddedCheckout => !string.IsNullOrWhiteSpace(payment.Checkout.Url),
            _ => true
        };
    }

    internal static bool HasMerchantConsumerDataAndNoConsumerType(PaymentRequest payment)
    {
        if (payment.Checkout.MerchantHandlesConsumerData == true)
        {
            // Then no consumer type
            if (HasConsumerType(payment))
            {
                return false;
            }

            // Must have consumer. A person or company not both
            if (!HasConsumer(payment))
            {
                return false;
            }

            if (BothPersonAndCompany(payment))
            {
                return false;
            }
        }

        return true;
    }

    internal static bool MustHaveAtLeastOneOrderItem(PaymentRequest payment)
    {
        return payment.Order.Items.Any();
    }

    internal static bool HasHostedReturnUrl(PaymentRequest payment)
    {
        return payment.Checkout.IntegrationType switch
        {
            Integration.HostedPaymentPage => !string.IsNullOrWhiteSpace(payment.Checkout.ReturnUrl),
            _ => true
        };
    }

    internal static bool ShippingCountryCodeMustBeISO3166(PaymentRequest payment)
    {
        var shippingCountry = payment.Checkout.Consumer?.ShippingAddress?.Country;
        if (shippingCountry is not null)
        {
            var isValid = CountryCodeExists(shippingCountry);
            if (!isValid)
            {
                return false;
            }
        }

        var shippingCountries = payment.Checkout.ShippingCountries;
        if (shippingCountries is not null && shippingCountries.Any())
        {
            foreach (var destination in shippingCountries)
            {
                if (!CountryCodeExists(destination?.CountryCode))
                {
                    return false;
                }
            }
        }

        return true;
    }

    internal static bool HasTermsUrl(PaymentRequest payment)
    {
        return !string.IsNullOrWhiteSpace(payment.Checkout.TermsUrl);
    }

    internal static bool HasConsumerType(PaymentRequest payment)
    {
        return payment.Checkout.ConsumerType is not null;
    }

    internal static bool HasConsumer(PaymentRequest payment)
    {
        return payment.Checkout.Consumer is not null;
    }

    internal static bool BothPersonAndCompany(PaymentRequest payment)
    {
        return !((payment.Checkout.Consumer!.Company is not null) ^ (payment.Checkout.Consumer.PrivatePerson is not null));
    }

    internal static bool CountryCodeExists(string? countryCode)
    {
        var country = Country.List.SingleOrDefault(c => c.ThreeLetterCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase));
        return country is not null;
    }

    internal static bool Below33WebHooks(PaymentRequest payment)
    {
        var countWebHooks = payment.Notifications?.WebHooks.Count() ?? 0;
        return countWebHooks <= 32;
    }

    internal static bool HasCountryCode(PaymentRequest payment)
    {
        var isValid = true;
        var hasCountryCode = payment.Checkout.CountryCode is not null;
        if (hasCountryCode)
        {
            isValid = CountryCodeExists(payment.Checkout.CountryCode);
            if (!isValid)
            {
                return false;
            }
        }

        return isValid;
    }

    internal static bool CheckWebHooks(PaymentRequest payment)
    {
        var checkWebHooks = payment.Notifications?.WebHooks.Aggregate(true, (soFar, current) => soFar && ProperWebHookUrl(current.Url) && ProperAuthorization(current.Authorization));
        return checkWebHooks is null || checkWebHooks.Value;
    }

    internal static bool PaymentConfigurationAllMethodOrAllType(PaymentRequest payment)
    {
        var paymentConfiguration = payment.PaymentMethodsConfiguration;
        if (paymentConfiguration is null || !paymentConfiguration.Any())
        {
            return true;
        }

        var allMethods = paymentConfiguration.All(c => c.Name?.IsMethod == true);
        var allTypes = paymentConfiguration.All(c => c.Name?.IsType == true);
        return allMethods || allTypes;
    }

    private static bool ProperWebHookUrl(string? url)
    {
        if (url is null)
        {
            return true;
        }

        var isHttps = url.StartsWith("https", StringComparison.OrdinalIgnoreCase);
        var hasLength = url.Length <= 256;
        return isHttps && hasLength;
    }

    private static bool ProperAuthorization(string? header)
    {
        if (header is null)
        {
            return true;
        }

        // Must only contain alphanumeric characters
        var size = header.All(char.IsLetterOrDigit) && header.Length >= 8 && header.Length <= 32;
        return size;
    }
}
