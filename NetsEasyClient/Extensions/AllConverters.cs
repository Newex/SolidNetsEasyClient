using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// Extension helper method
/// </summary>
public static class ConverterExtensions
{
    /// <summary>
    /// Add all the converters
    /// </summary>
    /// <param name="converters">The base converters</param>
    public static void AddAll(this IList<JsonConverter> converters)
    {
        foreach (var converter in AllConverters)
        {
            converters.Add(converter);
        }
    }

    /// <summary>
    /// Retrieve all json converters
    /// </summary>
    public static IList<JsonConverter> AllConverters =>
    [
        // Webhook converters
        new IWebhookConverter(),
        new PaymentCreatedConverter(),
        new PaymentCancelledConverter(),
        new ChargeCreatedConverter(),
        new CheckoutCompletedConverter(),
        new PaymentCancellationFailedConverter(),
        new RefundCompletedConverter(),
        new RefundFailedConverter(),
        new RefundInitiatedConverter(),
        new ReservationCreatedV1Converter(),
        new ReservationCreatedV2Converter(),
        new ReservationFailedConverter(),

        // Property converters
        new GuidTypeConverter(),
        new NullableGuidTypeConverter(),
        new ConsumerTypeEnumConverter(),
        new CurrencyStringEnumConverter(),
        new EventNameConverter(),
        new IntegrationEnumConverter(),
        new InvoiceDateConverter(),
        new MonthOnlyConverter(),
        new NullableDateTimeOffsetConverter()
    ];
}