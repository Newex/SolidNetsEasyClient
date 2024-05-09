using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using SolidNetsEasyClient.Filters;

namespace SolidNetsEasyClient.Extensions;

/// <summary>
/// NetsEasy route extensions
/// </summary>
public static class RouteExtensions
{
    /// <summary>
    /// Add endpoint for a callback that Nets should call, when a subscribed event occurs.
    /// </summary>
    /// <remarks>
    /// To properly handle client IP's, remember to add ForwardedHeaders so ASP.NET Core can work with proxies and load balancers.
    /// </remarks>
    /// <param name="app">The web application</param>
    /// <param name="route">The route</param>
    /// <param name="handler">The route handler</param>
    /// <returns>A route handler builder</returns>
    [RequiresDynamicCode("This API may perform reflection on the supplied delegate and its parameters. These types may require generated code and aren't compatible with native AOT applications.")]
    [RequiresUnreferencedCode("This API may perform reflection on the supplied delegate and its parameters. These types may be trimmed if not directly referenced.")]
    public static RouteHandlerBuilder MapNetsWebhook(this WebApplication app, [StringSyntax("Route")] string route, Delegate handler)
    {
        return app.MapPost(route, handler)
                  .AddEndpointFilter<WebhookFilter>();
    }
}
