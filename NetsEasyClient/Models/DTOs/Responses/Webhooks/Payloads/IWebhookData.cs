using System;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

/// <summary>
/// A webhook data payload
/// </summary>
public interface IWebhookData
{
    /// <summary>
    /// The payment identifier
    /// </summary>
    Guid PaymentId { get; }
}
