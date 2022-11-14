using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// The payment.refund.completed event is triggered when a refund has successfully been completed.
/// </summary>
public record RefundCompleted : Webhook<RefundCompletedData> { }
