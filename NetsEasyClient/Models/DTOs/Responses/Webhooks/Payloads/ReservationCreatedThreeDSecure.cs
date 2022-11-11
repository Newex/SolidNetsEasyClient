using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

/// <summary>
/// The 3D secure
/// </summary>
public record ReservationCreatedThreeDSecure
{
    /// <summary>
    /// The authentication enrollment status
    /// </summary>
    [JsonPropertyName("authenticationEnrollmentStatus")]
    public string AuthenticationEnrollmentStatus { get; init; } = string.Empty;

    /// <summary>
    /// The authentication status
    /// </summary>
    [JsonPropertyName("authenticationStatus")]
    public string AuthenticationStatus { get; init; } = string.Empty;

    /// <summary>
    /// The eci
    /// </summary>
    [JsonPropertyName("eci")]
    public string ECI { get; init; } = string.Empty;
}
