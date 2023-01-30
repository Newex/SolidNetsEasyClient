using Microsoft.AspNetCore.Mvc;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Filters;
using SolidNetsEasyClient.Models.Options;
using static Microsoft.AspNetCore.Http.StatusCodes;
using Auth = SolidNetsEasyClient.Tests.Attributes.Webhook.AuthorizationFilterContextBuilder;

namespace SolidNetsEasyClient.Tests.Attributes.Webhook;

[UnitTest]
public class WebhookIPFilterTests
{
    private static readonly NetsEasyOptions options = new()
    {
        ClientMode = ClientMode.Test,
        ApiKey = "abc"
    };

    [Fact]
    public void Denied_IP_returns_403_result()
    {
        // Arrange
        const string ipString = "192.168.1.1";
        var builder = Auth
            .Create(fromIP: ipString)
            .AddOptions(options with { BlacklistIPsForWebhook = $"127.0.0.1;{ipString}" });
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
            .AddOptions(options with { BlacklistIPsForWebhook = "127.0.0.1" });
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
            .AddOptions(options with { NetsIPWebhookEndpoints = $"{ipString}/24", });
        var context = builder.Build();
        var attribute = new SolidNetsEasyIPFilterAttribute();

        // Act
        attribute.OnAuthorization(context);
        var actual = context.Result;

        // Assert
        actual.Should().BeNull();
    }

    [Fact]
    public void Blacklist_takes_precedence_over_the_whitelist()
    {
        // Arrange
        const string ipString = "192.168.1.1";
        var builder = Auth
            .Create(fromIP: ipString)
            .AddOptions(options with
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

    [Fact]
    public void Denied_IP_from_proxy_returns_403_result()
    {
        // Arrange
        const string ipString = "192.168.1.1";
        const string actualIP = "77.87.246.118";
        var builder = Auth
            .Create(fromIP: ipString)
            .AddOptions(options with
            {
                BlacklistIPsForWebhook = actualIP
            })
            .AddRequestHeader((HeaderName: "x-forwarded-for", Value: $"{actualIP}, {ipString}"));
        var context = builder.Build();
        var attribute = new SolidNetsEasyIPFilterAttribute();

        // Act
        attribute.OnAuthorization(context);
        var actual = context.Result;

        // Assert
        actual.Should().Match<StatusCodeResult>(x => x.StatusCode == Status403Forbidden);
    }

    [Fact]
    public void IP_not_in_list_is_by_default_denied_returns_403_result()
    {
        // Arrange
        const string clientIP = "77.87.246.118";
        var builder = Auth.Create(fromIP: clientIP).AddOptions(options);
        var context = builder.Build();
        var attribute = new SolidNetsEasyIPFilterAttribute();

        // Act
        attribute.OnAuthorization(context);
        var actual = context.Result;

        // Assert
        actual.Should().Match<StatusCodeResult>(x => x.StatusCode == Status403Forbidden);
    }

    [Fact]
    public void IP_not_in_list_can_be_allowed_by_setting_property()
    {
        // Arrange
        const string clientIP = "77.87.246.118";
        var builder = Auth.Create(fromIP: clientIP).AddOptions(options);
        var context = builder.Build();
        var attribute = new SolidNetsEasyIPFilterAttribute()
        {
            AllowOnlyWhitelistedIPs = false
        };

        // Act
        attribute.OnAuthorization(context);
        var actual = context.Result;

        // Assert
        actual.Should().BeNull();
    }
}
