using System;
using System.Threading;
using System.Threading.Tasks;
using SolidNetsEasyClient.Models;
using SolidNetsEasyClient.Models.Requests;
using SolidNetsEasyClient.Models.Results;
using SolidNetsEasyClient.Models.Status;

namespace SolidNetsEasyClient.Clients;

/// <summary>
/// Payment client, responsible for mediating communication between NETS payment API and this client
/// </summary>
public interface IPaymentClient
{
    /// <summary>
    /// Cancels the specified payment. When a payment is canceled, the reserved amount of the payment will be released to the customer's payment card
    /// </summary>
    /// <remarks>
    /// Note that;
    /// Only full cancels are allowed. The amount must always match the total amount of the order;
    /// Once a payment has been charged (fully or partially), the payment cannot be canceled;
    /// It is not possible to change the status of a payment once it has been canceled;
    /// Nets will not charge a fee for a canceled payment
    /// </remarks>
    /// <param name="paymentID">The payment ID</param>
    /// <param name="order">The order</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>An awaitable task</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="paymentID"/> is empty</exception>
    Task<bool> CancelPaymentAsync(Guid paymentID, Order order, CancellationToken cancellationToken);

    /// <summary>
    /// Cancels a pending refund. A refund can be in a pending state when there are not enough funds in the merchant's account to make the refund
    /// </summary>
    /// <param name="refundId">The refund Id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>An awaitable task</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="refundId"/> is empty</exception>
    Task<bool> CancelPendingRefundAsync(Guid refundId, CancellationToken cancellationToken);

    /// <summary>
    /// Charges the specified payment. Charge a payment on the same day as you ship the matching order.A payment can be fully charged or partially charged
    /// </summary>
    /// <remarks>
    /// Full charge: Your customer will be charged the total amount of the payment. The amount must be specified in the request body and is required to match the total amount of the payment
    /// Partial charge: Only charge for a subset of the order items. In this case you have to provide the amount and the orderItems you want to charge in the request body
    /// </remarks>
    /// <param name="paymentID">The payment ID</param>
    /// <param name="charge">The charges</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <param name="idempotencyKey">The optional idempotency key used to avoid calling API endpoint several times</param>
    /// <returns>A charge result</returns>
    Task<ChargeResult?> ChargePaymentAsync(Guid paymentID, Charge charge, CancellationToken cancellationToken, string? idempotencyKey = null);

    /// <summary>
    /// Create a payment in NETS
    /// </summary>
    /// <remarks>
    /// Initializes a new payment object that becomes the object used throughout the checkout flow for a particular customer and order. Creating a payment object is the first step when you intend to accept a payment from your customer. Entering the amount 100 corresponds to 1 unit of the currency entered, such as e.g. 1 NOK
    /// </remarks>
    /// <param name="payment">The payment</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A payment result or throws an exception</returns>
    /// <exception cref="ArgumentException">Thrown if invalid payment object</exception>
    Task<PaymentResult> CreatePaymentAsync(PaymentRequest payment, CancellationToken cancellationToken);

    /// <summary>
    /// Get status for a payment
    /// </summary>
    /// <remarks>
    /// Retrieves the details of an existing payment. The paymentId is obtained from Nets when creating a <see cref="CreatePaymentAsync(PaymentRequest, CancellationToken)"/>
    /// </remarks>
    /// <param name="paymentID">The payment ID</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A payment status or null</returns>
    Task<PaymentStatus?> GetPaymentStatusAsync(Guid paymentID, CancellationToken cancellationToken);

    /// <summary>
    /// Refunds a previously settled transaction (a charged payment). The refunded amount will be transferred back to the customer's account. The required chargeId is returned from the Charge payment method
    /// A settled transaction can be fully or partially refunded:
    /// * Full refund requires only the amount to be specified in the request body.
    /// * Partial refund requires the amount and the orderItems to be refunded.
    /// </summary>
    /// <param name="chargeId">The charge Id</param>
    /// <param name="refund">The refund amount and items</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <param name="idempotencyKey">The optional idempotency key used to avoid calling API endpoint several times</param>
    /// <returns>A refund result</returns>
    Task<RefundResult?> RefundChargeAsync(Guid chargeId, Refund refund, CancellationToken cancellationToken, string? idempotencyKey = null);

    /// <summary>
    /// Retrieves the details of an existing charge operation. The <paramref name="chargeId"/> is obtained from Nets when creating a new charge. The primary usage of this method is to retrieve invoice details of a charge
    /// </summary>
    /// <param name="chargeId">The charge Id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A charge details info</returns>
    Task<ChargeDetailsInfo?> RetrieveChargeAsync(Guid chargeId, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves the details of an existing refund. The refundId is obtained from Nets when creating a new refund. The primary usage of this method is to retrieve invoice details of a refund
    /// </summary>
    /// <param name="refundId">The refund Id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A refund status</returns>
    Task<RetrieveRefund?> RetrieveRefundAsync(Guid refundId, CancellationToken cancellationToken);

    /// <summary>
    /// Terminates an ongoing checkout session. A payment can only be terminated before the checkout has completed (see the payment.checkout event). Use this method to prevent a customer from having multiple open payment sessions simultaneously.
    /// </summary>
    /// <param name="paymentID">The payment ID</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>An awaitable task</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="paymentID"/> is empty</exception>
    Task<bool> TerminatePaymentAsync(Guid paymentID, CancellationToken cancellationToken);

    /// <summary>
    /// Update an existing order for a payment
    /// </summary>
    /// <remarks>
    /// Updates the order for the specified payment. This endpoint makes it possible to change the order on the checkout page after the payment object has been created. This is typically used when managing destination-based shipping costs at the checkout. This endpoint can only be used as long as the checkout has not yet been completed by the customer. (See the payment.checkout.completed event.)
    /// </remarks>
    /// <param name="paymentID">The payment ID</param>
    /// <param name="updates">The order updates</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>An awaitable task</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="paymentID"/> is empty</exception>
    Task<bool> UpdateOrderAsync(Guid paymentID, OrderUpdate updates, CancellationToken cancellationToken);

    /// <summary>
    /// Updates myReference field on payment
    /// </summary>
    /// <param name="paymentId">The payment Id</param>
    /// <param name="myReference">The merchant payment reference</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>An awaitable task that is true on success or false otherwise</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="paymentId"/> is empty</exception>
    /// <seealso cref="PaymentRequest.MyReference"/>
    Task<bool> UpdatePaymentReferenceAsync(Guid paymentId, string? myReference, CancellationToken cancellationToken);

    /// <summary>
    /// Update an existing payments checkout url and reference
    /// </summary>
    /// <remarks>
    /// Updates the specified payment object with a new reference string and a checkoutUrl. If you instead want to update the order of a payment object, use the Update order method.
    /// </remarks>
    /// <param name="payment">The payment object</param>
    /// <param name="checkoutUrl">The checkout url</param>
    /// <param name="reference">The reference</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>An awaitable task</returns>
    Task<bool> UpdateReferences(Payment payment, string checkoutUrl, string reference, CancellationToken cancellationToken);
}