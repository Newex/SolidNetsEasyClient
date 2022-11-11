using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs.Contacts;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

/// <summary>
/// The checkout consumer
/// </summary>
public record CheckoutCompletedConsumer
{
    /// <summary>
    /// The first name of the customer
    /// </summary>
    [JsonPropertyName("firstName")]
    public string FirstName { get; init; } = string.Empty;

    /// <summary>
    /// The last name of the customer
    /// </summary>
    [JsonPropertyName("lastName")]
    public string LastName { get; init; } = string.Empty;

    /// <summary>
    /// The billing address
    /// </summary>
    [JsonPropertyName("billingAddress")]
    public CheckoutCompletedAddress BillingAddress { get; init; } = new();

    /// <summary>
    /// The country
    /// </summary>
    [JsonPropertyName("country")]
    public string Country { get; init; } = string.Empty;

    /// <summary>
    /// The email
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// The IP
    /// </summary>
    [JsonPropertyName("ip")]
    public string IP { get; init; } = string.Empty;

    /// <summary>
    /// The phone number
    /// </summary>
    [JsonPropertyName("phoneNumber")]
    public PhoneNumber PhoneNumber { get; init; } = new();

    /// <summary>
    /// The shipping address
    /// </summary>
    [JsonPropertyName("shippingAddress")]
    public CheckoutCompletedAddress ShippingAddress { get; init; } = new();
}
