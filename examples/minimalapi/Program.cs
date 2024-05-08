using SolidNetsEasyClient.Builder;
using SolidNetsEasyClient.Clients;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.Extensions;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddNetsEasy(options =>
{
    options.ApiKey = "my-api-key";
    options.IntegrationType = Integration.EmbeddedCheckout;
    options.CheckoutKey = "my-checkout-key";
    options.CheckoutUrl = "https://localhost/checkout";
    options.TermsUrl = "https://localhost/terms";
    options.PrivacyPolicyUrl = "https://localhost/privacy";
    options.ClientMode = ClientMode.Test;
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    // options.SerializerOptions.TypeInfoResolverChain.Add(WebhookSerializationContext.Default);
    options.SerializerOptions.Converters.Add(new IWebhookConverter());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/checkout", (NetsPaymentBuilder builder, NetsPaymentClient client, CancellationToken cancellationToken) =>
{
    // var paymentBuilder = builder.CreateSinglePayment(order, "my-order-id");
    // paymentBuilder.AddWebhook("https://localhost:8000/nets/webhook", EventName.PaymentCreated, "authHeaderVal123");

    // var paymentRequest = paymentBuilder.Build();
    // var paymentResult = await client.StartCheckoutPayment(paymentRequest, cancellationToken);
    // return TypedResults.Created("/payment/my-order-id", paymentResult);
    throw new NotImplementedException();
});

// Must be POST
app.MapPost("/nets/webhook", (HttpContext context, NetsPaymentClient client) =>
{
    var authHeader = context.Request.Headers.Authorization;
    if (string.IsNullOrEmpty(authHeader))
    {
        return Results.Forbid();
    }
    var isValid = string.Equals("authHeaderVal123", authHeader);
    if (!isValid)
    {
        return Results.Forbid();
    }

    throw new NotImplementedException();
});

app.Run();