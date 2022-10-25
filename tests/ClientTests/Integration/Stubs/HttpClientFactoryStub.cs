using System;
using System.Net.Http;

namespace SolidNetsEasyClient.Tests.ClientTests.Integration.Stubs;

public class HttpClientFactoryStub : IHttpClientFactory
{
    public HttpClient CreateClient(string name)
    {
        return new HttpClient
        {
            BaseAddress = new("https://test.api.dibspayment.eu"),
        };
    }
}
