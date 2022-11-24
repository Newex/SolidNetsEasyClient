using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;
using SolidNetsEasyClient.Models.DTOs.Requests.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;

namespace SolidNetsEasyClient.Clients;

/// <summary>
/// Subscription client, responsible for mediating communication between NETS payment API and this client
/// </summary>
/// <remarks>
/// Subscriptions allow you to charge your customers on a regular basis, for example a monthly subscription for a product the customer must pay for every month. When a subscription is charged, a new payment object is created to represent the purchase of the subscription product. It is possible to verify and charge multiple subscriptions in bulk using the Bulk charge subscriptions method.
/// </remarks>
public interface ISubscriptionClient
{
    /// <summary>
    /// Charges multiple subscriptions at once. The request body must contain: (*) A unique string that identifies this bulk charge operation. (*) A set of subscription identifiers that should be charged. To get status updates about the bulk charge you can subscribe to the webhooks for charges and refunds (payment.charges.* and payments.refunds.*). See also the webhooks documentation.
    /// </summary>
    /// <param name="subscriptions">The list of subscriptions to charge in bulk</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <param name="externalBulkChargeId">A string that uniquely identifies the bulk charge operation. Use this property for enabling safe retries. Must be between 1 and 64 characters.</param>
    /// <param name="notifications">Subscribe to notifications</param>
    /// <returns>A bulk id</returns>
    /// <exception cref="ArgumentException">Thrown if subscriptions or external bulk charge id is invalid</exception>
    Task<BulkId> BulkChargeSubscriptionsAsync(IList<SubscriptionCharge> subscriptions, CancellationToken cancellationToken, string? externalBulkChargeId = null, Notification? notifications = null);

    /// <summary>
    /// Retrieves charges associated with the specified bulk charge operation. The bulkId is returned from Nets in the response of the Bulk charge subscriptions method.
    /// </summary>
    /// <param name="bulkId">The identifier of the bulk charge operation that was returned from the Bulk charge subscriptions method.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A paginated set of subscriptions</returns>
    Task<PaginatedSubscriptions> RetrieveBulkChargesAsync(Guid bulkId, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves charges associated with the specified bulk charge operation. The bulkId is returned from Nets in the response of the Bulk charge subscriptions method. This method supports pagination. Specify the range of subscriptions to retrieve by using either skip and take. The boolean property named more in the response body, indicates whether there are more subscriptions beyond the requested range.
    /// </summary>
    /// <param name="bulkId">The bulk id</param>
    /// <param name="skip">The number of subscription entries to skip from the start. Use this property in combination with the take property.</param>
    /// <param name="take">The maximum number of subscriptions to be retrieved. Use this property in combination with the skip property.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A paginated set of subscriptions</returns>
    Task<PaginatedSubscriptions> RetrieveBulkChargesAsync(Guid bulkId, int skip, int take, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves charges associated with the specified bulk charge operation. The bulkId is returned from Nets in the response of the Bulk charge subscriptions method. This method supports pagination. Specify the range of subscriptions to retrieve by using pageNumber together with pageSize. The boolean property named more in the response body, indicates whether there are more subscriptions beyond the requested range.
    /// </summary>
    /// <param name="bulkId">The identifier of the bulk charge operation that was returned from the Bulk charge subscriptions method.</param>
    /// <param name="pageSize">The size of each page when specify the range of subscriptions using the pageNumber property.</param>
    /// <param name="pageNumber">The page number to be retrieved. Use this property in combination with the pageSize property.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A paginated set of subscriptions</returns>
    Task<PaginatedSubscriptions> RetrieveBulkChargesAsync(Guid bulkId, int pageSize, ushort pageNumber, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves verifications associated with the specified bulk verification operation. The bulkId is returned from Nets in the response of the Verify subscriptions method. This method supports pagination. Specify the range of subscriptions to retrieve by using either skip and take or pageNumber together with pageSize. The boolean property named more in the response body, indicates whether there are more subscriptions beyond the requested range.
    /// </summary>
    /// <param name="bulkId">The identifier of the bulk verification operation that was returned from the erfiy subscriptions method.</param>
    /// <param name="skip">The number of subscription entries to skip from the start. Use this property in combination with the take property.</param>
    /// <param name="take">The maximum number of subscriptions to be retrieved. Use this property in combination with the skip property.</param>
    /// <param name="pageNumber">The page number to be retrieved. Use this property in combination with the pageSize property.</param>
    /// <param name="pageSize">The size of each page when specify the range of subscriptions using the pageNumber property.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A paginated result set of subscriptions</returns>
    /// <exception cref="ArgumentException">Thrown if bulk id is empty or if skip, take, pageNumber or pageSize is negative</exception>
    Task<PaginatedSubscriptions> RetrieveBulkVerificationsAsync(Guid bulkId, int? skip, int? take, int? pageNumber, int? pageSize, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves an existing subscription by a subscriptionId. The subscriptionId can be obtained from the Retrieve payment method.
    /// </summary>
    /// <param name="subscriptionId">The subscription identifier (a UUID).</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Details of a subscription</returns>
    /// <exception cref="ArgumentException">Thrown if subscription id is empty</exception>
    Task<SubscriptionDetails> RetrieveSubscriptionAsync(Guid subscriptionId, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a subscription matching the specified externalReference. This method can only be used for retrieving subscriptions that have been imported from a payment platform other than Nets Easy. Subscriptions created within Nets Easy do not have an externalReference value set.
    /// </summary>
    /// <param name="externalReference">The external reference to search for.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Details of a subscription</returns>
    /// <exception cref="ArgumentException">Thrown if the external reference is null or only contains whitespaces</exception>
    Task<SubscriptionDetails> RetrieveSubscriptionByExternalReferenceAsync(string externalReference, CancellationToken cancellationToken);

    /// <summary>
    /// Verifies the specified set of subscriptions in bulk. The bulkId returned from a successful request can be used for querying the status of the subscriptions.
    /// </summary>
    /// <param name="verifications">The set of subscriptions that should be verified. Each item in the array should define either a subscriptioId or an externalReference, but not both.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A bulk id</returns>
    /// <exception cref="ArgumentException">Thrown if invalid subscriptions or external bulk verification id</exception>
    Task<BulkId> VerifyBulkSubscriptionsAsync(BulkSubscriptionVerification verifications, CancellationToken cancellationToken);
}
