using System.Text.Json.Serialization;
using MinimalAPI.Models;

namespace MinimalAPI.SerializationContexts;

[JsonSerializable(typeof(Product))]
internal partial class ProductSerializationContext : JsonSerializerContext
{
}
