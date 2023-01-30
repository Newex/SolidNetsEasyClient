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
/// SolidNetsEasy webhook attribute callback for the <see cref="EventName.RefundFailed"/> event. Note that Nets expects the response for a success to be exactly 200 OK.
/// </summary>
public class SolidNetsEasyRefundFailedAttribute : SolidNetsEasyEventAttribute<RefundFailed, RefundFailedData>
{
    /// <inheritdoc />
    protected override string RouteName { get; init; } = RouteNameConstants.RefundFailed;

    /// <inheritdoc />
    public SolidNetsEasyRefundFailedAttribute() { }

    /// <inheritdoc />
    public SolidNetsEasyRefundFailedAttribute([StringSyntax("Route")] string template) : base(template) { }

    /// <inheritdoc />
    protected override bool Validate(RefundFailed data, IHasher hasher, byte[] key, string authorization, string? complement, string? nonce)
    {
        var invariant = new AmountInvariant
        {
            Amount = data.Data.Amount.Amount,
            Nonce = nonce
        };
        return AuthorizationHeaderFlow.ValidateAuthorization(hasher, key, invariant, authorization, complement);
    }
}
