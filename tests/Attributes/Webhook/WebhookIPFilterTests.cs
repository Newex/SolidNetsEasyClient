using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using SolidNetsEasyClient.Filters;
using SolidNetsEasyClient.Models.Options;
using SolidNetsEasyClient.Tests.Tools;

using static Microsoft.AspNetCore.Http.StatusCodes;

namespace SolidNetsEasyClient.Tests.Attributes.Webhook;

[UnitTest]
public class WebhookIPFilterTests
{
    [Fact]
    public void Denied_IP_returns_403_result()
    {
        // Arrange
        const string ipString = "192.168.1.1";
        var options = Options.Create<PlatformPaymentOptions>(new()
        {
            BlacklistIPsForWebhook = $"127.0.0.1;{ipString}"
        });
        var httpContext = Mocks.HttpContext(IPAddress.Parse(ipString), "POST", serviceDefinitions: (typeof(IOptions<PlatformPaymentOptions>), options));
        var actionContext = new ActionContext(
            httpContext,
            new Microsoft.AspNetCore.Routing.RouteData(),
            new ActionDescriptor());
        var filters = new List<IFilterMetadata>();
        // var context = new ResourceExecutingContext(actionContext, filters, Enumerable.Empty<IValueProviderFactory>().ToList());
        var context = new AuthorizationFilterContext(actionContext, filters);
        var attribute = new WebhookIPFilterAttribute();

        // Act
        attribute.OnAuthorization(context);
        var actual = context.Result;

        // Assert
        actual.Should().Match<StatusCodeResult>(x => x.StatusCode == Status403Forbidden);
    }

    [Fact]
    public void Allowed_IP_should_not_return_403_result()
    {
        // Arrange
        const string ipString = "192.168.1.1";
        var options = Options.Create<PlatformPaymentOptions>(new()
        {
            NetsIPWebhookEndpoints = $"{ipString}/24"
        });
        var httpContext = Mocks.HttpContext(IPAddress.Parse(ipString), "POST", serviceDefinitions: (typeof(IOptions<PlatformPaymentOptions>), options));
        var actionContext = new ActionContext(
            httpContext,
            new Microsoft.AspNetCore.Routing.RouteData(),
            new ActionDescriptor());
        var filters = new List<IFilterMetadata>();
        // var context = new ResourceExecutingContext(actionContext, filters, Enumerable.Empty<IValueProviderFactory>().ToList());
        var context = new AuthorizationFilterContext(actionContext, filters);
        var attribute = new WebhookIPFilterAttribute();

        // Act
        attribute.OnAuthorization(context);
        var actual = context.Result;

        // Assert
        actual.Should().BeNull();
    }
}
