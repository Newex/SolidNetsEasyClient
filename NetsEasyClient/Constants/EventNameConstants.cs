namespace SolidNetsEasyClient.Constants;

/// <summary>
/// Name of the webhook events
/// </summary>
public static class EventNameConstants
{
    /// <summary>
    /// A payment has been created
    /// </summary>
    public const string PaymentCreated = "payment.created";

    /// <summary>
    /// The amount of the payment has been reserved
    /// </summary>
    public const string ReservationCreated = "payment.reservation.created.v2";

    /// <summary>
    /// The amount of the payment has been reserved (version 1)
    /// </summary>
    public const string V1ReservationCreated = "payment.reservation.created";

    /// <summary>
    /// A reservation attempt has failed
    /// </summary>
    public const string ReservationFailed = "payment.reservation.failed";

    /// <summary>
    /// The customer has completed the checkout
    /// </summary>
    public const string CheckoutCompleted = "payment.checkout.completed";

    /// <summary>
    /// The customer has successfully been charged, partially or fully
    /// </summary>
    public const string ChargeCreated = "payment.charge.created.v2";

    /// <summary>
    /// A charge attempt has failed
    /// </summary>
    public const string ChargeFailed = "payment.charge.failed";

    /// <summary>
    /// A refund has been initiated.
    /// </summary>
    public const string RefundInitiated = "payment.refund.initiated.v2";

    /// <summary>
    /// A refund attempt has failed
    /// </summary>
    public const string RefundFailed = "payment.refund.failed";

    /// <summary>
    /// A refund has successfully been completed
    /// </summary>
    public const string RefundCompleted = "payment.refund.completed";

    /// <summary>
    /// A reservation has been canceled
    /// </summary>
    public const string ReservationCancelled = "payment.cancel.created";

    /// <summary>
    /// A cancellation has failed
    /// </summary>
    public const string ReservationCancellationFailed = "payment.cancel.failed";
}
