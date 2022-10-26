using SolidNetsEasyClient.Constants;

namespace SolidNetsEasyClient.Models.Options;

/// <summary>
/// The platform payment options
/// </summary>
public record PlatformPaymentOptions
{
    /// <summary>
    /// The http client mode, which can be either in test mode or in live mode
    /// </summary>
    public ClientMode ClientMode { get; set; }

    /// <summary>
    /// The secret API key
    /// </summary>
    public string Authorization { get; set; } = string.Empty;

    /// <summary>
    /// An identifier of the ecommerce platform
    /// </summary>
    public string? CommercePlatformTag { get; set; }
}
