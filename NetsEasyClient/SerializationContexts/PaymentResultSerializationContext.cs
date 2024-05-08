using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;

namespace SolidNetsEasyClient.SerializationContexts;

/// <summary>
/// Serializer context for <see cref="Models.DTOs.Responses.Payments.PaymentResult"/>
/// </summary>
[JsonSerializable(typeof(PaymentResult))]
public partial class PaymentResultSerializationContext : JsonSerializerContext
{
}
