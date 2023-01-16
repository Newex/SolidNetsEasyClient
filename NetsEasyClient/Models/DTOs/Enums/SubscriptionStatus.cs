using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Enums;

/// <summary>
/// The current processing status of the subscription. Possible values are: 'Pending', 'Succeeded', and 'Failed'.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SubscriptionStatus
{
    /// <summary>
    /// Pending status
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Succeeded status
    /// </summary>
    Succeeded = 1,

    /// <summary>
    /// Failed status
    /// </summary>
    Failed = 2
}
