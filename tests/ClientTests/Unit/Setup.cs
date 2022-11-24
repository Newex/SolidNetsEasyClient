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
    private const string NetsBaseURI = "https://api.dibspayment.eu";
    private static readonly PlatformPaymentOptions options = new()
    {
        ApiKey = "MycustomAPI_KEY",
        CheckoutKey = "MyCheckout_Key",
        CheckoutUrl = "http://my.checkout.url",
        TermsUrl = "http://terms.and.conditions.url"
    };

    public static PaymentClient PaymentClient(HttpMethod method, string relativePath, HttpStatusCode success, string responseJson, Func<HttpRequestMessage, bool>? condition = null)
    {
        var defaultCondition = condition ?? ((_) => true);
        return new PaymentClient(
            Options.Create(options),
            Mocks.HttpClientFactory(method, NetsBaseURI + relativePath, defaultCondition, success, responseJson)
        );
    }

    public static SubscriptionClient SubscriptionClient(HttpMethod method, string relativePath, HttpStatusCode success, string responseJson, Func<HttpRequestMessage, bool>? condition = null)
    {
        var defaultCondition = condition ?? ((_) => true);
        return new SubscriptionClient(
            Options.Create(options),
            Mocks.HttpClientFactory(method, NetsBaseURI + relativePath, defaultCondition, success, responseJson)
        );
    }
}
