using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;

namespace MinimalAPI.SerializationContexts;

[JsonSerializable(typeof(Order))]
internal partial class OrderSerializationContext : JsonSerializerContext
{
}
