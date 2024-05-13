using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SolidNetsEasyClient.Builder;
using SolidNetsEasyClient.Clients;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.Tests.ActualClientTests;

[IntegrationTest]
public class HappyPathTesting
{
    private readonly NetsPaymentClient client;
    private readonly NetsPaymentBuilder builder;
    public HappyPathTesting()
    {
        var options = Options.Create(new NetsEasyOptions()
        {
            CheckoutKey = "my-checkout-key",
            CheckoutUrl = "https://localhost:5110/checkout",
            TermsUrl = "https://localhost:5110/terms",
            PrivacyPolicyUrl = "https://localhost:5110/privacy",
            ClientMode = ClientMode.Test,
            IntegrationType = Integration.EmbeddedCheckout,
            DefaultDenyWebhook = false,
            WhitelistIPsForWebhook = "127.0.0.1"
        });
        var httpClient = new HttpClient()
        {
            BaseAddress = NetsEndpoints.TestingBaseUri,
        };
        httpClient.DefaultRequestHeaders.Add("Authorization", "my-api-key-here");
        client = new NetsPaymentClient(httpClient, options);
        builder = new NetsPaymentBuilder(options);
    }

    [Fact]
    public async Task Test_scenario()
    {
        // 1. Create payment
        var payment = builder.CreatePayment(new Order()
        {
            Currency = Currency.DKK,
            Items = [
                new()
                {
                    Name = "Nuka-Cola",
                    Quantity = 1,
                    Reference = "fallout-nuka-cola",
                    Unit = "pcs",
                    UnitPrice = 25_00,
                }
            ],
            Reference = "my-order-id-reference"
        }, "my-payment-reference").Build();

        // 2. Start checkout
        var paymentResult = await client.StartCheckoutPayment(payment);

        // 3. Get payment info
        var paymentInfo = await client.RetrievePaymentDetails(paymentResult!.PaymentId);
    }
}
