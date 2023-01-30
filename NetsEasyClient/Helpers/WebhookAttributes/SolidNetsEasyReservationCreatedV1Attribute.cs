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
/// SolidNetsEasy webhook attribute callback for the <see cref="EventName.ReservationCreatedV1"/> event. Note that Nets expects the response for a success to be exactly 200 OK.
/// </summary>
public class SolidNetsEasyReservationCreatedV1Attribute : SolidNetsEasyEventAttribute<ReservationCreatedV1, ReservationCreatedDataV1>
{
    /// <inheritdoc />
    public SolidNetsEasyReservationCreatedV1Attribute() { }

    /// <inheritdoc />
    public SolidNetsEasyReservationCreatedV1Attribute([StringSyntax("Route")] string template) : base(template) { }

    /// <inheritdoc />
    protected override string RouteName { get; init; } = RouteNameConstants.ReservationCreatedV1;

    /// <inheritdoc />
    protected override bool Validate(ReservationCreatedV1 data, IHasher hasher, byte[] key, string authorization, string? complement, string? nonce)
    {
        var invariant = new AmountInvariant
        {
            Amount = data.Data.Amount.Amount,
            Nonce = nonce
        };
        return AuthorizationHeaderFlow.ValidateAuthorization(hasher, key, invariant, authorization, complement);
    }
}
