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
/// SolidNetsEasy webhook attribute callback for the <see cref="EventName.ReservationFailed"/> event. Note that Nets expects the response for a success to be exactly 200 OK.
/// </summary>
public class SolidNetsEasyReservationFailedAttribute : SolidNetsEasyEventAttribute<ReservationFailed, ReservationFailedData>
{
    /// <inheritdoc />
    public SolidNetsEasyReservationFailedAttribute() { }

    /// <inheritdoc />
    public SolidNetsEasyReservationFailedAttribute([StringSyntax("Route")] string template) : base(template) { }

    /// <inheritdoc />
    protected override string RouteName { get; init; } = RouteNameConstants.ReservationFailed;

    /// <inheritdoc />
    protected override bool Validate(ReservationFailed data, IHasher hasher, byte[] key, string authorization, string? complement, string? nonce)
    {
        var invariant = new ReservationFailedInvariant
        {
            Amount = data.Data.Amount.Amount,
            OrderItems = data.Data.OrderItems,
            Nonce = nonce
        };

        return AuthorizationHeaderFlow.ValidateAuthorization(hasher, key, invariant, authorization, complement);
    }
}
