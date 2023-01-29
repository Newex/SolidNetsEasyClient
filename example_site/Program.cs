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
    if (category?.Contains("Microsoft.Asp") == false && !category.Contains("Microsoft.Extensions"))
    {
        return true;
    }

    return level >= LogLevel.Warning;
});

builder.Services.AddHealthChecks();

builder.Services.AddControllersWithViews();

// Nets Easy
builder
    .Services
    .AddNetsEasyClient()
    .ConfigureEncryptionOptions(opt =>
    {
        // Key rotation is not implemented - so do not leak key
        opt.Key = new byte[10]
        {
            0x53,
            0x31,
            0xC8,
            0xAF,
            0x6A,
            0xF3,
            0x6D,
            0xFA,
            0x5B,
            0xCC
        };
    })
    .ConfigureFromConfiguration(builder.Configuration);

builder.Services.Configure<MyOptions>(builder.Configuration.GetSection(MyOptions.Section));

var app = builder.Build();

app.MapHealthChecks("/healthz");
app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}"
);

app.Run();
