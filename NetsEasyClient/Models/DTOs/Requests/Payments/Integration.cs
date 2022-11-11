using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Payments;

/// <summary>
/// An enumeration of integration types
/// </summary>
[JsonConverter(typeof(IntegrationEnumConverter))]
public enum Integration
{
    /// <summary>
    /// Default, where the checkout page will be embedded within an iframe on the website
    /// </summary>
    EmbeddedCheckout,

    /// <summary>
    /// Redirects to NETS website, where the customer can pay
    /// </summary>
    HostedPaymentPage,
}
