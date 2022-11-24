using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// A web hook payment reservation created event DTO
/// </summary>
public record ReservationCreatedV1 : Webhook<ReservationCreatedDataV1> { }
