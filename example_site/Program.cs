using ExampleSite.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidNetsEasyClient.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddFilter((_, category, level) =>
{
    if (!category.Contains("Microsoft.Asp") || !category.Contains("Microsoft.Extensions"))
    {
        return true;
    }
    else
    {
        return level >= LogLevel.Warning;
    }
});

builder.Services.AddHealthChecks();

builder.Services.AddControllersWithViews();

// nets easy
builder
.Services
.AddNetsEasyClient()
.Configure(builder.Configuration);

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("MyCorsPolicy", corsBuilder => corsBuilder.SetIsOriginAllowedToAllowWildcardSubdomains()
//     .WithOrigins("*")
//     .AllowAnyMethod()
//     .AllowCredentials()
//     .AllowAnyHeader()
//     .Build());
// });

builder.Services.Configure<MyOptions>(builder.Configuration.GetSection(MyOptions.Section));

var app = builder.Build();
// app.UseCors("MyCorsPolicy");

app.MapHealthChecks("/healthz");
app.UseStaticFiles();
app.MapDefaultControllerRoute();

app.Run();
