using System;

namespace SolidNetsEasyClient.Tests.Tools;

public static class TestResponses
{
    public const string CreatePaymentResponse = "{\"paymentId\":\"02530000635421d4b30d1cb9e6b55eca\"}";
    public const string PaymentStatusResponseJson = "{\"payment\":{\"paymentId\":\"0197000063542b66b30d1cb9e6b55eee\",\"summary\":{},\"consumer\":{\"shippingAddress\":{},\"company\":{\"contactDetails\":{\"phoneNumber\":{}}},\"privatePerson\":{\"phoneNumber\":{}},\"billingAddress\":{}},\"paymentDetails\":{\"invoiceDetails\":{},\"cardDetails\":{}},\"orderDetails\":{\"amount\":960000,\"currency\":\"DKK\",\"reference\":\"6dd226d8-560e-4fbe-b0b3-642782e6545a\"},\"checkout\":{\"url\":\"https://my.checkout.url\"},\"created\":\"2022-10-22T17:41:58.1001+00:00\"}}";

    public const string SubscriptionStatus = @"{
            ""subscriptionId"": ""d079718b-ff63-45dd-947b-4950c023750f"",
            ""frequency"": 0,
            ""interval"": 0,
            ""endDate"": ""2019-08-24T14:15:22Z"",
            ""paymentDetails"": {
                ""paymentType"": ""CARD"",
                ""paymentMethod"": ""Visa"",
                ""cardDetails"": {
                    ""expiryDate"": ""1223"",
                    ""maskedPan"": ""string""
                }
            },
            ""importError"": {
                ""importStepsResponseCode"": ""string"",
                ""importStepsResponseSource"": ""string"",
                ""importStepsResponseText"": ""string""
            }
        }";
}
