using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Payments;

/// <summary>
/// A summary of the payment request
/// </summary>
public record Summary
{
    /// <summary>
    /// The amount that has been reserved in the customer's bank account at the time of the purchase to make sure there are sufficient funds to charge the payment
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("reservedAmount")]
    public int? ReservedAmount { get; init; }

    /// <summary>
    /// The charged amount
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("chargedAmount")]
    public int? ChargedAmount { get; init; }

    /// <summary>
    /// The amount that has been refunded
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("refundedAmount")]
    public int? RefundedAmount { get; init; }

    /// <summary>
    /// The amount that has been cancelled
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("cancelledAmount")]
    public int? CancelledAmount { get; init; }
}
