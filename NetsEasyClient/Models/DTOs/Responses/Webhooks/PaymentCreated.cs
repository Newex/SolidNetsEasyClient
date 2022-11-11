using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// The <see cref="EventName.PaymentCreated"/> event is triggered when a new payment is created. This happens when the customer hits the "Pay" button on the checkout page.
/// </summary>
public record PaymentCreated : Webhook<PaymentCreatedData> { }
