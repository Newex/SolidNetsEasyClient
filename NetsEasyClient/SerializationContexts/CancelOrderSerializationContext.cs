using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;

namespace SolidNetsEasyClient.SerializationContexts;

/// <summary>
/// Cancellation order serialization context.
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
[JsonSerializable(typeof(CancelOrder))]
public partial class CancelOrderSerializationContext : JsonSerializerContext
{
}
