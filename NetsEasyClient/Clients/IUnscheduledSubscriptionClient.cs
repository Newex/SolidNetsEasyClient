using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
}
