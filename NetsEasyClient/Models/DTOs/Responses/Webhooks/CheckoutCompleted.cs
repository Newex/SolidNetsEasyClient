using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Contacts;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

/// <summary>
/// A web hook DTO for the checkout completed event
/// </summary>
public record CheckoutCompleted : Webhook<CheckoutCompletedData> { }

/// <summary>
/// The checkout completed data payload
/// </summary>
public record CheckoutCompletedData
{
    /// <summary>
    /// The payment identifier
    /// </summary>
    [Required]
    [JsonConverter(typeof(GuidTypeConverter))]
    [JsonPropertyName("paymentId")]
    public Guid PaymentId { get; init; }

    /// <summary>
    /// Specifies the order associated with the payment.
    /// </summary>
    [Required]
    [JsonPropertyName("order")]
    public WebhookOrder Order { get; init; } = new();

    /// <summary>
    /// The consumer (from the example)
    /// </summary>
    [JsonPropertyName("consumer")]
    public CheckoutCompletedConsumer? Consumer { get; init; }
}

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

/// <summary>
/// The checkout completed address
/// </summary>
public record CheckoutCompletedAddress
{
    /// <summary>
    /// The first address line
    /// </summary>
    [JsonPropertyName("addressLine1")]
    public string AddressLine1 { get; init; } = string.Empty;

    /// <summary>
    /// The second address line
    /// </summary>
    [JsonPropertyName("addressLine2")]
    public string AddressLine2 { get; init; } = string.Empty;

    /// <summary>
    /// The city
    /// </summary>
    [JsonPropertyName("city")]
    public string City { get; init; } = string.Empty;

    /// <summary>
    /// The country
    /// </summary>
    [JsonPropertyName("country")]
    public string Country { get; init; } = string.Empty;

    /// <summary>
    /// The post code
    /// </summary>
    [JsonPropertyName("postcode")]
    public string PostCode { get; init; } = string.Empty;

    /// <summary>
    /// The receiver line
    /// </summary>
    [JsonPropertyName("receiverLine")]
    public string ReceiverLine { get; init; } = string.Empty;
}