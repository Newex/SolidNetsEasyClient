using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// The webhook DTO for when a payment has failed to be cancelled
/// </summary>
/// <remarks>
/// The payment.cancel.failed event is triggered when a cancellation of a reservation has failed.
/// </remarks>
public record PaymentCancelledFailed : Webhook<PaymentCancelledFailedData> { }