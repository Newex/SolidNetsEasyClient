using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using SolidNetsEasyClient.Extensions;
using SolidNetsEasyClient.Helpers.Encryption.Flows;
using SolidNetsEasyClient.Helpers.Invariants;
using SolidNetsEasyClient.Logging.SolidNetsEasyPaymentCreatedAttributeLogging;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Models.Options;
using static Microsoft.AspNetCore.Http.HttpMethods;

namespace SolidNetsEasyClient.Helpers.WebhookAttributes;

/// <summary>
/// SolidNets Easy webhook attribute callback for the <see cref="EventName.PaymentCreated"/> event
/// </summary>
/// <remarks>
/// In order for the built-in validation to occur the endpoint for the Action method must accept a (*) complement parameter of string and (*) a nonce parameter of string.
/// </remarks>
public sealed class SolidNetsEasyPaymentCreatedAttribute : ActionFilterAttribute, IActionHttpMethodProvider, IRouteTemplateProvider
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public SolidNetsEasyPaymentCreatedAttribute()
    {
    }

    /// <summary>
    /// Instantiate a new <see cref="SolidNetsEasyPaymentCreatedAttribute"/>
    /// </summary>
    /// <param name="template">The route template</param>
    public SolidNetsEasyPaymentCreatedAttribute([StringSyntax("Route")] string template)
    {
        ArgumentNullException.ThrowIfNull(template);
        Template = template;
    }

    /// <summary>
    /// Get the supported http methods
    /// </summary>
    public IEnumerable<string> HttpMethods => new List<string>()
    {
        "POST"
    };

    /// <summary>
    /// The parameter name for the nonce value.
    /// </summary>
    public string NonceParameterName { get; set; } = "nonce";

    /// <summary>
    /// The parameter name for the complement
    /// </summary>
    public string? ComplementParameterName { get; set; } = "complement";

    /// <inheritdoc />
    [StringSyntax("Route")]
    public string? Template { get; init; }

    /// <inheritdoc />
    [DisallowNull]
    public string? Name { get; set; }

    /// <inheritdoc />
    int? IRouteTemplateProvider.Order => Order;

    /// <summary>
    /// Validate authorization header using the built-in functions. Defaults to true.
    /// </summary>
    public bool ValidateAuthorizationHeader { get; set; } = true;

    /// <inheritdoc />
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var request = context.HttpContext.Request;
        var logger = ServiceProviderExtensions.GetLogger<SolidNetsEasyPaymentCreatedAttribute>(request.HttpContext.RequestServices);
        if (!IsPost(request.Method) || !ValidateAuthorizationHeader)
        {
            // Short-circuit pipeline must only handle POST request OR user does not want to validate
            logger.InfoNotPOSTRequest();
            return;
        }

        // The header does not follow any scheme just repeats what was set during notification setup
        var authorization = request.Headers.Authorization;
        if (string.IsNullOrWhiteSpace(authorization))
        {
            // 401 Unauthorized
            logger.ErrorMissingAuthorizationHeader();
            context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            return;
        }

        // Only 1 PaymentCreated parameter
        var payment = (PaymentCreated?)context.ActionArguments.Values.SingleOrDefault(o => o?.GetType() == typeof(PaymentCreated))!;
        if (payment is null)
        {
            logger.ErrorMissingPaymentArgument();
            context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            return;
        }

        var data = GetNonceAndComplement(request);
        if (data is null)
        {
            logger.ErrorMissingNonce();
            context.Result = new StatusCodeResult(StatusCodes.Status400BadRequest);
            return;
        }

        (var nonce, var complement) = data.Value;
        var http = context.HttpContext;
        var encryptionOptions = ServiceProviderExtensions.GetOptions<WebhookEncryptionOptions>(http.RequestServices);
        if (encryptionOptions is null)
        {
            ArgumentNullException.ThrowIfNull(encryptionOptions);
        }

        var invariant = new PaymentCreatedInvariant
        {
            Amount = payment.Data.Order.Amount.Amount,
            OrderItems = payment.Data.Order.OrderItems,
            OrderReference = payment.Data.Order.Reference,
            Nonce = nonce,
        };
        var isValid = PaymentCreatedFlow.ValidatePaymentCreatedEventCallback(encryptionOptions.Value.Hasher, encryptionOptions.Value.Key, invariant, authorization!, complement);
        if (!isValid)
        {
            // 403 Forbidden
            logger.ErrorInvalidAuthorizationHeader();
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
        }
    }

    private (string Nonce, string? Complement)? GetNonceAndComplement(HttpRequest request)
    {
        if (!request.Query.TryGetValue(NonceParameterName, out var nonce))
        {
            var tmp = request.RouteValues[NonceParameterName]?.ToString();
            if (tmp is null)
            {
                return null;
            }

            nonce = tmp;
        }

        string? complement;
        if (ComplementParameterName is null)
        {
            return (nonce!, null);
        }

        complement = !request.Query.TryGetValue(ComplementParameterName, out var complementValue)
            ? (request.RouteValues[ComplementParameterName]?.ToString())
            : complementValue.ToString();

        return (Nonce: nonce.ToString(), Complement: complement);
    }
}
