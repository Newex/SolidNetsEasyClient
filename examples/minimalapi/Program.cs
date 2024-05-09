using Microsoft.AspNetCore.Mvc;
using MinimalAPI.SerializationContexts;
using SolidNetsEasyClient.Builder;
using SolidNetsEasyClient.Clients;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Extensions;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
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
    options.DefaultDenyWebhook = false;
    options.WhitelistIPsForWebhook = "127.0.0.1";
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Add(OrderSerializationContext.Default);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/checkout", async (NetsPaymentBuilder builder, NetsPaymentClient client, [FromBody] Order order, CancellationToken cancellationToken) =>
{
    var paymentBuilder = builder.CreateSinglePayment(order, "my-order-id");
    paymentBuilder.AddWebhook("https://localhost:8000/nets/webhook", EventName.PaymentCreated, "authHeaderVal123");

    var paymentRequest = paymentBuilder.Build();
    var paymentResult = await client.StartCheckoutPayment(paymentRequest, cancellationToken);
    return TypedResults.Created("/payment/my-order-id", paymentResult);
});

app.MapNetsWebhook("/nets/webhook", (HttpContext context, NetsPaymentClient client) =>
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