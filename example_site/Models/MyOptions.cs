namespace ExampleSite.Models;

public record MyOptions
{
    public string WebhookCallbackUrl { get; set; } = string.Empty;

    public const string Section = "MySectionExample";
}
