using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;
using SolidNetsEasyClient.Models.DTOs.Requests.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;

namespace SolidNetsEasyClient.Clients;

/// <summary>
/// Unscheduled subscription client, responsible for mediating communication between NETS payment API and this client
/// </summary>
/// <remarks>
/// Unscheduled subscriptions allow you to charge your customers at an unscheduled time interval with a variable amount, for example an automatic top-up agreement for a rail-card when the consumer drops below a certain stored value. When an unscheduled subscription is charged, a new payment object is created to represent the purchase of the unscheduled subscription product. It is possible to verify and charge multiple unscheduled subscriptions in bulk using the Bulk charge unscheduled subscriptions method.
/// </remarks>
public interface IUnscheduledSubscriptionClient
{
    /// <summary>
    /// Charges multiple unscheduled subscriptions at once. The request body must contain: (*) A unique string that identifies this bulk charge operation; (*) A set of unscheduled subscription identifiers that should be charged.; To get status updates about the bulk charge you can subscribe to the webhooks for charges and refunds (payment.charges.* and payments.refunds.*). See also the webhooks documentation.
    /// </summary>
    /// <param name="bulk">The array of unscheduled subscriptions that should be charged. Each item in the array should define either a subscriptionId or an externalReference, but not both.</param>
    /// <param name="externalBulkChargeId">A string that uniquely identifies the bulk charge operation. Use this property for enabling safe retries. Must be between 1 and 64 characters.</param>
    /// <param name="notifications">Notifications allow you to subscribe to status updates for a payment.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A bulk id</returns>
    /// <exception cref="ArgumentException">Thrown if argument is invalid</exception>
    /// <exception cref="SerializationException">Thrown if response is successfull but cannot be serialized to expected result</exception>
    /// <exception cref="HttpRequestException">Thrown if response is not successful</exception>
    Task<BulkId> BulkChargeUnscheduledSubscriptionsAsync(IList<ChargeUnscheduledSubscription> bulk, string externalBulkChargeId, Notification? notifications, CancellationToken cancellationToken);

    /// <summary>
    /// Charges multiple unscheduled subscriptions at once. The request body must contain: (*) A unique string that identifies this bulk charge operation; (*) A set of unscheduled subscription identifiers that should be charged.; To get status updates about the bulk charge you can subscribe to the webhooks for charges and refunds (payment.charges.* and payments.refunds.*). See also the webhooks documentation.
    /// </summary>
    /// <param name="bulk">The bulk</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A bulk id</returns>
    /// <exception cref="ArgumentException">Thrown if argument is invalid</exception>
    /// <exception cref="SerializationException">Thrown if response is successfull but cannot be serialized to expected result</exception>
    /// <exception cref="HttpRequestException">Thrown if response is not successful</exception>
    Task<BulkId> BulkChargeUnscheduledSubscriptionsAsync(BulkUnscheduledSubscriptionCharge bulk, CancellationToken cancellationToken);

