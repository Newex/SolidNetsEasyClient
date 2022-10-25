using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models.Status;

/// <summary>
/// Refund states
/// </summary>
[JsonConverter(typeof(RefundStateConverter))]
public enum RefundStateEnum
{
    /// <summary>
    /// Pending refund
    /// </summary>
    Pending,

    /// <summary>
    /// Refund cancelled
    /// </summary>
    Cancelled,

    /// <summary>
    /// Refund failed
    /// </summary>
    Failed,

    /// <summary>
    /// Refund completed
    /// </summary>
    Completed,

    /// <summary>
    /// Refund expired
    /// </summary>
    Expired
}
