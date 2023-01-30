using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
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
    private readonly Dictionary<string, StringValues> headers = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<(object, Type)> services = new();
    private Mock<HttpContext>? httpContext;

    private AuthorizationFilterContextBuilder(string ip, string method)
    {
        httpMethod = method;
        this.ip = IPAddress.Parse(ip);
    }

    public AuthorizationFilterContextBuilder AddOptions(NetsEasyOptions options)
    {
        return AddMockedService(Options.Create(options));
    }

    public AuthorizationFilterContextBuilder SetHttpMethod(string method)
    {
        httpMethod = method;
        return this;
    }

    public AuthorizationFilterContextBuilder AddMockedService<T>([DisallowNull] T service)
    {
        services.Add((service, typeof(T)));
        return this;
    }

    public AuthorizationFilterContextBuilder AddRequestHeader((string HeaderName, string Value) header)
    {
        headers.Add(header.HeaderName, header.Value);
        return this;
    }

    public AuthorizationFilterContext Build()
    {
        httpContext = Tools.Mocks.HttpContext(ip, httpMethod, headers);
        foreach (var (service, type) in services)
        {
            httpContext.AddService(service, type);
        }

        var filters = new List<IFilterMetadata>();
        var actionContext = new ActionContext(
            httpContext.Object,
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
