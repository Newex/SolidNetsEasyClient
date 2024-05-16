using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;

namespace SolidNetsEasyClient.SerializationContexts;

/// <summary>
/// Order update serialization context
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
[JsonSerializable(typeof(OrderUpdate))]
public partial class OrderUpdateSerializationContext : JsonSerializerContext
{
}
