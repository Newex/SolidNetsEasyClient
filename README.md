# What is it?
This is a client for making type safe requests to the [Easy Payment API endpoint](https://developer.nexigroup.com/nexi-checkout/en-EU/api/payment-v1/).

### Status
Current development status is: under development, somewhat functional library. 
Since Nexi and Nets have had a merger, this library uses a mixture of naming references to Nets and Nexi.

# Quickstart

Add the package to your project:  
```
$ `dotnet add package SolidNetsEasyClient`
```

Register the service in the startup process:

```csharp
// Register nets services
builder.Services
  .AddNetsEasy(options =>
  {
    // Configure all the options here...
    // MUST set these either through appsettings.json or via env vars:
    options.ApiKey = "my-api-key";
    options.TermsUrl = "https://my-site/terms-and-conditions";
    options.PrivacyPolicyUrl = "https://my-site/privacy-policy";

    // If embedded checkout:
    options.IntegrationType = Integration.EmbeddedCheckout;
    options.CheckoutUrl = "https://exact.url.to/checkout";
    options.CheckoutKey = "my-checkout-key";

    // If hosted on nexi:
    options.IntegrationType = Integration.HostedPaymentPage;
    options.ReturnUrl = "https://redirect-on-success.to/my-page";
    options.CancelUrl = "https://redirect-on-cancel.to/my-other-page";
  });
```

Use the payment builder to construct a payment object, or make a `PaymentRequest` yourself:

```csharp
Order order = new Order { /* With order items */};
NetsPaymentBuilder builder; // Inject by dependency injection
PaymentRequest payment = builder.CreatePayment(order)
                        .WithPrivateCustomer(
                            customerId: "myInternalCustomerId",
                            firstName: "John",
                            lastName: "Doe",
                            email: "john@doe.com"
                        )
                        .MerchantHandlesCustomerData()
                        .AddCustomer()
                        .ChargeImmidiately()
                        .AddWebhook("https://my-site/callback/charge", EventName.ChargeCreated, "authorizationheader123")
                        .AddWebhook("https://my-site/callback", EventName.PaymentCreated, "authorizationheader123")
                        .Build();
```

Then start a checkout session using the http client `NexiClient` or the typed `IPaymentClient`.

```csharp
NexiClient client; // Inject by dependency injection
PaymentResult? payment = await paymentClient.StartCheckoutPayment(payment);
```

# Webhooks
The webhook notifications, will be directed to the endpoint you specified in the `AddWebhook` builder method.  
It is highly recommended to use these for processing customer payments. The callback webhook will be an http `POST` request with an authorization header as specified on the payment request.

For example, you can either use the generic interface `IWebhook<WebhookData>` to catch any webhook callbacks OR you can specify the expected callback type (e.g) `ChargeCreated`.

```csharp
[HttpPost("/callback")]
public IResult Method(IWebhook<WebhookData> payload)
{
    if (payload is PaymentCreated paymentCreated)
    {
        // Handle payment creation..
    }
    else if (payload is ChargeFailed chargeFailed)
    {
        // etc.
    }
}
```

```csharp
// Route: https://my-site/callback/charge
public IResult Method(ChargeCreation payload)
{
    // Handle charge creation..
}
```

|Nets event name | Description (from [Nets Easy](https://developers.nets.eu/nets-easy/en-EU/api/webhooks/#payment-events))| DTO |  
| --- | --- | --- |  
|payment.created | A payment has been created. | PaymentCreated |
|payment.reservation.created | The amount of the payment has been reserved.| ReservationCreatedV1 |
|payment.reservation.created.v2 | The amount of the payment has been reserved.| ReservationCreatedV2 |
|payment.reservation.failed | A reservation attempt has failed.| ReservationFailed |
|payment.checkout.completed | The customer has completed the checkout.| CheckoutCompleted |
|payment.charge.created.v2 | The customer has successfully been charged, partially or fully.| ChargeCreated |
|payment.charge.failed | A charge attempt has failed.| ChargeFailed |
|payment.refund.initiated.v2 | A refund has been initiated. | RefundInitiated |
|payment.refund.failed | A refund attempt has failed.| RefundFailed |
|payment.refund.completed | A refund has successfully been completed.| RefundCompleted |
|payment.cancel.created | A reservation has been canceled. | PaymentCancelled |
|payment.cancel.failed | A cancellation has failed. | PaymentCancellationFailed |

# Proxy
You must remember to configure the forwarded headers, if you want ASP.NET Core to work with proxy servers and load balancers.  
Documentation reference: https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-8.0

### Summary

Configure the forwarded headers for `X-Forwarded-For` header field.

```csharp
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});
```

Then insert the middleware as the first item in the pipeline:

```csharp
app.UseForwardedHeaders();
```

## IP-filter
The `SolidNetsEasyIPFilterAttribute` can both whitelist and blacklist ips or ip-ranges (using the CIDR-format). Thus minimally ensuring that any webhook requests are coming from an accepted source.

For the minimal API you can use the endpoint filter `WebhookFilter` to ensure that the IP is on the whitelist.  
Or just use the `MapNetsWebhook` extension method, which also ensures the response to be 200 OK and listens on POST requests.

# Terminology for Nexi-Nets
According to my own understanding.

| Term | Definition |
|------|------------|
| **Merchant** | You, the seller.|
| **Consumer** | The customer. |
| **Payment** | A request from merchant to the consumer, to get paid. |
| **Charge** | When money has been transfered thus finalizing the payment. |
| **Order** | The items the consumer has bought. |
| **Subscription** | A completely regular recurring payment, which cannot change in either Amount or in Time. E.g. Always 50 DKK on the last day of the month. Has an exact end date. |
| **Unscheduled subscription** | A variable recurring payment, which can change in Amount and in Time. Consumer MUST agree to the terms that Nexi/Nets stores the consumer's payment details. Has no end date. |

# Features
Use the client to make and manage payments, from the backend. Remember that the you still need a frontend for a customer to input payment details.


# Roadmap
- [x] Handle payments, subscriptions and webhooks in a type safe and easy to use way.
- [x] Create nuget package
- [x] Add unit tests
- [x] Add easy to use configuration for handling API keys and other client settings
- [x] Add example site
- [ ] Add documentation for the NetsWebhookController (usage and explanation)
- [ ] Proof read README
- [ ] Add options section that explains each option
- [ ] Add Nets Terminology for vocabulary such as: Payment, checkout, charge...