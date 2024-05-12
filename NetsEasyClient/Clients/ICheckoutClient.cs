using System;
using System.Threading;
using System.Threading.Tasks;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;

namespace SolidNetsEasyClient.Clients;

/// <summary>
/// Client for NETS, when customer is about to checkout and pay.
/// </summary>
/// <remarks>
/// <![CDATA[ NETS Easy Payment API (2024): https://developer.nexigroup.com/nexi-checkout/en-EU/api/payment-v1/ ]]> <br />
/// <![CDATA[ Do not use this in a singleton class. See https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory#avoid-typed-clients-in-singleton-services ]]>
/// </remarks>
public interface ICheckoutClient : IDisposable
{
    /// <summary>
    /// The checkout key
    /// </summary>
    string? CheckoutKey { get; }

    /// <summary>
    /// Send the payment request to NETS to initiate a checkout process. 
    /// This should be called before showing the payment page, since the response info should be used on the payment page. 
    /// The checkout can either be a one-time payment, or a subscription.
    /// </summary>
    /// <param name="paymentRequest">The payment request</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The payment result</returns>
    /// <exception cref="InvalidOperationException">Thrown if invalid payment request.</exception>
    ValueTask<PaymentResult?> StartCheckoutPayment(PaymentRequest paymentRequest, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieve payment details for a payment.
    /// </summary>
    /// <param name="paymentId">The payment id</param>
    /// <param name="cancellationToken">The optional cancellation token</param>
    /// <returns>Payment detail or null</returns>
    ValueTask<PaymentStatus?> RetrievePaymentDetails(Guid paymentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the specified payment object with a new reference string and a
    /// checkoutUrl. If you instead want to update the order of a payment
    /// object, use the <see cref="UpdateOrderBeforeCheckout(Guid, OrderUpdate, CancellationToken)"/>
    /// </summary>
    /// <param name="paymentId">The payment id</param>
    /// <param name="references">The new updated reference information</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>True if updated references otherwise false</returns>
    ValueTask<bool> UpdateReferenceInformationBeforeCheckout(Guid paymentId, ReferenceInformation references, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update order, before final payment. Updates the order for the specified
    /// payment. This endpoint makes it possible to change the order on the
    /// checkout page after the payment object has been created. This is
    /// typically used when managing destination-based shipping costs at the
    /// checkout. This endpoint can only be used as long as the checkout has not
    /// yet been completed by the customer. (See the payment.checkout.completed
    /// event.)
    /// </summary>
    /// <param name="paymentId">The payment id</param>
    /// <param name="update">The order updates</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>True if updated otherwise false</returns>
    ValueTask<bool> UpdateOrderBeforeCheckout(Guid paymentId, OrderUpdate update, CancellationToken cancellationToken = default);

    /// <summary>
    /// Terminate payment before checkout is completed.
    /// Terminates an ongoing checkout session. A payment can only be terminated
    /// before the checkout has completed (see the payment.checkout event). Use
    /// this method to prevent a customer from having multiple open payment
    /// sessions simultaneously.
    /// </summary>
    /// <param name="paymentId">The payment id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>True if payment has been terminated otherwise false</returns>
    ValueTask<bool> TerminatePaymentBeforeCheckout(Guid paymentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels the specified payment. When a payment is canceled, the reserved
    /// amount of the payment will be released to the customer's payment card.
    /// </summary>
    /// <remarks>
    /// Only full cancels are allowed. The amount must always match the total
    /// amount of the order.
    /// Once a payment has been charged (fully or partially), the payment cannot
    /// be canceled.
    /// It is not possible to change the status of a payment once it has been
    /// canceled.
    /// Nexi Group will not charge a fee for a canceled payment.
    /// </remarks>
    /// <param name="paymentId">The payment id</param>
    /// <param name="order">The order cancellation</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>True if order has been canceled otherwise false</returns>
    ValueTask<bool> CancelPaymentBeforeCharge(Guid paymentId, CancelOrder order, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates myReference field on payment. The myReference can be used if you 
    /// want to create a myReference ID that can be used in your own accounting 
    /// system to keep track of the actions connected to the payment.
    /// </summary>
    /// <param name="paymentId">The payment id.</param>
    /// <param name="myReference">The updated reference for the payment.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if reference has been updated otherwise false.</returns>
    ValueTask<bool> UpdateMyReference(Guid paymentId, PaymentReference myReference, CancellationToken cancellationToken = default);
}
