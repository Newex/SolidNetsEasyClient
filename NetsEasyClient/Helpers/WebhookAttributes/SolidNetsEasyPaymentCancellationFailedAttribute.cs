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
/// SolidNetsEasy webhook attribute callback for the <see cref="EventName.PaymentCancellationFailed"/> event. Note that Nets expects the response for a success to be exactly 200 OK.
/// </summary>
/// <remarks>
/// The hidden query parameters are by default: complement (string), nonce (string), bulk (bool).
/// The required parameter from the request body is: <see cref="PaymentCancellationFailed"/>
/// </remarks>
public class SolidNetsEasyPaymentCancellationFailedAttribute : SolidNetsEasyEventAttribute<PaymentCancellationFailed, PaymentCancellationFailedData>
{
    /// <inheritdoc />
    protected override string RouteName { get; init; } = RouteNameConstants.PaymentCancellationFailed;

    /// <inheritdoc />
    public SolidNetsEasyPaymentCancellationFailedAttribute() { }

    /// <inheritdoc />
    public SolidNetsEasyPaymentCancellationFailedAttribute([StringSyntax("Route")] string template) : base(template) { }

    /// <inheritdoc />
    protected override bool Validate(PaymentCancellationFailed data, IHasher hasher, byte[] key, string authorization, string? complement, string? nonce)
    {
        var invariant = new OrderItemsAmountInvariant
        {
            OrderItems = data.Data.OrderItems,
            Amount = data.Data.Amount.Amount,
            Nonce = nonce
        };

        return AuthorizationHeaderFlow.ValidateAuthorization(hasher, key, invariant, authorization, complement);
    }
}
