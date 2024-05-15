using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;
using SolidNetsEasyClient.Models.DTOs.Requests.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments.Subscriptions;

namespace SolidNetsEasyClient.Clients;

/// <summary>
/// Unscheduled subscription client. 
/// An unscheduled subscription is created during checkout. 
/// The difference between an unscheduled subscription and a regular 
/// subscription is that a regular subscription does not vary the charged 
/// amount or the charge date.
/// </summary>
/// <remarks>
/// <![CDATA[ Nexi Checkout API (2024): https://developer.nexigroup.com/nexi-checkout/en-EU/api/payment-v1/ ]]> <br />
/// <![CDATA[ Do not use this in a singleton class. See https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory#avoid-typed-clients-in-singleton-services ]]>
/// </remarks>
public interface IUnscheduledSubscriptionClient
{
    /// <summary>
    /// Retrieves an existing unscheduled subscription by a 
    /// unscheduledSubscriptionId. The unscheduledSubscriptionId can be obtained 
    /// from the Retrieve payment method. 
    /// </summary>
    /// <param name="unscheduledSubscriptionId">The unscheduled subscription id. 
    /// The unscheduledSubscriptionId can be obtained from the Retrieve payment 
    /// method.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A details about an unscheduled subscription or null</returns>
    ValueTask<UnscheduledSubscriptionDetails?> RetrieveUnscheduledSubscription(Guid unscheduledSubscriptionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an unscheduled subscription matching the specified 
    /// externalReference. This method can only be used for retrieving 
    /// unscheduled subscriptions that have been imported from a payment 
    /// platform other than Checkout. Unscheduled subscriptions created within 
    /// Checkout do not have an externalReference value set.
    /// </summary>
    /// <param name="externalReference">The external reference</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A details about an unscheduled subscription or null</returns>
    ValueTask<UnscheduledSubscriptionDetails?> RetrieveUnscheduledSubscriptionByExternalReference(string externalReference, CancellationToken cancellationToken = default);

    /// <summary>
    /// Charges a single unscheduled subscription. The unscheduledSubscriptionId 
    /// can be obtained from the Retrieve payment method. On success, this 
    /// method creates a new payment object and performs a charge of the 
    /// specified amount. Both the new paymentId and chargeId are returned in 
    /// the response body.
    /// </summary>
    /// <param name="unscheduledSubscriptionId">The unscheduled subscription id</param>
    /// <param name="charge">The charge</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The charge result or null</returns>
    ValueTask<UnscheduledSubscriptionChargeResult?> ChargeUnscheduledSubscription(Guid unscheduledSubscriptionId, UnscheduledSubscriptionCharge charge, CancellationToken cancellationToken = default);

    /// <summary>
    /// Charges multiple unscheduled subscriptions at once. The request body must contain: 
    /// A unique string that identifies this bulk charge operation 
    /// A set of unscheduled subscription identifiers that should be charged. 
    /// To get status updates about the bulk charge you can subscribe to the 
    /// webhooks for charges and refunds (payment.charges.* and 
    /// payments.refunds.*). See also the webhooks documentation.
    /// </summary>
    /// <param name="externalBulkChargeId">The external bulk charge id. A string 
    /// that uniquely identifies the bulk charge operation. Use this property 
    /// for enabling safe retries. Must be between 1 and 64 characters.</param>
    /// <param name="charges">The unscheduled subscriptions to charge. The array 
    /// of unscheduled subscriptions that should be charged. Each item in the 
    /// array should define either a subscriptionId or an externalReference, but 
    /// not both.</param>
    /// <param name="notifications">Notifications allow you to subscribe to 
    /// status updates for a payment.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A bulk id or null</returns>
    ValueTask<BaseBulkResult?> BulkChargeUnscheduledSubscriptions(string externalBulkChargeId,
                                                                  IList<ChargeUnscheduledSubscription> charges,
                                                                  Notification? notifications = null,
                                                                  CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves charges associated with the specified bulk charge operation. 
    /// The bulkId is returned from Nexi Group in the response of the Bulk 
    /// charge unscheduled subscriptions method. 
    /// This method supports pagination. Specify the range of subscriptions to 
    /// retrieve by using either skip and take or pageNumber together with 
    /// pageSize. The boolean property named more in the response body, 
    /// indicates whether there are more subscriptions beyond the requested 
    /// range.
    /// </summary>
    /// <param name="bulkId">The bulk id. The identifier of the bulk charge 
    /// operation that was returned from the Bulk charge unscheduled 
    /// subscriptions method.</param>
    /// <param name="range">The optional range to skip or take.</param>
    /// <param name="page">The optional page number and size to take.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A page of unscheduled subscription statuses or null.</returns>
    ValueTask<PageResult<UnscheduledSubscriptionProcessStatus>?> RetrieveBulkUnscheduledCharges(Guid bulkId,
                                                                                                (int skip, int take)? range = null,
                                                                                                (int pageNumber, int pageSize)? page = null,
                                                                                                CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifies the specified set of unscheduled subscriptions in bulk. The 
    /// bulkId returned from a successful request can be used for querying the 
    /// status of the unscheduled subscriptions.
    /// </summary>
    /// <param name="externalBulkVerificationId">The idempotency key. A string 
    /// that uniquely identifies the verification operation. Use this property 
    /// for enabling safe retries. Must be between 1 and 64 characters.</param>
    /// <param name="subscriptions">The set of unscheduled subscriptions that 
    /// should be verified. Each item in the array should define either a 
    /// unscheduledSubscriptionId or an externalReference, but not both.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A bulk id result or null</returns>
    ValueTask<BaseBulkResult?> VerifyCardsForUnscheduledSubscriptions(string externalBulkVerificationId, IList<UnscheduledSubscriptionInfo> subscriptions, CancellationToken cancellationToken = default);
}
