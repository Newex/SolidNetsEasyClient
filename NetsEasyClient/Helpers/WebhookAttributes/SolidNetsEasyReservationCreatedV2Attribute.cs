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
/// SolidNetsEasy webhook attribute callback for the <see cref="EventName.ReservationCreatedV2"/> event. Note that Nets expects the response for a success to be exactly 200 OK.
/// </summary>
public class SolidNetsEasyReservationCreatedV2Attribute : SolidNetsEasyEventAttribute<ReservationCreatedV2, ReservationCreatedDataV2>
{
    /// <inheritdoc />
    public SolidNetsEasyReservationCreatedV2Attribute() { }

    /// <inheritdoc />
    public SolidNetsEasyReservationCreatedV2Attribute([StringSyntax("Route")] string template) : base(template) { }

    /// <inheritdoc />
    protected override string RouteName { get; init; } = RouteNameConstants.ReservationCreatedV2;

    /// <inheritdoc />
    protected override bool Validate(ReservationCreatedV2 data, IHasher hasher, byte[] key, string authorization, string? complement, string? nonce)
    {
        var invariant = new ReservationCreatedInvariant
        {
            Amount = data.Data.Amount.Amount,
            Nonce = nonce
        };

        return AuthorizationHeaderFlow.ValidateAuthorization(hasher, key, invariant, authorization, complement);
    }
}
