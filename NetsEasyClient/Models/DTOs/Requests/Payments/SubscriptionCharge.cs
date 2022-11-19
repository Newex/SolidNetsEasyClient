using System;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Payments;

public record SubscriptionCharge
{
    [JsonPropertyName("subscriptionId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(GuidTypeConverter))]
    public Guid? SubscriptionId { get; init; }

    [JsonPropertyName("externalReference")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ExternalReference { get; init; }

    [JsonPropertyName("order")]
    public Order Order { get; init; } = new();
}
