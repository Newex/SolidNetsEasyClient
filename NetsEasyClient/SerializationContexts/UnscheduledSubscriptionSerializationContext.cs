using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;

namespace SolidNetsEasyClient.SerializationContexts;

/// <summary>
/// Unscheduled subscription serialization context
/// </summary>
[JsonSourceGenerationOptions(AllowTrailingCommas = true, Converters =
[
    typeof(GuidTypeConverter),
    typeof(NullableGuidTypeConverter),
    typeof(ConsumerTypeEnumConverter),
    typeof(CurrencyStringEnumConverter),
    typeof(EventNameConverter),
    typeof(IntegrationEnumConverter),
    typeof(InvoiceDateConverter),
    typeof(MonthOnlyConverter),
], UseStringEnumConverter = true)]
[JsonSerializable(typeof(UnscheduledSubscriptionChargeResult))]
[JsonSerializable(typeof(UnscheduledSubscriptionCharge))]
[JsonSerializable(typeof(UnscheduledSubscriptionDetails))]
public partial class UnscheduledSubscriptionSerializationContext : JsonSerializerContext
{
}
