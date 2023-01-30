using Microsoft.Extensions.Logging;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;
using SolidNetsEasyClient.Validators;

namespace SolidNetsEasyClient.Logging.PaymentValidatorLogging;

/// <summary>
/// Log extensions for the <see cref="PaymentValidator"/>
/// </summary>
public static partial class LogExtensions
{
    /// <summary>
    /// Error missing url
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="payment">The payment</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Missing,
        Level = LogLevel.Error,
        Message = "Payment with embedded checkout must have a url in payment.checkout.url parameter. See {Payment}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorEmbeddedCheckoutMissingUrl(this ILogger logger, PaymentRequest payment);

    /// <summary>
    /// Error missing order item
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="payment">The payment</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Missing,
        Level = LogLevel.Error,
        Message = "Payment must have at least 1 order item. See {Payment}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorMissingOrderItem(this ILogger logger, PaymentRequest payment);

    /// <summary>
    /// Error missing terms url
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="payment">The payment</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Missing,
        Level = LogLevel.Error,
        Message = "Payment must have terms url. See {Payment}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorMissingTerms(this ILogger logger, PaymentRequest payment);

    /// <summary>
    /// Error missing return url
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="payment">The payment</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Missing,
        Level = LogLevel.Error,
        Message = "Payment must have return url when using hosted payment page. See {Payment}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorMissingReturnUrl(this ILogger logger, PaymentRequest payment);

    /// <summary>
    /// Error invalid shipping country codes
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="payment">The payment</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Payment shipping countries must be in ISO-3166 format. See {Payment}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidShippingCountryFormat(this ILogger logger, PaymentRequest payment);

    /// <summary>
    /// Error invalid consumer data
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="payment">The payment</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Payment must not have consumer type if the merchant handles consumer data - furthermore payment must only have 1 of either natural person consumer or company consumer. See {Payment}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidConsumerData(this ILogger logger, PaymentRequest payment);

    /// <summary>
    /// Error invalid country code checkout
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="payment">The payment</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Payment must have valid country code in payment.checkout.countrycode . See {Payment}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidCountryCodeCheckout(this ILogger logger, PaymentRequest payment);

    /// <summary>
    /// Error exceeding maximum number of webhooks
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="payment">The payment</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Payment must have maximum have 32 webhook callbacks. See {Payment}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorMaxWebhooks(this ILogger logger, PaymentRequest payment);

    /// <summary>
    /// Error invalid webhooks
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="payment">The payment</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Payment webhooks must have a callback with an https endpoint with a maximum length of 256 and the authorization header must only consist of alphanumeric characters with a maximum length of 32. See {Payment}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidWebhooks(this ILogger logger, PaymentRequest payment);

    /// <summary>
    /// Error invalid payment configuration
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="payment">The payment</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Payment must only have all payment methods in payment.paymentmethodsconfiguration or all payment types. See {Payment}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidPaymentConfiguration(this ILogger logger, PaymentRequest payment);

    /// <summary>
    /// Error invalid payment amount
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="payment">The payment</param>
    [LoggerMessage(
        EventId = LogEventIDs.Errors.Invalid,
        Level = LogLevel.Error,
        Message = "Payment must have a non-negative amount. See {Payment}",
        SkipEnabledCheck = true
    )]
    public static partial void ErrorInvalidPaymentAmount(this ILogger logger, PaymentRequest payment);
}
