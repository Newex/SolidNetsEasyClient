namespace SolidNetsEasyClient.Helpers.Encryption.Flows;

/// <summary>
/// Represents an authorization header
/// </summary>
public readonly record struct AuthorizationHeaderModel
{
    /// <summary>
    /// The authorization header value
    /// </summary>
    public required string Authorization { get; init; }

    /// <summary>
    /// The optional authorization complement value
    /// </summary>
    public string? Complement { get; init; }
}
