using System.Net.Http;
using Microsoft.Extensions.Options;
using SolidNetsEasyClient.Clients;
using SolidNetsEasyClient.Models.Options;
using SolidNetsEasyClient.Tests.ClientTests.Integration.Stubs;

namespace SolidNetsEasyClient.Tests.ClientTests.Integration;

public static class Setup
{
    public static PaymentClient PaymentClient()
    {
        const string key = "secret-key";

        return new PaymentClient(
            Options.Create<PlatformPaymentOptions>(new()
            {
                Authorization = key
            }),
            new HttpClientFactoryStub()
        );
    }
}
