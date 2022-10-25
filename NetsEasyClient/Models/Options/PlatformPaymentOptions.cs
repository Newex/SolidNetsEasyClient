namespace SolidNetsEasyClient.Models.Options;

/// <summary>
/// The platform payment options
/// </summary>
public record PlatformPaymentOptions
{
    /// <summary>
    /// The secret API key
    /// </summary>
    public string Authorization { get; set; } = string.Empty;

    /// <summary>
    /// An identifier of the ecommerce platform
    /// </summary>
    public string? CommercePlatformTag { get; set; }
}
