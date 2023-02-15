# What is it?
This is a client for making type safe requests to the [nets easy API endpoint](https://developers.nets.eu/nets-easy/en-EU/api/payment-v1/).

### Status
Current development status is: under development, somewhat functional library.

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
.ConfigureFromConfiguration(builder.Configuration);
```

Either via appsettings.json or environment variables set the payment option values (or via the `ConfigureNetsEasyOptions` extension method)
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
NetsPaymentFactory paymentFactory; // Inject by dependency injection into the constructor
var paymentBuilder = paymentFactory.CreatePaymentBuilder(order)
                        .EmbedCheckoutOnMyPage()
                        .WithPrivateCustomer(
                            customerId: "myInternalCustomerId",
                            firstName: "John",
                            lastName: "Doe",
                            email: "john@doe.com"
                        )
                        .ChargePaymentOnCreation(true)
                        .SubscribeToEvent(EventName.ChargeCreated, Url)
                        .AsSinglePayment();
PaymentRequest payment = paymentBuilder.BuildPaymentRequest;
```
Then start a checkout session using the `PaymentClient` or `SubscriptionClient` or `UnscheduledSubscriptionClient` in your `Controller` via dependency injection:


```csharp
PaymentClient paymentClient; // Inject by dependency injection
PaymentResult payment = await paymentClient.CreatePaymentAsync(payment, CancellationToken.None);
```

To get webhook notifications create a controller and add the following action methods, see example site for more examples:

```csharp
[SolidNetsEasyIPFilter(WhitelistIPs = "::1")]
[Route("/webhook")]
public class WebhookController : Controller
{
    [SolidNetsEasyChargeCreated("nets/charge/created")]
    public ActionResult ChargeCreated([FromBody] ChargeCreated charge)
    {
        // Handle code response
        // Note the action MUST have the correct Event webhook DTO, in this example: ChargeCreated
        return Ok();
    }
}
```

Naming convention for the webhook attributes is `SolidNetsEasy_{EventName}_Attribute`, for example: `SolidNetsEasyPaymentCreatedAttribute`.

# Webhooks
Use these attributes if you want SolidNetsEasy to construct the URL when sending payment to Nets using the `SubscribeToEvent`-method with the `IUrlHelper` parameter.  
Furthermore SolidNetsEasy will validate the incoming requests using Authorization header, a complement to the authorization header and an optional nonce value.


|Nets event name |Attribute | Description (from [Nets Easy](https://developers.nets.eu/nets-easy/en-EU/api/webhooks/#payment-events))| DTO |
--- | --- | --- | --- |
|payment.created |SolidNetsEasyPaymentCreatedAttribute | A payment has been created. | PaymentCreated |
|payment.reservation.created | SolidNetsEasyReservationCreatedV1Attribute |	The amount of the payment has been reserved.| ReservationCreatedV1 |
|payment.reservation.created.v2 | SolidNetsEasyReservationCreatedV2Attribute | 	The amount of the payment has been reserved.| ReservationCreatedV2 |
|payment.reservation.failed | SolidNetsEasyReservationFailedAttribute | A reservation attempt has failed.| ReservationFailed |
|payment.checkout.completed | SolidNetsEasyCheckoutCompletedAttribute | The customer has completed the checkout.| CheckoutCompleted |
|payment.charge.created.v2 | SolidNetsEasyChargeCreatedAttribute | The customer has successfully been charged, partially or fully.| ChargeCreated |
|payment.charge.failed | SolidNetsEasyChargeFailedAttribute | A charge attempt has failed.| ChargeFailed |
|payment.refund.initiated.v2 | SolidNetsEasyRefundInitiatedAttribute | A refund has been initiated. | RefundInitiated |
|payment.refund.failed | SolidNetsEasyRefundFailedAttribute | A refund attempt has failed.| RefundFailed |
|payment.refund.completed | SolidNetsEasyRefundCompletedAttribute | A refund has successfully been completed.| RefundCompleted |
|payment.cancel.created | SolidNetsEasyPaymentCancelledAttribute | A reservation has been canceled. | PaymentCancelled |
|payment.cancel.failed | SolidNetsEasyPaymentCancellationFailedAttribute | A cancellation has failed. | PaymentCancellationFailed |

## How url webhook endpoints are constructed
The webhook attributes uses named routes. It is important that each route in the application are unique. Using the `NetsEasyOption.BaseUrl`-property in combination with the webhook url route name a full path is constructed. To this url path 2 additional parameters are inserted:

* a complement,
* and a nonce

## What is a Complement and a nonce?
The Authorization header is calculated using HMAC-SHA256 which gives an output of 256 bits.  
Since Nets restricts header value to only alphanumeric values, the 256 bits hash is encoded into a Base62 (yes base62).  
And because Nets restricts the header length to 32 characters any remaining characters are just dumped into the `Complement`.

A nonce is a random string used to prevent replay-attacks.

## Step-by-step Authorization header creation
1. Determine what event the user wants to subscribe to, so that we can know what properties we will know beforehand.
2. Use the info (invariant) with a random nonce to calculate HMAC-SHA256 using the `System.Security.Cryptography.HMACSHA256` class with a key from the `SolidNetsEasyClient.Models.Options.WebhookEncryptionOptions.Key` property.
3. Set the Authorization to the first 32 characters.
4. Construct a URL to the correct webhook endpoint using route name, by attribute.
5. Add the remaining complement and nonce to the constructed URL.

## Validation
1. Combine the Authorization-header with the Complement.
2. Calculate the HMAC-SHA256 for the invariant with the nonce.
3. Check if the calculated hash is the same as in step 1.

## IP-filter
The `SolidNetsEasyIPFilterAttribute` can both whitelist and blacklist ips or ip-ranges (using the CIDR-format). Thus minimally ensuring that any webhook requests are coming from an accepted source.

But it should be kept in mind that an IP can be spoofed easily.

## Security disclaimer
I am not a security expert. I have tried to rely only on known security implementations, from Microsoft. I have not implemented any kind of key rotation mechanism or if this is even applicable.

If you have any suggestions please let me know.

# Options
You can configure options using the afforementioned `NetsEasyOptions` class either by appsettings.json; environment variables or in-code using the `NetConfigurationBuilder`.

You can also configure the webhook options using `WebhookEncryptionOptions` class by same methods as above.


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