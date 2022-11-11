using System;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Models.DTOs.Contacts;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Customers;

/// <summary>
/// A natural customer person information
/// </summary>
public record PrivatePersonInfo : Person
{
    /// <summary>
    /// The merchant reference
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("merchantReference")]
    public string? MerchantReference { get; init; }

    /// <summary>
    /// The date on which the customer was born
    /// </summary>
    [JsonConverter(typeof(NullableDateTimeOffsetConverter))]
    [JsonPropertyName("dateOfBirth")]
    public DateTimeOffset? DateOfBirth { get; init; }

    /// <summary>
    /// The email address
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("email")]
    public string? Email { get; init; }

    /// <summary>
    /// An international phone number
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("phoneNumber")]
    public PhoneNumber? PhoneNumber { get; init; }
}
