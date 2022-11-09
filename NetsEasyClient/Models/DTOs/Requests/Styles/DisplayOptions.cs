using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Styles;

/// <summary>
/// Options for what is displayed on the checkout page
/// </summary>
public record DisplayOptions
{
    /// <summary>
    /// Determines if the merchant name should be displayed
    /// </summary>
    /// <remarks>
    /// If set to true, displays the merchant name above the checkout. Default value is true when using a <see cref="Integration.HostedPaymentPage"/>
    /// </remarks>
    [JsonPropertyName("showMerchantName")]
    public bool ShowMerchantName { get; init; }

    /// <summary>
    /// Determines if the order summary should be displayed in the checkout page
    /// </summary>
    /// <remarks>
    /// If set to true, displays the order summary above the checkout. Default value is true when using a <see cref="Integration.HostedPaymentPage"/>
    /// </remarks>
    [JsonPropertyName("showOrderSummary")]
    public bool ShowOrderSummary { get; init; }
}
