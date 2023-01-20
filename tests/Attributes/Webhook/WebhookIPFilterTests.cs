using Microsoft.AspNetCore.Mvc;
using SolidNetsEasyClient.Filters;

using static Microsoft.AspNetCore.Http.StatusCodes;
using Auth = SolidNetsEasyClient.Tests.Attributes.Webhook.AuthorizationFilterContextBuilder;

namespace SolidNetsEasyClient.Tests.Attributes.Webhook;

[UnitTest]
public class WebhookIPFilterTests
{
    [Fact]
    public void Denied_IP_returns_403_result()
    {
        // Arrange
        const string ipString = "192.168.1.1";
        var builder = Auth
            .Create(fromIP: ipString)
            .AddOptions(new() { BlacklistIPsForWebhook = $"127.0.0.1;{ipString}" });
        var context = builder.Build();
        var attribute = new SolidNetsEasyIPFilterAttribute();

        // Act
        attribute.OnAuthorization(context);
        var actual = context.Result;

        // Assert
        actual.Should().Match<StatusCodeResult>(x => x.StatusCode == Status403Forbidden);
    }

    [Fact]
    public void Property_blacklist_should_take_precedence_over_configured_blacklist()
    {
        // Arrange
        const string ipString = "192.168.1.1";
        var builder = Auth
            .Create(fromIP: ipString)
            .AddOptions(new() { BlacklistIPsForWebhook = "127.0.0.1" });
        var context = builder.Build();
        var attribute = new SolidNetsEasyIPFilterAttribute()
        {
            BlacklistIPs = ipString
        };

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
        var builder = Auth
            .Create(fromIP: ipString)
            .AddOptions(new() { NetsIPWebhookEndpoints = $"{ipString}/24", });
        var context = builder.Build();
        var attribute = new SolidNetsEasyIPFilterAttribute();

        // Act
        attribute.OnAuthorization(context);
        var actual = context.Result;

        // Assert
        actual.Should().BeNull();
    }

    [Fact]
    public void Blaclist_takes_precedence_over_the_whitelist()
    {
        // Arrange
        const string ipString = "192.168.1.1";
        var builder = Auth
            .Create(fromIP: ipString)
            .AddOptions(new()
            {
                BlacklistIPsForWebhook = ipString,
                NetsIPWebhookEndpoints = $"{ipString}/24",
            });
        var context = builder.Build();
        var attribute = new SolidNetsEasyIPFilterAttribute()
        {
            WhitelistIPs = ipString
        };

        // Act
        attribute.OnAuthorization(context);
        var actual = context.Result;

        // Assert
        actual.Should().Match<StatusCodeResult>(x => x.StatusCode == Status403Forbidden);
    }
}
