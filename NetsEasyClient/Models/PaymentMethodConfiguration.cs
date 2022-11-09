using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models;

/// <summary>
/// Specifies payment method configuration to be used for this payment
/// </summary>
public record PaymentMethodConfiguration
{
    /// <summary>
    /// The name of the payment method or payment type to be configured, if the specified payment method is not configured correctly in the merchant configurations then this won't take effect. Payment type cannot be specified alongside payment methods that belong to it, if it happens the request will fail with an error
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("name")]
    public PaymentTypeMethodName? Name { get; init; }

    /// <summary>
    /// Indicates that the specified payment method/type is allowed to be used for this payment, defaults to true. If one or more payment method/type is configured in the parent array then this value will be considered false for any other payment method that the parent array doesn't cover
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool Enabled { get; init; } = true;
}
