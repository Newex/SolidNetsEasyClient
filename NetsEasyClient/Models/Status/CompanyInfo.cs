using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.Status;

/// <summary>
/// The customer's company info
/// </summary>
public record CompanyInfo
{
    /// <summary>
    /// The merchant reference
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("merchantReference")]
    public string? MerchantReference { get; init; }

    /// <summary>
    /// The company name
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("name")]
    public string? Name { get; }

    /// <summary>
    /// The registration number
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("registrationNumber")]
    public string? RegistrationNumber { get; init; }

    /// <summary>
    /// Information about the contact person for a company
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("contactDetails")]
    public ContactDetail? ContactDetails { get; init; }
}
