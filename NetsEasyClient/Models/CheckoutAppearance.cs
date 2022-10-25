using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models;

/// <summary>
/// Defines the checkout appearance
/// </summary>
public record CheckoutAppearance
{
    /// <summary>
    /// Controls what is displayed on the checkout page
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("displayOptions")]
    public DisplayOptions? DisplayOptions { get; init; }

    /// <summary>
    /// Controls what text is displayed on the checkout page
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("textOptions")]
    public TextOptions? TextOptions { get; init; }
}
