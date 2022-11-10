using System.Diagnostics;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Enums;

/// <summary>
/// An event enum for web hook events
/// </summary>
[JsonConverter(typeof(EventNameConverter))]
[DebuggerDisplay("{@event}")]
public enum EventName
{
    /// <summary>
    /// A payment has been created
    /// </summary>
    PaymentCreated,

    /// <summary>
    /// The amount of the payment has been reserved
    /// </summary>
    ReservationCreated,

    /// <summary>
    /// A reservation attempt has failed
    /// </summary>
    ReservationFailed,

    /// <summary>
    /// The customer has completed the checkout
    /// </summary>
    CheckoutCompleted,

    /// <summary>
    /// The customer has successfully been charged, partially or fully
    /// </summary>
    ChargeCreated,

    /// <summary>
    /// A charge attempt has failed
    /// </summary>
    ChargeFailed,

    /// <summary>
    /// A refund has been initiated.
    /// </summary>
    RefundInitiated,

    /// <summary>
    /// A refund attempt has failed
    /// </summary>
    RefundFailed,

    /// <summary>
    /// A refund has successfully been completed
    /// </summary>
    RefundCompleted,

    /// <summary>
    /// A reservation has been canceled
    /// </summary>
    ReservationCancelled,

    /// <summary>
    /// A cancellation has failed
    /// </summary>
    ReservationCancellationFailed
}

/// <summary>
/// Extension helpers for the event name enumeration
/// </summary>
public static class EventNameHelper
{
    /// <summary>
    /// Convert a string event name to the corresponding enum
    /// </summary>
    /// <param name="eventName">The event name string</param>
    /// <returns>An enum of event name or null</returns>
    public static EventName? ToEventName(string? eventName)
    {
        EventName? result = eventName?.ToLowerInvariant() switch
        {
            EventNameConstants.PaymentCreated => EventName.PaymentCreated,
            EventNameConstants.ReservationCreated => EventName.ReservationCreated,
            EventNameConstants.ReservationFailed => EventName.ReservationFailed,
            EventNameConstants.CheckoutCompleted => EventName.CheckoutCompleted,
            EventNameConstants.ChargeCreated => EventName.ChargeCreated,
            EventNameConstants.ChargeFailed => EventName.ChargeFailed,
            EventNameConstants.RefundInitiated => EventName.RefundInitiated,
            EventNameConstants.RefundFailed => EventName.RefundFailed,
            EventNameConstants.RefundCompleted => EventName.RefundCompleted,
            EventNameConstants.ReservationCancellationFailed => EventName.ReservationCancellationFailed,
            EventNameConstants.ReservationCancelled => EventName.ReservationCancelled,
            _ => null,
        };
        return result;
    }

    /// <summary>
    /// Convert an enum to a corresponding string name
    /// </summary>
    /// <param name="eventName">The enum event name</param>
    /// <returns>A string representation of the enum</returns>
    public static string? ToStringEventName(this EventName eventName)
    {
        var result = eventName switch
        {
            EventName.PaymentCreated => EventNameConstants.PaymentCreated,
            EventName.ReservationCreated => EventNameConstants.ReservationCreated,
            EventName.ReservationFailed => EventNameConstants.ReservationFailed,
            EventName.CheckoutCompleted => EventNameConstants.CheckoutCompleted,
            EventName.ChargeCreated => EventNameConstants.ChargeCreated,
            EventName.ChargeFailed => EventNameConstants.ChargeFailed,
            EventName.RefundInitiated => EventNameConstants.RefundInitiated,
            EventName.RefundFailed => EventNameConstants.RefundFailed,
            EventName.RefundCompleted => EventNameConstants.RefundCompleted,
            EventName.ReservationCancelled => EventNameConstants.ReservationCancelled,
            EventName.ReservationCancellationFailed => EventNameConstants.ReservationCancellationFailed,
            _ => null
        };
        return result;
    }
}