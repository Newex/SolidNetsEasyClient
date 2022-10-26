using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SolidNetsEasyClient.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddNetsEasyClient();

var app = builder.Build();

app.MapDefaultControllerRoute();

app.Run();
