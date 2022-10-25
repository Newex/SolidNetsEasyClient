using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.Status;

/// <summary>
/// The contact detail for the <see cref="CompanyInfo"/>
/// </summary>
public record ContactDetail
{
    /// <summary>
    /// The first name (also known as given name)
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("firstName")]
    public string? FirstName { get; init; }

    /// <summary>
    /// The last name (also known as surname/family name)
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("lastName")]
    public string? LastName { get; init; }

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
