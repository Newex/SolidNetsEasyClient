using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.SerializationContexts;

/// <summary>
/// Webhook response serialization context
/// </summary>
[JsonSerializable(typeof(Webhook<WebhookData>))]
public partial class WebhookSerializationContext : JsonSerializerContext
{
}
