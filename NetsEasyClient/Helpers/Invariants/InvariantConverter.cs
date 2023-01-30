using System;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;

namespace SolidNetsEasyClient.Helpers.Invariants;

/// <summary>
/// Convert an order and event, before sending a request to Nets Easy, to an invariant that will be checkable post Nets Easy request.
/// </summary>
public static class InvariantConverter
{
    /// <summary>
    /// Convert to in invariant
    /// </summary>
    /// <param name="order">The order</param>
    /// <param name="eventName">The event name</param>
    /// <param name="nonce">The optional nonce</param>
    /// <returns>An invariant</returns>
    /// <exception cref="NotSupportedException">Thrown when the event is not supported</exception>
    public static IInvariantSerializable GetInvariant(Order order, EventName eventName, string? nonce)
    {
        var invariant = eventName switch
        {
            EventName.PaymentCreated => PaymentCreated(order, nonce),
            EventName.ReservationCreatedV1 => ReservationCreatedV1(order, nonce),
            _ => throw new NotSupportedException()
        };

        return invariant;
    }

    private static IInvariantSerializable PaymentCreated(Order order, string? nonce)
    {
        return new PaymentCreatedInvariant
        {
            OrderReference = order.Reference!,
            OrderItems = order.Items,
            Amount = order.Amount,
            Nonce = nonce
        };
    }

    private static IInvariantSerializable ReservationCreatedV1(Order order, string? nonce)
    {
        return new ReservationCreatedV1Invariant
        {
            Amount = order.Amount,
            Nonce = nonce
        };
    }
}
