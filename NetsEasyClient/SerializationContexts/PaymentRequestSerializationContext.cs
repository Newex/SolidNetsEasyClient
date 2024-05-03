using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;

namespace SolidNetsEasyClient.SerializationContexts;

/// <summary>
/// Serialization context for <see cref="SolidNetsEasyClient.Models.DTOs.Requests.Payments.PaymentRequest"/>
/// </summary>
[JsonSerializable(typeof(PaymentRequest))]
public partial class PaymentRequestSerializationContext : JsonSerializerContext
{
}
