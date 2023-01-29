using System;
using SolidNetsEasyClient.Helpers.WebhookAttributes;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Helpers;

/// <summary>
/// Helper for retrieving route names for webhook endpoints
/// </summary>
public static class RouteNamesForAttributes
{
    /// <summary>
    /// Get the route name for an event
    /// </summary>
    /// <param name="eventName">The event</param>
    /// <returns>A route name</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when event name is not found</exception>
    public static string GetRouteNameByEvent(EventName eventName)
    {
        return eventName switch
        {
            EventName.PaymentCreated => SolidNetsEasyPaymentCreatedAttribute.PaymentCreatedRoute,
            _ => throw new ArgumentOutOfRangeException(nameof(eventName))
        };
    }
}
