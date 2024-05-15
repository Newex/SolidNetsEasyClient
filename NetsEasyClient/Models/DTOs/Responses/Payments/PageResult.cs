using System.Collections.Generic;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Payments;

/// <summary>
/// A paginated result set of <typeparamref name="T"/>
/// </summary>
/// <typeparam name="T">The subscription process statuses</typeparam>
public record PageResult<T>
{
    /// <summary>
    /// A page of subscription statusses
    /// </summary>
    [JsonPropertyName("page")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IList<T>? Page { get; init; }

    /// <summary>
    /// Indicates whether there are more subscriptions beyond the requested range.
    /// </summary>
    [JsonPropertyName("more")]
    public bool More { get; init; }

    /// <summary>
    /// Indicates whether the operation has completed or is still processing subscriptions. Possible values are 'Done' and 'Processing'.
    /// </summary>
    [JsonPropertyName("status")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BulkStatus? Status { get; init; }
}