    /// <summary>
    /// Charges a single unscheduled subscription. The unscheduledSubscriptionId can be obtained from the Retrieve payment method. On success, this method creates a new payment object and performs a charge of the specified amount. Both the new paymentId and chargeId are returned in the response body.
    /// </summary>
    /// <param name="unscheduledSubscriptionId">The unscheduled subscription identifier (a UUID) returned from the Retrieve payment method.</param>
    /// <param name="order">Specifies an order associated with a payment. An order must contain at least one order item. The amount of the order must match the sum of the specified order items.</param>
    /// <param name="notifications">Notifications allow you to subscribe to status updates for a payment.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A result containing the payment and charge id</returns>
    /// <exception cref="ArgumentException">Thrown if argument is invalid</exception>
    /// <exception cref="SerializationException">Thrown if response is successfull but cannot be serialized to expected result</exception>
    /// <exception cref="HttpRequestException">Thrown if response is not successful</exception>
    Task<UnscheduledSubscriptionChargeResult> ChargeUnscheduledSubscriptionAsync(Guid unscheduledSubscriptionId, Order order, Notification notifications, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves charges associated with the specified bulk charge operation. The bulkId is returned from Nets in the response of the Bulk charge unscheduled subscriptions method. This method supports pagination. Specify the range of subscriptions to retrieve by using either skip and take or pageNumber together with pageSize. The boolean property named more in the response body, indicates whether there are more subscriptions beyond the requested range.
    /// </summary>
    /// <param name="bulkId">The identifier of the bulk charge operation that was returned from the Bulk charge unscheduled subscriptions method.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A paginated result set</returns>
    /// <exception cref="ArgumentException">Thrown if argument is invalid</exception>
    /// <exception cref="SerializationException">Thrown if response is successfull but cannot be serialized to expected result</exception>
    /// <exception cref="HttpRequestException">Thrown if response is not successful</exception>
    Task<PageResult<UnscheduledSubscriptionProcessStatus>> RetrieveBulkUnscheduledSubscriptionChargesAsync(Guid bulkId, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves charges associated with the specified bulk charge operation. The bulkId is returned from Nets in the response of the Bulk charge unscheduled subscriptions method. This method supports pagination. Specify the range of subscriptions to retrieve by using either skip and take or pageNumber together with pageSize. The boolean property named more in the response body, indicates whether there are more subscriptions beyond the requested range.
    /// </summary>
    /// <param name="bulkId">The identifier of the bulk charge operation that was returned from the Bulk charge unscheduled subscriptions method.</param>
    /// <param name="skip">The number of subscription entries to skip from the start. Use this property in combination with the take property.</param>
    /// <param name="take">The maximum number of subscriptions to be retrieved. Use this property in combination with the skip property.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A paginated result set</returns>
    /// <exception cref="ArgumentException">Thrown if argument is invalid</exception>
    /// <exception cref="SerializationException">Thrown if response is successfull but cannot be serialized to expected result</exception>
    /// <exception cref="HttpRequestException">Thrown if response is not successful</exception>
    Task<PageResult<UnscheduledSubscriptionProcessStatus>> RetrieveBulkUnscheduledSubscriptionChargesAsync(Guid bulkId, int skip, int take, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves charges associated with the specified bulk charge operation. The bulkId is returned from Nets in the response of the Bulk charge unscheduled subscriptions method. This method supports pagination. Specify the range of subscriptions to retrieve by using either skip and take or pageNumber together with pageSize. The boolean property named more in the response body, indicates whether there are more subscriptions beyond the requested range.
    /// </summary>
    /// <param name="bulkId">The identifier of the bulk charge operation that was returned from the Bulk charge unscheduled subscriptions method.</param>
    /// <param name="pageNumber">The page number to be retrieved. Use this property in combination with the pageSize property.</param>
    /// <param name="pageSize">The size of each page when specify the range of subscriptions using the pageNumber property.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A paginated result set</returns>
    /// <exception cref="ArgumentException">Thrown if argument is invalid</exception>
    /// <exception cref="SerializationException">Thrown if response is successfull but cannot be serialized to expected result</exception>
    /// <exception cref="HttpRequestException">Thrown if response is not successful</exception>
    Task<PageResult<UnscheduledSubscriptionProcessStatus>> RetrieveBulkUnscheduledSubscriptionChargesAsync(Guid bulkId, int pageNumber, ushort pageSize, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves verifications associated with the specified bulk unscheduled verification operation. The bulkId is returned from Nets in the response of the Verify unscheduled subscriptions method. This method supports pagination. Specify the range of subscriptions to retrieve by using either skip and take or pageNumber together with pageSize. The boolean property named more in the response body, indicates whether there are more subscriptions beyond the requested range.
    /// </summary>
    /// <param name="bulkId">The identifier of the bulk verification operation that was returned from the Verify unscheduled subscriptions method.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <param name="skip">The number of unscheduled subscription entries to skip from the start. Use this property in combination with the take property.</param>
    /// <param name="take">The maximum number of unscheduled subscriptions to be retrieved. Use this property in combination with the skip property.</param>
    /// <param name="pageNumber">The page number to be retrieved. Use this property in combination with the pageSize property.</param>
    /// <param name="pageSize">The size of each page when specify the range of unscheduled subscriptions using the pageNumber property.</param>
    /// <returns>A page result set of unscheduled subscription process statuses</returns>
    /// <exception cref="ArgumentException">Thrown if argument is invalid</exception>
    /// <exception cref="SerializationException">Thrown if response is successfull but cannot be serialized to expected result</exception>
    /// <exception cref="HttpRequestException">Thrown if response is not successful</exception>
    Task<PageResult<UnscheduledSubscriptionProcessStatus>> RetrieveBulkVerificationsAsync(Guid bulkId, CancellationToken cancellationToken, int? skip = null, int? take = null, int? pageNumber = null, int? pageSize = null);

    /// <summary>
    /// Retrieves an unscheduled subscription matching the specified externalReference. This method can only be used for retrieving unscheduled subscriptions that have been imported from a payment platform other than Nets Easy. Unscheduled subscriptions created within Nets Easy do not have an externalReference value set.
    /// </summary>
    /// <param name="externalReference">The external reference to search for.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Information about the unscheduled subscription</returns>
    /// <exception cref="ArgumentException">Thrown if external reference is empty or null</exception>
    /// <exception cref="HttpRequestException">Thrown if response is not successfull</exception>
    Task<UnscheduledSubscriptionDetails> RetrieveUnscheduledSubscriptionByExternalReferenceAsync(string externalReference, CancellationToken cancellationToken);

    /// <summary>
    /// Verifies the specified set of unscheduled subscriptions in bulk. The bulkId returned from a successful request can be used for querying the status of the unscheduled subscriptions.
    /// </summary>
    /// <param name="subscriptions">The set of unscheduled subscriptions that should be verified. Each item in the array should define either a unscheduledSubscriptionId or an externalReference, but not both.</param>
    /// <param name="externalBulkVerificationId">A string that uniquely identifies the verification operation. Use this property for enabling safe retries. Must be between 1 and 64 characters.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A bulk id</returns>
    /// <exception cref="ArgumentException">Thrown if argument is invalid</exception>
    /// <exception cref="SerializationException">Thrown if response is successfull but cannot be serialized to expected result</exception>
    /// <exception cref="HttpRequestException">Thrown if response is not successful</exception>
    Task<BulkId> VerifyBulkSubscriptionsAsync(IList<UnscheduledSubscriptionInfo> subscriptions, string externalBulkVerificationId, CancellationToken cancellationToken);
}
