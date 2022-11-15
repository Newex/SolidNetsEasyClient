using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Payments;

/// <summary>
/// Card details information
/// </summary>
public record CardDetailsInfo
{
    /// <summary>
    /// A masked version of the PAN (Primary Account Number). At maximum, only the first six and last four digits of the account number are displayed
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("maskedPan")]
    public string? MaskedPan { get; init; }

    /// <summary>
    /// The four-digit expiration date of the payment card. The format should be: MMYY
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("expiryDate")]
    public MonthOnly? ExpiryDate { get; init; }
}
