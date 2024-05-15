using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;

namespace SolidNetsEasyClient.SerializationContexts;

/// <summary>
/// Subscription serialization context
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
[JsonSerializable(typeof(BulkSubscriptionVerification))]
[JsonSerializable(typeof(PageResult<SubscriptionProcessStatus>))]
[JsonSerializable(typeof(BulkSubscriptionResult))]
[JsonSerializable(typeof(BulkCharge))]
[JsonSerializable(typeof(SubscriptionDetails))]
public partial class SubscriptionSerializationContext : JsonSerializerContext
{
}
