using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Options;
using SolidNetsEasyClient.Clients;
using SolidNetsEasyClient.Models.Options;
using SolidNetsEasyClient.Tests.Tools;

namespace SolidNetsEasyClient.Tests.ClientTests.Unit;
#nullable enable
public static class Setup
{
    public static PaymentClient PaymentClient(HttpMethod method, string relativePath, HttpStatusCode success, string responseJson, Func<HttpRequestMessage, bool>? condition = null)
    {
        var defaultCondition = condition ?? ((_) => true);
        return new PaymentClient(
            Options.Create<PlatformPaymentOptions>(new()
            {
                ApiKey = "MycustomAPI_KEY"
            }),
            Mocks.PaymentHttpClientFactory(method, "https://api.dibspayment.eu" + relativePath, defaultCondition, success, responseJson)
        );
    }
}
