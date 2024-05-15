using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
}
