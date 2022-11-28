# What is it?
This is a client for making type safe requests to the [nets easy API endpoint](https://developers.nets.eu/nets-easy/en-EU/api/payment-v1/).

### Status
Current development status is: under development, non functional library.

# Quickstart

Add the package to your project:  
```
$ `dotnet add package SolidNetsEasyClient`
```

Register the service in the startup process:

```csharp
// Register nets services
builder
.Services
.AddNetsEasyClient()
.Configure(builder.Configuration);
```

Either via appsettings.json or environment variables set the payment option values (or via the `Configure` extension method)
```
// File appsettings.json
  {
    ...
    "SolidNetsEasy": {
        "ApiKey": "<insert-secret-api-key-here>", // DO NOT EXPOSE to end user
        "CheckoutKey": "<insert-checkout-key-here>", // Use on the front end
        "CheckoutUrl": "https://exact.url.to/checkout",
        "TermsUrl": "http://my.terms.url",
        "PrivacyPolicyUrl": "http://privacy.url",
        "CommercePlatformTag": "optional identifier for the ecommerce platform",
        "ReturnUrl": "http://only-use-when-hosting-checkout-on-nets.com/aka/HostedPaymentPage",
        "ClientMode": "Test" // Can be 'Test' or 'Live' mode
    }
  }
```

And use the payment builder to construct a payment object:

```csharp
Order order = new Order { /* With order items */};
var paymentBuilder = NetsPaymentBuilder
                        .CreatePayment(order)
                        .WithPrivateCustomer(
                            customerId: "myInternalCustomerId",
                            firstName: "John",
                            lastName: "Doe",
                            email: "john@doe.com"
                        )
                        .ChargePaymentOnCreation(true);
PaymentRequest payment = paymentBuilder.BuildPaymentRequest;
```
Then use the `PaymentClient` or `SubscriptionClient` or `UnscheduledSubscriptionClient` in your `Controller` via dependency injection:

```csharp
public class MyController : Controller
{
    public MyController(PaymentClient paymentClient)
    {
        // ...
    }
}
```

To start a checkout session:
```csharp
PaymentResult payment = await paymentClient.CreatePaymentAsync(payment, CancellationToken.None);
```


# Features
Use the client to make and manage payments, from the backend. Remember that the you still need a frontend for a customer to input payment details.


# Roadmap
* Handle payments, subscriptions and webhooks in a type safe and easy to use way.
* Create nuget package
* Add unit tests
* Add easy to use configuration for handling API keys and other client settings
* Add example site