using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.SerializationContexts;

/// <summary>
/// Serializer context
/// </summary>
[JsonSerializable(typeof(IWebhook<WebhookData>))]
public partial class WebhookSerializationContext : JsonSerializerContext
{
}
