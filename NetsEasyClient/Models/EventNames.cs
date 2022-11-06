using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models;

/// <summary>
/// A list of event names
/// </summary>
[JsonConverter(typeof(EventNameConverter))]
public record EventNames
{
    private readonly string @event;

    /// <summary>
    /// Instantiate a new object of <see cref="EventNames"/>
    /// </summary>
    /// <param name="event">The event name</param>
    internal EventNames(string @event)
    {
        this.@event = @event;
    }

    /// <summary>
    /// Use <see cref="EventNames"/> where a string is expected
    /// </summary>
    /// <param name="eventName">The event name</param>
    public static implicit operator EventNames(string eventName) => new(eventName);

    /// <summary>
    /// Use a string where an <see cref="EventNames"/> is expected
    /// </summary>
    /// <param name="eventName">The event name</param>
    public static implicit operator string(EventNames eventName) => eventName.@event;

    /// <summary>
    /// Get the name of the event
    /// </summary>
    /// <returns>The name of the event</returns>
    public override string ToString()
    {
        return @event;
    }

    /// <summary>
    /// Event names for the payment API
    /// </summary>
    public static class Payment
    {
        /// <summary>
        /// A payment has been created
        /// </summary>
        public static EventNames PaymentCreated => "payment.created";

        /// <summary>
        /// The amount of the payment has been reserved
        /// </summary>
        public static EventNames ReservationCreated => "payment.reservation.created.v2";

        /// <summary>
        /// A reservation attempt has failed
        /// </summary>
        public static EventNames ReservationFailed => "payment.reservation.failed";

        /// <summary>
        /// The customer has completed the checkout
        /// </summary>
        public static EventNames CheckoutCompleted => "payment.checkout.completed";

        /// <summary>
        /// The customer has successfully been charged, partially or fully
        /// </summary>
        public static EventNames ChargeCreated => "payment.charge.created.v2";

        /// <summary>
        /// A charge attempt has failed
        /// </summary>
        public static EventNames ChargeFailed => "payment.charge.failed";

        /// <summary>
        /// A refund has been initiated.
        /// </summary>
        public static EventNames RefundInitiated => "payment.refund.initiated.v2";

        /// <summary>
        /// A refund attempt has failed
        /// </summary>
        public static EventNames RefundFailed => "payment.refund.failed";

        /// <summary>
        /// A refund has successfully been completed
        /// </summary>
        public static EventNames RefundCompleted => "payment.refund.completed";

        /// <summary>
        /// A reservation has been canceled
        /// </summary>
        public static EventNames ReservationCancelled => "payment.cancel.created";

        /// <summary>
        /// A cancellation has failed
        /// </summary>
        public static EventNames ReservationCancellationFailed => "payment.cancel.failed";
    }
}
