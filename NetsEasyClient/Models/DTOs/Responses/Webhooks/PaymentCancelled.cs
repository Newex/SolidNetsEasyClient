using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// The webhook DTO for the when a payment has been cancelled
/// </summary>
/// <remarks>
/// The payment.cancel.created event is triggered when a reservation has been canceled.
/// </remarks>
public record PaymentCancelled : Webhook<PaymentCancelledData> { }
