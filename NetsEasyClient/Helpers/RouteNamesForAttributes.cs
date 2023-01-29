using System;
using SolidNetsEasyClient.Helpers.WebhookAttributes;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Helpers;

public static class RouteNamesForAttributes
{
    public static string GetRouteNameByEvent(EventName eventName)
    {
        return eventName switch
        {
            EventName.PaymentCreated => SolidNetsEasyPaymentCreatedAttribute.PaymentCreatedRoute,
            _ => throw new ArgumentOutOfRangeException(nameof(eventName))
        };
    }
}
