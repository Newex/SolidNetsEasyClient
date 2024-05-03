using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;

namespace SolidNetsEasyClient.SerializationContexts;

/// <summary>
/// Serializer context for <see cref="SolidNetsEasyClient.Models.DTOs.Responses.Payments.PaymentResult"/>
/// </summary>
[JsonSerializable(typeof(PaymentResult))]
public partial class PaymentResultSerializationContext : JsonSerializerContext
{
}
