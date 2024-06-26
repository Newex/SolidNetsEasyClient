using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// A web hook payment reservation failed event DTO
/// </summary>
public record ReservationFailed : Webhook<ReservationFailedData>
{
    /// <summary>
    /// The list of order items that are associated with the failed reservation and charge. Contains at least one order item.
    /// </summary>
    public IList<Item> OrderItems
    {
        get => Data.OrderItems;
        init => Data.OrderItems = value;
    }

    /// <summary>
    /// The data associated with this event
    /// </summary>
    [Required]
    [JsonPropertyName("data")]
    public override ReservationFailedData Data { get; init; } = new();
}
