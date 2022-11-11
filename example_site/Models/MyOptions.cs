using System;

namespace ExampleSite.Models;

public record MyOptions
{
    public string WebhookCallbackUrl { get; set; } = string.Empty;
    public string MySigningKey { get; init; } = string.Empty;

    public const string Section = "MySectionExample";
}
