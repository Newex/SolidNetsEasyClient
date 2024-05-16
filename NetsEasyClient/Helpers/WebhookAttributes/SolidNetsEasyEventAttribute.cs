using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using SolidNetsEasyClient.Extensions;
using SolidNetsEasyClient.Logging.SolidNetsEasyPaymentCreatedAttributeLogging;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;
using static Microsoft.AspNetCore.Http.HttpMethods;

namespace SolidNetsEasyClient.Helpers.WebhookAttributes;

/// <summary>
/// Generic attribute for webhook events
/// </summary>
/// <typeparam name="T">The webhook event type</typeparam>
/// <typeparam name="TData">The webhook event payload type</typeparam>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public abstract class SolidNetsEasyEventAttribute<T, TData> : ActionFilterAttribute, IActionHttpMethodProvider, IRouteTemplateProvider
    where T : Webhook<TData>
    where TData : WebhookData, new()
{
    /// <summary>
    /// Instantiate a new event attribute
    /// </summary>
    public SolidNetsEasyEventAttribute()
    {
        Name = RouteName;
    }

    /// <summary>
    /// Instantiate a new event attribute with a given route template
    /// </summary>
    /// <param name="template">The route template</param>
    public SolidNetsEasyEventAttribute([StringSyntax("Route")] string template)
    {
        ArgumentNullException.ThrowIfNull(template);
        Template = template;
        Name = RouteName;
    }

    /// <summary>
    /// The route name
    /// </summary>
    protected abstract string RouteName { get; init; }

    /// <summary>
    /// The http methods supported by the action
    /// </summary>
    public IEnumerable<string> HttpMethods => new List<string>
    {
        "POST"
    };

    /// <summary>
    /// The routing template
    /// </summary>
    [StringSyntax("Route")]
    public string? Template { get; init; }

    /// <summary>
    /// The routing name
    /// </summary>
    [DisallowNull]
    public string? Name { get; set; }

    /// <inheritdoc />
    int? IRouteTemplateProvider.Order => Order;

    /// <inheritdoc />
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var request = context.HttpContext.Request;
        var logger = ServiceProviderExtensions.GetLogger<SolidNetsEasyEventAttribute<T, TData>>(request.HttpContext.RequestServices);
        if (!IsPost(request.Method))
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

        // Only 1 data event parameter
        T? payload = null;
        try
        {
            payload = (T?)context.ActionArguments.Values.SingleOrDefault(o => o?.GetType() == typeof(T))!;
            if (payload is null)
            {
                logger.ErrorMissingEventPayloadArgument();
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                return;
            }
        }
        catch (InvalidOperationException ex)
        {
            logger.ErrorMoreThanOneEventPayloadArgument(ex);
            context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            return;
        }

        var http = context.HttpContext;
    }

    /// <summary>
    /// If successfully executed the action, then change the response to 200 OK if not
    /// </summary>
    /// <param name="context">The context</param>
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        context.HttpContext.Response.OnStarting(obj =>
        {
            var ctx = (ResultExecutingContext)obj;
            var logger = ServiceProviderExtensions.GetLogger<SolidNetsEasyEventAttribute<T, TData>>(context.HttpContext.RequestServices);
            var status = ctx.HttpContext.Response.StatusCode;
            if (status == StatusCodes.Status200OK)
            {
                return Task.CompletedTask;
            }

            if (status is > 200 and <= 299)
            {
                logger.WarningWrongResponseCode(status, ctx.HttpContext.Request);
                ctx.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }, context);
    }
}
