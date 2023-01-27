using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
// using Microsoft.Extensions.DependencyInjection;
using Moq;
using RichardSzalay.MockHttp;
using SolidNetsEasyClient.Constants;

namespace SolidNetsEasyClient.Tests.Tools;

public static class Mocks
{
    public static IHttpClientFactory HttpClientFactory(HttpMethod requestMethod, string endpoint, Func<HttpRequestMessage, bool> condition, HttpStatusCode successCode, string responseJson)
    {
        var mock = new Mock<IHttpClientFactory>();
        var mockHttp = new MockHttpMessageHandler();

        mockHttp
            .When(requestMethod, endpoint)
            .With(req => req.Headers.Contains("Authorization"))
            .With(condition)
            .Respond(successCode, "application/json", responseJson);

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = NetsEndpoints.LiveBaseUri;

        mock.Setup(f => f.CreateClient(It.IsAny<string>()))
        .Returns(client);

        return mock.Object;
    }

    /// <summary>
    /// Default http request with TUS-Resumable 1.0.0 header if nothing specified
    /// </summary>
    /// <param name="method">The request method</param>
    /// <param name="header">The header</param>
    /// <returns>A mock http request</returns>
    public static HttpRequest HttpRequest(string method = "GET", params (string Key, string Value)[] header)
    {
        var mock = new Mock<HttpRequest>();

        mock.Setup(r => r.Method)
        .Returns(method);

        mock.Setup(r => r.Headers)
        .Returns(new Dictionary<string, string>().ToHeaderDictionary(header));

        return mock.Object;
    }

    private static IHeaderDictionary ToHeaderDictionary(this Dictionary<string, string> headers, params (string, string)[] values)
    {
        var result = new HeaderDictionary();
        foreach (var header in headers)
        {
            result.Add(header.Key, header.Value);
        }

        foreach (var header in values)
        {
            result.Add(header.Item1, header.Item2);
        }

        return result;
    }

    public static IServiceProvider ServiceProvider<T>(T service)
    {
        var mock = new Mock<IServiceProvider>();

        mock.Setup(s => s.GetService(typeof(T)))
        .Returns(service);

        return mock.Object;
    }

    public static Mock<HttpContext> HttpContext(IPAddress fromIp, string method, Dictionary<string, StringValues>? headers = null)
    {
        var mock = new Mock<HttpContext>();

        mock.SetupAllProperties();
        mock.Setup(c => c.Connection.RemoteIpAddress)
        .Returns(fromIp);

        mock.Setup(c => c.Request.Method)
        .Returns(method);

        mock.Setup(c => c.Request.Headers)
        .Returns(new HeaderDictionary(headers ?? new()));

        return mock;
    }

    public static void AddService(this Mock<HttpContext> context, object service, Type serviceType)
    {
        context.Setup(c => c.RequestServices.GetService(serviceType))
        .Returns(service);
    }
}
