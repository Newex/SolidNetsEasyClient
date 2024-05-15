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
/// Bulk subscription Nets client
/// </summary>
public interface ISubscriptionClient
{
    /// <summary>
    /// Retrieves an existing subscription by a subscriptionId. The
    /// subscriptionId can be obtained from the Retrieve payment method.
    /// </summary>
    /// <param name="subscriptionId">The subscription id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The subscription details or null</returns>
    ValueTask<SubscriptionDetails?> RetrieveSubscription(Guid subscriptionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a subscription matching the specified externalReference. This 
    /// method can only be used for retrieving subscriptions that have been 
    /// imported from a payment platform other than Checkout. Subscriptions 
    /// created within Checkout do not have an externalReference value set.
    /// </summary>
    /// <param name="externalReference">The external reference id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Subscription details or null</returns>
    ValueTask<SubscriptionDetails?> RetrieveSubscriptionByExternalReference(string externalReference, CancellationToken cancellationToken = default);

    /// <summary>
    /// Charges multiple subscriptions at once. The request body must contain: 
    /// A unique string that identifies this bulk charge operation 
    /// A set of subscription identifiers that should be charged. 
    /// To get status updates about the bulk charge you can subscribe to the
    /// webhooks for charges and refunds (payment.charges.* and
    /// payments.refunds.*). See also the webhooks documentation.
    /// </summary>
    /// <param name="externalBulkChargeId">The idempotency identifier, which also identifies this bulk charges.</param>
    /// <param name="subscriptions">The list of subscription to charge.</param>
    /// <param name="notification">The notifications for the webhook callback.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A bulk charge result or null</returns>
    ValueTask<BulkSubscriptionResult?> BulkChargeSubscriptions(string externalBulkChargeId, IList<SubscriptionCharge> subscriptions, Notification notification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves charges associated with the specified bulk charge operation. 
    /// The bulkId is returned from Nexi Group in the response of the Bulk 
    /// charge subscriptions method. 
    /// This method supports pagination. Specify the range of subscriptions to 
    /// retrieve by using either skip and take or pageNumber together with 
    /// pageSize. The boolean property named more in the response body, 
    /// indicates whether there are more subscriptions beyond the requested 
    /// range. 
    /// </summary>
    /// <param name="bulkId">The bulk id</param>
    /// <param name="range">The range. 'range.skip' the number of subscription entries to
    /// skip from the start. Use this property in combination with the take
    /// property.
    /// 'range.take' the maximum number of subscriptions to be retrieved. Use this property
    /// in combination with the 'range.skip' property.
    /// </param>
    /// <param name="page">The page to retrieve. 'page.pageNumber' the page number to be
    /// retrieved. Use this property in combination with the 'page.pageSize' property. 
    /// 'page.pageSize' The size of each page when specify the range of
    /// subscriptions using the pageNumber property.
    /// </param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A paginated result of the bulk subscription status or null</returns>
    ValueTask<PageResult<SubscriptionProcessStatus>?> RetrieveBulkCharges(Guid bulkId,
                                                                          (int skip, int take)? range = null,
                                                                          (int pageNumber, int pageSize)? page = null,
                                                                          CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifies the specified set of subscriptions in bulk. The bulkId returned 
    /// from a successful request can be used for querying the status of the 
    /// subscriptions.
    /// This starts the verification process and can later be queried to view 
    /// the status of the verification.
    /// </summary>
    /// <param name="externalBulkVerificationId">A string that uniquely 
    /// identifies the verification operation. Use this property for enabling 
    /// safe retries. Must be between 1 and 64 characters.</param>
    /// <param name="subscriptions">The set of subscriptions that should be
    /// verified. Each item in the array should define either a subscriptioId or
    /// an externalReference, but not both.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A bulk id result or null</returns>
    ValueTask<BulkSubscriptionResult?> VerifySubscriptions(string externalBulkVerificationId,
                                                           IList<SubscriptionCharge> subscriptions,
                                                           CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves verifications associated with the specified bulk verification 
    /// operation. The bulkId is returned from Nexi Group in the response of the 
    /// Verify subscriptions method. 
    /// 
    /// This method supports pagination. Specify the range of subscriptions to 
    /// retrieve by using either skip and take or pageNumber together with 
    /// pageSize. The boolean property named more in the response body, 
    /// indicates whether there are more subscriptions beyond the requested 
    /// range.
    /// </summary>
    /// <param name="bulkId">The bulk id</param>
    /// <param name="range">The optional range selection</param>
    /// <param name="page">The optional page selection</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A page result of subscription verification statuses or null.</returns>
    ValueTask<PageResult<SubscriptionVerificationStatus>?> RetrieveBulkVerifications(Guid bulkId, (int skip, int take)? range = null, (int pageNumber, int pageSize)? page = null, CancellationToken cancellationToken = default);
}
