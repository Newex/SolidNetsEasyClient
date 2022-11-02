using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SolidNetsEasyClient.Extensions;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();
// app.UseCors("MyCorsPolicy");

app.MapHealthChecks("/healthz");
app.UseStaticFiles();
app.MapDefaultControllerRoute();

app.Run();
