using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;

namespace SolidNetsEasyClient.SerializationContexts;

/// <summary>
/// Serializer context for <see cref="Models.DTOs.Responses.Payments.PaymentResult"/>
/// </summary>
[JsonSourceGenerationOptions(AllowTrailingCommas = true, Converters = [
    typeof(GuidTypeConverter),
    typeof(NullableGuidTypeConverter),
    typeof(ConsumerTypeEnumConverter),
    typeof(CurrencyStringEnumConverter),
    typeof(EventNameConverter),
    typeof(IntegrationEnumConverter),
    typeof(InvoiceDateConverter),
    typeof(MonthOnlyConverter),
], UseStringEnumConverter = true)]
[JsonSerializable(typeof(PaymentResult))]
public partial class PaymentResultSerializationContext : JsonSerializerContext
{
}
