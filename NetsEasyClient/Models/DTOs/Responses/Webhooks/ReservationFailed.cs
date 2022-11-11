using System.Collections.Generic;
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
        get { return Data.OrderItems; }
        init { Data.OrderItems = value; }
    }
}
