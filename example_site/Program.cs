using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SolidNetsEasyClient.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// nets easy
builder
.Services
.AddNetsEasyClient()
.Configure(builder.Configuration);

var app = builder.Build();

app.MapDefaultControllerRoute();

app.Run();
