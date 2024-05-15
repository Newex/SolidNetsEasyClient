using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;

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
    /// <returns>The charge result</returns>
    ValueTask<UnscheduledSubscriptionChargeResult?> ChargeUnscheduledSubscription(Guid unscheduledSubscriptionId, UnscheduledSubscriptionCharge charge, CancellationToken cancellationToken = default);
}
