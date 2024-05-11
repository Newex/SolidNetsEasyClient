using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;

namespace SolidNetsEasyClient.SerializationContexts;

/// <summary>
/// Refund serialization context
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
])]
[JsonSerializable(typeof(RefundResult))]
[JsonSerializable(typeof(RetrieveRefund))]
public partial class RefundSerializationContext : JsonSerializerContext
{
}
