using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// A web hook DTO for the checkout completed event
/// </summary>
public record CheckoutCompleted : Webhook<CheckoutCompletedData> { }
