using Microsoft.AspNetCore.Builder;
using SolidNetsEasyClient.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add nets
builder.Services.AddNetsEasyClient();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
