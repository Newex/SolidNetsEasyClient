using System;
using System.Net;
using System.Net.Http;
using Moq;
using RichardSzalay.MockHttp;
using SolidNetsEasyClient.Constants;

namespace SolidNetsEasyClient.Tests.Tools;

public static class Mocks
{
#nullable enable
    public static IHttpClientFactory PaymentHttpClientFactory(HttpMethod requestMethod, string endpoint, Func<HttpRequestMessage, bool> condition, HttpStatusCode successCode, string responseJson)
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
}