using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Enums;

/// <summary>
/// Indicates whether the operation has completed or is still processing subscriptions. Possible values are 'Done' and 'Processing'.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BulkStatus
{
    /// <summary>
    /// Bulk processing has completed
    /// </summary>
    Done,

    /// <summary>
    /// Bulk processing is still processing
    /// </summary>
    Processing
}