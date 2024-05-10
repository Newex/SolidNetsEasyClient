using System.Diagnostics.CodeAnalysis;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

namespace SolidNetsEasyClient.Helpers.WebhookAttributes;

/// <summary>
/// SolidNetsEasy webhook attribute callback for the <see cref="EventName.ChargeCreated"/> event. Note that Nets expects the response for a success to be exactly 200 OK.
/// </summary>
/// <remarks>
/// The hidden query parameters are by default: complement (string), nonce (string), bulk (bool).
/// The required parameter from the request body is: <see cref="ChargeCreated"/>
/// </remarks>
public class SolidNetsEasyChargeCreatedAttribute : SolidNetsEasyEventAttribute<ChargeCreated, ChargeData>
{
    /// <inheritdoc />
    protected override string RouteName { get; init; } = RouteNameConstants.ChargeCreated;

    /// <inheritdoc />
    public SolidNetsEasyChargeCreatedAttribute() { }

    /// <inheritdoc />
    public SolidNetsEasyChargeCreatedAttribute([StringSyntax("Route")] string template) : base(template) { }
}
