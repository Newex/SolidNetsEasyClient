using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Moq;
using SolidNetsEasyClient.Models.Options;
using SolidNetsEasyClient.Tests.Tools;

namespace SolidNetsEasyClient.Tests.Attributes.Webhook;

public static class Setup
{
}

public sealed class AuthorizationFilterContextBuilder
{
    private readonly IPAddress ip;
    private string httpMethod;
    private Mock<HttpContext>? httpContext;

    private AuthorizationFilterContextBuilder(string ip, string method)
    {
        httpMethod = method;
        this.ip = IPAddress.Parse(ip);
    }

    public AuthorizationFilterContextBuilder AddOptions(PlatformPaymentOptions options)
    {
        return AddMockedService(Options.Create(options));
    }

    public AuthorizationFilterContextBuilder SetHttpMethod(string method)
    {
        httpMethod = method;
        return this;
    }

    public AuthorizationFilterContextBuilder AddMockedService<T>(T service)
    {
        (httpContext ??= Tools.Mocks.HttpContext(ip, httpMethod)).AddService(service);
        return this;
    }

    public AuthorizationFilterContext Build()
    {
        var filters = new List<IFilterMetadata>();
        var actionContext = new ActionContext(
            httpContext?.Object ?? new DefaultHttpContext(),
            new Microsoft.AspNetCore.Routing.RouteData(),
            new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()
        );
        return new(actionContext, filters);
    }

    public static AuthorizationFilterContextBuilder Create(string fromIP, string? method = null)
    {
        var httpMethod = method ?? "POST";
        return new AuthorizationFilterContextBuilder(fromIP, httpMethod);
    }
}
