using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;

namespace SolidNetsEasyClient.SerializationContexts;

/// <summary>
/// Payment serializer context
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
[JsonSerializable(typeof(PaymentStatus))]
[JsonSerializable(typeof(PaymentReference))]
public partial class PaymentSerializationContext : JsonSerializerContext
{
}
