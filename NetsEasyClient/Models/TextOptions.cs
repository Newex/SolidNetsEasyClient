using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models;

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
    Pay,

    /// <summary>
    /// Purchase
    /// </summary>
    Purchase,

    /// <summary>
    /// Order
    /// </summary>
    Order,

    /// <summary>
    /// Book
    /// </summary>
    Book,

    /// <summary>
    /// Reserve
    /// </summary>
    Reserve,

    /// <summary>
    /// Signup
    /// </summary>
    Signup,

    /// <summary>
    /// Subscribe
    /// </summary>
    Subscribe,

    /// <summary>
    /// Accept
    /// </summary>
    Accept
}