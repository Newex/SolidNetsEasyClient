using MinimalAPI.Models;
using MinimalAPI.SerializationContexts;
using SolidNetsEasyClient.Builder;
using SolidNetsEasyClient.Clients;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Extensions;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Payloads;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddNetsEasy(options =>
{
    options.IntegrationType = Integration.EmbeddedCheckout;
    options.CheckoutUrl = "https://localhost:5110/checkout";
    options.TermsUrl = "https://localhost:5110/terms";
    options.PrivacyPolicyUrl = "https://localhost:5110/privacy";
    options.ClientMode = ClientMode.Test;
    options.DefaultDenyWebhook = false;
    options.WhitelistIPsForWebhook = "127.0.0.1";
})
.ConfigureFromConfiguration(builder.Configuration);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Add(OrderSerializationContext.Default);
    options.SerializerOptions.TypeInfoResolverChain.Add(ProductSerializationContext.Default);
});

var app = builder.Build();

app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapFallbackToFile("index.html");

app.MapPost("/checkout", async (NetsPaymentBuilder builder, ICheckoutClient client, Product product, CancellationToken cancellationToken) =>
{
    var order = new Order
    {
        Currency = product.Currency,
        Items = [
            new()
            {
                Name = product.Name,
                Quantity = 1,
                Reference = product.ID.ToString("N"),
                TaxRate = 0,
                Unit = product.Unit,
                UnitPrice = product.Price,
            },
        ],
        Reference = "my-order-id"
    };
    var paymentBuilder = builder.CreateSinglePayment(order, "my-payment-id");
    paymentBuilder.AddWebhook("https://localhost:5110/nets/webhook", EventName.PaymentCreated, "authHeaderVal123");

    var paymentRequest = paymentBuilder.Build();
    var paymentResult = await client.StartCheckoutPayment(paymentRequest, cancellationToken);

    return TypedResults.Created("/payment/my-payment-id", new
    {
        CheckoutKey = client.CheckoutKey,
        PaymentId = paymentResult?.PaymentId.ToString("N")
    });
});

app.MapNetsWebhook("/nets/webhook", (HttpContext context, IWebhook<WebhookData> payload) =>
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

    return Results.NoContent();
});

app.Run();