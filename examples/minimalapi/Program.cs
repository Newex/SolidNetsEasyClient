using Microsoft.AspNetCore.Mvc;
using SolidNetsEasyClient.Builder;
using SolidNetsEasyClient.Clients;
using SolidNetsEasyClient.Extensions;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddNetsEasyEmbeddedCheckout(checkoutUrl: "https://localhost:8000/checkout",
                                             termsUrl: "https://localhost:8000/terms",
                                             privacyPolicyUrl: "https://localhost:8000/privacy")
.ConfigureNetsEasyOptions(options =>
{
    options.ApiKey = "my-api-key";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/checkout", async (NetsPaymentBuilder builder, NetsPaymentClient client, Order order, CancellationToken cancellationToken) =>
{
    var paymentBuilder = builder.CreateSinglePayment(order, "my-order-id");
    paymentBuilder.AddWebhook("https://localhost:8000/nets/paid", EventName.PaymentCreated, "authHeaderVal123");

    var paymentRequest = paymentBuilder.Build();
    var paymentResult = await client.StartCheckoutPayment(paymentRequest, cancellationToken);
    return TypedResults.Created("/payment/my-order-id", paymentResult);
});

// Must be POST
app.MapPost("/nets/paid", (HttpContext context, NetsPaymentClient client, [FromBody] PaymentCreated paymentCreated) =>
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

    // Handle payment accepted!
    var orderId = paymentCreated.Data.Order.Reference;
    return Results.Ok(orderId);
});

app.Run();