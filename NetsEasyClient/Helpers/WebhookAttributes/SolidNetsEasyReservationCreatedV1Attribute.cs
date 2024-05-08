using System.Diagnostics.CodeAnalysis;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Helpers.WebhookAttributes;

/// <summary>
/// SolidNetsEasy webhook attribute callback for the <see cref="EventName.ReservationCreatedV1"/> event. Note that Nets expects the response for a success to be exactly 200 OK.
/// </summary>
/// <remarks>
/// The hidden query parameters are by default: complement (string), nonce (string), bulk (bool).
/// The required parameter from the request body is: <see cref="ReservationCreatedV1"/>
/// </remarks>
public class SolidNetsEasyReservationCreatedV1Attribute : SolidNetsEasyEventAttribute<ReservationCreatedV1, ReservationCreatedDataV1>
{
    /// <inheritdoc />
    public SolidNetsEasyReservationCreatedV1Attribute() { }

    /// <inheritdoc />
    public SolidNetsEasyReservationCreatedV1Attribute([StringSyntax("Route")] string template) : base(template) { }

    /// <inheritdoc />
    protected override string RouteName { get; init; } = RouteNameConstants.ReservationCreatedV1;
}
