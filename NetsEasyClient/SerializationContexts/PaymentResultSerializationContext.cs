using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;

namespace SolidNetsEasyClient.SerializationContexts;

/// <summary>
/// Serializer context for <see cref="Models.DTOs.Responses.Payments.PaymentResult"/>
/// </summary>
[JsonSourceGenerationOptions(AllowTrailingCommas = true, Converters = [typeof(PaymentResultConverter)])]
[JsonSerializable(typeof(PaymentResult))]
public partial class PaymentResultSerializationContext : JsonSerializerContext
{
}
