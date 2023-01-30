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
            EventName.PaymentCreated => OrderReferenceItemsAmountInvariant(order, nonce),
            EventName.ReservationCreatedV1 => AmountInvariant(order, nonce),
            EventName.ReservationCreatedV2 => AmountInvariant(order, nonce),
            EventName.ReservationFailed => OrderItemsAmountInvariant(order, nonce),
            EventName.CheckoutCompleted => OrderReferenceItemsAmountInvariant(order, nonce),
            EventName.PaymentCancelled => OrderItemsAmountInvariant(order, nonce),
            EventName.PaymentCancellationFailed => OrderItemsAmountInvariant(order, nonce),
            EventName.ChargeCreated => OrderItemsAmountInvariant(order, nonce),
            EventName.ChargeFailed => OrderItemsAmountInvariant(order, nonce),
            EventName.RefundInitiated => AmountInvariant(order, nonce),
            EventName.RefundCompleted => AmountInvariant(order, nonce),
            EventName.RefundFailed => AmountInvariant(order, nonce),
            _ => throw new NotSupportedException()
        };

        return invariant;
    }

    private static IInvariantSerializable OrderReferenceItemsAmountInvariant(Order order, string? nonce)
    {
        return new OrderReferenceItemsAmountInvariant
        {
            OrderReference = order.Reference!,
            OrderItems = order.Items,
            Amount = order.Amount,
            Nonce = nonce
        };
    }

    private static IInvariantSerializable AmountInvariant(Order order, string? nonce)
    {
        return new AmountInvariant
        {
            Amount = order.Amount,
            Nonce = nonce
        };
    }

    private static IInvariantSerializable OrderItemsAmountInvariant(Order order, string? nonce)
    {
        return new OrderItemsAmountInvariant
        {
            OrderItems = order.Items,
            Amount = order.Amount,
            Nonce = nonce
        };
    }
}
