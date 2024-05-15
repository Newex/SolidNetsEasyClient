using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.DTOs.Enums;

/// <summary>
/// Refund states
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<RefundStateEnum>))]
public enum RefundStateEnum
{
    /// <summary>
    /// Pending refund
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Refund cancelled
    /// </summary>
    Cancelled = 1,

    /// <summary>
    /// Refund failed
    /// </summary>
    Failed = 2,

    /// <summary>
    /// Refund completed
    /// </summary>
    Completed = 3,

    /// <summary>
    /// Refund expired
    /// </summary>
    Expired = 4
}
