using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;
using SolidNetsEasyClient.Models.DTOs.Requests.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;

namespace SolidNetsEasyClient.Clients;

/// <summary>
/// Bulk subscription Nets client
/// </summary>
public interface IBulkSubscriptionClient
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
}
