using System;
using System.Buffers;
using System.Linq;
using ISO3166;
using Microsoft.Extensions.Logging;
using SolidNetsEasyClient.Logging.PaymentValidatorLogging;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;

namespace SolidNetsEasyClient.Validators;

/// <summary>
/// A payment validator
/// </summary>
internal static class PaymentValidator
{
    private static readonly SearchValues<char> searchValues = SearchValues.Create("<>'\"&\\");

    /// <summary>
    /// Checks if a payment is valid
    /// </summary>
    /// <param name="payment">The payment</param>
    /// <param name="logger">The logger</param>
    /// <returns>True if valid otherwise false</returns>
    internal static bool IsValidPaymentObject(PaymentRequest payment, ILogger logger)
    {
        // Checkout URL must not be empty
        // TODO: Add checking for illegal characters in
        // billingAddress.Country,
        // orderDetails.Currency,
        // checkoutUrl,
        // webhooks.eventName,
        // webhooks.Url,
        // merchantNumber
        if (!EmbeddedCheckoutHasCheckoutUrl(payment))
        {
            logger.ErrorEmbeddedCheckoutMissingUrl(payment);
            return false;
        }

        if (!MustHaveAtLeastOneOrderItem(payment))
        {
            logger.ErrorMissingOrderItem(payment);
            return false;
        }

        if (!HasTermsUrl(payment))
        {
            logger.ErrorMissingTerms(payment);
            return false;
        }

        if (!HasHostedReturnUrl(payment))
        {
            logger.ErrorMissingReturnUrl(payment);
            return false;
        }

        if (!ShippingCountryCodesMustBeISO3166(payment))
        {
            logger.ErrorInvalidShippingCountryFormat(payment);
            return false;
        }

        if (!HasMerchantConsumerDataAndNoConsumerType(payment))
        {
            logger.ErrorInvalidConsumerData(payment);
            return false;
        }

        if (!HasCountryCode(payment))
        {
            logger.ErrorInvalidCountryCodeCheckout(payment);
            return false;
        }

        if (!Below33WebHooks(payment))
        {
            logger.ErrorMaxWebhooks(payment);
            return false;
        }

        if (!CheckWebHooks(payment.Notifications))
        {
            logger.ErrorInvalidWebhooks(payment);
            return false;
        }

        if (!PaymentConfigurationAllMethodOrAllType(payment))
        {
            logger.ErrorInvalidPaymentConfiguration(payment);
            return false;
        }

        if (!NonSubscriptionMustHaveNonNegativeAmount(payment))
        {
            logger.ErrorInvalidPaymentAmount(payment);
            return false;
        }

        // MyReference,
        if (!MerchantReferenceIsProper(payment))
        {
            return false;
        }

        // consumer.shippingAddress.Country,
        if (!HasProperShippingAddress(payment))
        {
            return false;
        }

        // orderItems.Reference,
        // orderItems.Name,
        // orderItems.Unit,
        if (!HasProperOrderItems(payment))
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

    internal static bool ShippingCountryCodesMustBeISO3166(PaymentRequest payment)
    {
        var shippingCountries = payment.Checkout.ShippingCountries;
        if (shippingCountries?.Any() == true)
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
        var countWebHooks = payment.Notifications?.WebHooks.Count ?? 0;
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

    internal static bool CheckWebHooks(Models.DTOs.Requests.Webhooks.Notification? notification)
    {
        var checkWebHooks = notification?.WebHooks.Aggregate(true, (soFar, current) => soFar && ProperWebHookUrl(current.Url) && ProperAuthorization(current.Authorization));
        return checkWebHooks is null || checkWebHooks.Value;
    }

    internal static bool PaymentConfigurationAllMethodOrAllType(PaymentRequest payment)
    {
        var paymentConfiguration = payment.PaymentMethodsConfiguration;
        if (paymentConfiguration?.Any() != true)
        {
            return true;
        }

        var allMethods = paymentConfiguration.All(c => c.Name?.IsMethod == true);
        var allTypes = paymentConfiguration.All(c => c.Name?.IsType == true);
        return allMethods || allTypes;
    }

    internal static bool ProperAuthorization(string? header)
    {
        if (header is null)
        {
            return false;
        }

        // Must only contain alphanumeric characters
        var size = header.All(char.IsLetterOrDigit) && header.Length >= 8 && header.Length <= 32;
        return size;
    }

    internal static bool NonSubscriptionMustHaveNonNegativeAmount(PaymentRequest payment)
    {
        if (payment.Subscription is not null || payment.UnscheduledSubscription is not null)
        {
            return true;
        }

        return payment.Order.Amount > 0;
    }

    internal static bool HasProperOrderItems(PaymentRequest payment)
    {
        var result = true;
        foreach (var item in payment.Order.Items)
        {
            result = result
                && NoSpecialCharacters(item.Reference)
                && item.Reference.Length <= 128
                && NoSpecialCharacters(item.Name)
                && item.Name.Length <= 128
                && NoSpecialCharacters(item.Unit)
                && item.Unit.Length <= 128;

            if (!result)
            {
                break;
            }
        }

        return result;
    }

    internal static bool MerchantReferenceIsProper(PaymentRequest payment)
    {
        if (payment.MyReference is null)
        {
            return true;
        }

        if (NoSpecialCharacters(payment.MyReference)
            && payment.MyReference.Length <= 36)
        {
            return true;
        }

        return false;
    }

    internal static bool HasProperShippingAddress(PaymentRequest payment)
    {
        if (payment.Checkout?.Consumer?.ShippingAddress is null)
        {
            return true;
        }

        var result = true;
        var address = payment.Checkout.Consumer.ShippingAddress;
        if (address.AddressLine1 is not null)
        {
            result = NoSpecialCharacters(address.AddressLine1)
                    && address.AddressLine1.Length <= 128;
        }
        if (result && address.AddressLine2 is not null)
        {
            result = NoSpecialCharacters(address.AddressLine2)
                    && address.AddressLine2.Length <= 128;
        }
        if (result && address.PostalCode is not null)
        {
            result = NoSpecialCharacters(address.PostalCode)
                    && address.PostalCode.Length <= 12;
        }
        if (result && address.City is not null)
        {
            result = NoSpecialCharacters(address.City)
                    && address.City.Length <= 128;
        }
        if (result && address.Country is not null)
        {
            result = NoSpecialCharacters(address.Country)
                     && CountryCodeExists(address.Country);
        }

        return result;
    }

    private static bool NoSpecialCharacters(string? input)
    {
        if (input is not null)
            return input.AsSpan().IndexOfAny(searchValues) == -1;
        return true;
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
}
