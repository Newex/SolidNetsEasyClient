using System.Diagnostics.CodeAnalysis;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Helpers.Encryption;
using SolidNetsEasyClient.Helpers.Encryption.Flows;
using SolidNetsEasyClient.Helpers.Invariants;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Helpers.WebhookAttributes;

/// <summary>
/// SolidNetsEasy webhook attribute callback for the <see cref="EventName.CheckoutCompleted"/> event. Note that Nets expects the response for a success to be exactly 200 OK.
/// </summary>
public class SolidNetsEasyCheckoutCompletedAttribute : SolidNetsEasyEventAttribute<CheckoutCompleted, CheckoutCompletedData>
{
    /// <inheritdoc />
    protected override string RouteName { get; init; } = RouteNameConstants.CheckoutCompleted;

    /// <inheritdoc />
    public SolidNetsEasyCheckoutCompletedAttribute() { }

    /// <inheritdoc />
    public SolidNetsEasyCheckoutCompletedAttribute([StringSyntax("Route")] string template) : base(template) { }

    /// <inheritdoc />
    protected override bool Validate(CheckoutCompleted data, IHasher hasher, byte[] key, string authorization, string? complement, string? nonce)
    {
        var invariant = new OrderReferenceItemsAmountInvariant
        {
            OrderReference = data.Data.Order.Reference,
            OrderItems = data.Data.Order.OrderItems,
            Amount = data.Data.Order.Amount.Amount,
            Nonce = nonce
        };
        return AuthorizationHeaderFlow.ValidateAuthorization(hasher, key, invariant, authorization, complement);
    }
}
