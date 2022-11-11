using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// The payment.refund.failed event is triggered when a refund attempt has failed.
/// </summary>
public record RefundFailed : Webhook<RefundFailedData> { }