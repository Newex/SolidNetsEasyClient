using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// The payment.charge.failed event is triggered when a charge attempt has failed.
/// </summary>
public record ChargeFailed : Webhook<ChargeFailedData> { }