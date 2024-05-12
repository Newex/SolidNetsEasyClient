using System;
using System.Threading;
using System.Threading.Tasks;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;

namespace SolidNetsEasyClient.Clients;

/// <summary>
/// Client for NETS, when you have charged the payment or need to refund the charge.
/// </summary>
/// <remarks>
/// <![CDATA[ NETS Easy Payment API (2024): https://developer.nexigroup.com/nexi-checkout/en-EU/api/payment-v1/ ]]> <br />
/// <![CDATA[ Do not use this in a singleton class. See https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory#avoid-typed-clients-in-singleton-services ]]>
/// </remarks>
public interface IChargeClient : IDisposable
{
    /// <summary>
    /// Charges the specified payment. Charge a payment on the same day as you 
    /// ship the matching order.A payment can be fully charged or partially 
    /// charged. 
    /// Full charge: Your customer will be charged the total amount of the 
    /// payment. The amount must be specified in the request body and is 
    /// required to match the total amount of the payment. 
    /// Partial charge: Only charge for a subset of the order items. In this 
    /// case you have to provide the amount and the orderItems you want to 
    /// charge in the request body.
    /// </summary>
    /// <param name="paymentId">The payment id</param>
    /// <param name="charge">The payment to charge</param>
    /// <param name="idempotencyKey">To ensure you only call the charge once. A 
    /// string that uniquely identifies the charge you are attempting. Must be 
    /// between 1 and 64 characters.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The result of the charge or null</returns>
    ValueTask<ChargeResult?> ChargePayment(Guid paymentId, Charge charge, string? idempotencyKey = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the details of an existing charge operation. The chargeId is
    /// obtained from Nexi Group when creating a new charge. The primary usage
    /// of this method is to retrieve invoice details of a charge.
    /// </summary>
    /// <param name="chargeId">The charge id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Details about a charge or null</returns>
    ValueTask<ChargeDetailsInfo?> RetrieveCharge(Guid chargeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Refunds a previously settled transaction (a charged payment). The 
    /// refunded amount will be transferred back to the customer's account. The 
    /// required chargeId is returned from the Charge payment method. 
    /// A settled transaction can be fully or partially refunded: 
    /// Full refund requires only the amount to be specified in the request body. 
    /// Partial refund requires the amount and the orderItems to be refunded.
    /// </summary>
    /// <param name="chargeId">The charge id</param>
    /// <param name="charge">The charge to refund</param>
    /// <param name="idempotencyKey">To ensure you only call the charge once. A 
    /// string that uniquely identifies the charge you are attempting. Must be 
    /// between 1 and 64 characters.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A refund result</returns>
    ValueTask<RefundResult?> RefundCharge(Guid chargeId, CancelOrder charge, string? idempotencyKey = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Refunds a previously settled payment. The refunded amount will be transferred back to the customer's account. 
    /// A settled payment can be fully or partially refunded, this end-point is not supported for these payment methods: 
    /// Arvato, PayPal, RatePayInvoice, RatePaySepa, RatePayInstallment, EasyInvoice, EasyCampaign, EasyInstallment.
    /// </summary>
    /// <param name="paymentId">The payment id.</param>
    /// <param name="order">The order to refund.</param>
    /// <param name="idempotencyKey">To ensure you only call the charge once. A 
    /// string that uniquely identifies the charge you are attempting. Must be 
    /// between 1 and 64 characters.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A refund result</returns>
    ValueTask<RefundResult?> RefundPayment(Guid paymentId, CancelOrder order, string? idempotencyKey = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the details of an existing refund. The refundId is obtained 
    /// from Nexi Group when creating a new refund. The primary usage of this 
    /// method is to retrieve invoice details of a refund.
    /// </summary>
    /// <param name="refundId">The refund id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Refund details or null</returns>
    ValueTask<RetrieveRefund?> RetrieveRefund(Guid refundId, CancellationToken cancellationToken = default);
}
