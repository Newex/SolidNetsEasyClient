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
builder.Services.AddNetsEasy(options =>
{

});

var app = builder.Build();

app.MapHealthChecks("/healthz");
app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}"
);

app.Run();
