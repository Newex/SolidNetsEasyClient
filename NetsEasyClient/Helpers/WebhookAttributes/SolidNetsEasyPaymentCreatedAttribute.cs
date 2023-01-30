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
/// SolidNetsEasy webhook attribute callback for the <see cref="EventName.PaymentCreated"/> event. Note that Nets expects the response for a success to be exactly 200 OK.
/// </summary>
public sealed class SolidNetsEasyPaymentCreatedAttribute : SolidNetsEasyEventAttribute<PaymentCreated, PaymentCreatedData>
{
    /// <inheritdoc />
    protected override string RouteName { get; init; } = RouteNameConstants.PaymentCreatedRoute;

    /// <inheritdoc />
    public SolidNetsEasyPaymentCreatedAttribute() { }

    /// <inheritdoc />
    public SolidNetsEasyPaymentCreatedAttribute([StringSyntax("Route")] string template) : base(template) { }

    /// <inheritdoc />
    protected override bool Validate(PaymentCreated data, IHasher hasher, byte[] key, string authorization, string? complement, string? nonce)
    {
        var invariant = new PaymentCreatedInvariant
        {
            Amount = data.Data.Order.Amount.Amount,
            OrderItems = data.Data.Order.OrderItems,
            OrderReference = data.Data.Order.Reference,
            Nonce = nonce
        };
        return AuthorizationHeaderFlow.ValidateAuthorization(hasher, key, invariant, authorization, complement);
    }
}