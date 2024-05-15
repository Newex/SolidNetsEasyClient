using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Contacts;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments.Subscriptions;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;

/// <summary>
/// The bulk subscription response, for a bulk charge operation.
/// </summary>
public record BulkSubscriptionResult : BaseBulkResult
{
    /// <summary>
    /// An international phone number.
    /// </summary>
    [JsonPropertyName("phoneNumber")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PhoneNumber? PhoneNumber { get; init; }
}