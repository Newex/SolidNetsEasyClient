using SolidNetsEasyClient.Builder;
using SolidNetsEasyClient.Clients;
using SolidNetsEasyClient.Extensions;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddNetsEasyClient(options =>
{
    // The nets easy option configuration
    /**
    MUST set these required option values.
        - The NETS API key

        == IF using EmbeddedCheckout ==
        - CheckoutKey
        - CheckoutUrl
        - TermsUrl
        - PrivacyPolicyUrl

        == IF using HostedCheckout ==
        - ReturnUrl
        - CancelUrl
        - TermsUrl
        - PrivacyPolicyUrl
    */
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
    paymentBuilder.AddWebhook("https://localhost:8000/webhook/callback", EventName.PaymentCreated, "authHeaderVal123");

    var paymentRequest = paymentBuilder.Build();
    var paymentResult = await client.StartCheckoutPayment(paymentRequest, cancellationToken);
    return TypedResults.Created("/payment/my-order-id", paymentResult);
});

app.MapGet("payment/{orderId}", (HttpContext context, NetsPaymentClient client, string orderId) =>
{
    // TODO: implement example
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

    return Results.Ok(orderId);
});

app.Run();