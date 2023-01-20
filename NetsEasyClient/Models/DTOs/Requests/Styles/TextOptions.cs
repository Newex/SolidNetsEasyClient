using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Styles;

/// <summary>
/// Options for what text is displayed on the checkout page
/// </summary>
public record TextOptions
{
    /// <summary>
    /// The payment button text
    /// </summary>
    /// <remarks>
    /// Overrides payment button text. The payment button text is localized
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("completePaymentButtonText")]
    public PaymentText? CompletePaymentButtonText { get; init; }
}

/// <summary>
/// Predefined allowed payment text
/// </summary>
public enum PaymentText
{
    /// <summary>
    /// Pay
    /// </summary>
    Pay = 0,

    /// <summary>
    /// Purchase
    /// </summary>
    Purchase = 1,

    /// <summary>
    /// Order
    /// </summary>
    Order = 2,

    /// <summary>
    /// Book
    /// </summary>
    Book = 3,

    /// <summary>
    /// Reserve
    /// </summary>
    Reserve = 4,

    /// <summary>
    /// Signup
    /// </summary>
    Signup = 5,

    /// <summary>
    /// Subscribe
    /// </summary>
    Subscribe = 6,

    /// <summary>
    /// Accept
    /// </summary>
    Accept = 7
}
