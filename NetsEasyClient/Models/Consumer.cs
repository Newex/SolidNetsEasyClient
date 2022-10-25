using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models;

/// <summary>
/// Contains information about the customer. If provided, this information will be used for initating the consumer data of the payment object
/// </summary>
public record Consumer
{
    /// <summary>
    /// The consumer reference, i.e. the user ID
    /// </summary>
    /// <example>
    /// user-id-2034
    /// </example>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("reference")]
    public string? Reference { get; init; }

    /// <summary>
    /// The email address
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("email")]
    public string? Email { get; init; }

    /// <summary>
    /// The address of a customer (private or business)
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("shippingAddress")]
    public ShippingAddress? ShippingAddress { get; init; }

    /// <summary>
    /// An international phone number
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("phoneNumber")]
    public PhoneNumber? PhoneNumber { get; init; }

    /// <summary>
    /// The name of a natural person
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("privatePerson")]
    public Person? PrivatePerson { get; init; }

    /// <summary>
    /// A business consumer
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("company")]
    public Company? Company { get; init; }
}
