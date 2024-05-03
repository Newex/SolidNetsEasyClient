using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SolidNetsEasyClient.Clients;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.DelegatingHandlers;
using SolidNetsEasyClient.Helpers.Encryption;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.Builder;

/// <summary>
/// The nets configuration builder
/// </summary>
public sealed class NetsConfigurationBuilder
{
    /// <summary>
    /// private readonly ServiceDescriptor
    /// </summary>
    private readonly IServiceCollection services;
    private readonly IHttpClientBuilder httpBuilder;

    private NetsConfigurationBuilder(IServiceCollection services, IHttpClientBuilder httpBuilder)
    {
        this.services = services;
        this.httpBuilder = httpBuilder;
    }

    /// <summary>
    /// Add an API key, commerce platform tag and set the client operating mode from a configuration source, such as: environment variable or appsettings.json file.
    /// Furthermore if the configuration properties contain webhook settings, these will be configured.
    /// </summary>
    /// <param name="configuration">The configuration object</param>
    /// <returns>A builder object</returns>
    public NetsConfigurationBuilder ConfigureFromConfiguration(IConfiguration configuration)
    {
        var section = configuration.GetSection(NetsEasyOptions.NetsEasyConfigurationSection);
        _ = services.Configure<NetsEasyOptions>(section);

        var webhookSection = configuration.GetSection(WebhookEncryptionOptions.WebhookEncryptionConfigurationSection);
        _ = services.Configure<WebhookEncryptionOptions>(webhookSection);
        return this;
    }

    /// <summary>
    /// Configure the options for the nets easy payment requests directly instead of loading them from a configuration source
    /// </summary>
    /// <param name="options">The options to set</param>
    /// <returns>A builder object</returns>
    public NetsConfigurationBuilder ConfigureNetsEasyOptions(Action<NetsEasyOptions> options)
    {
        _ = services.Configure(options);
        return this;
    }

    /// <summary>
    /// Configure the http client factory builder for NETS.
    /// </summary>
    /// <param name="configure">The configuration action</param>
    /// <returns>A builder object</returns>
    public NetsConfigurationBuilder ConfigureHttpClientFactory(Action<IHttpClientBuilder> configure)
    {
        configure(httpBuilder);
        return this;
    }

    /// <summary>
    /// Configure the typed http client, for NETS.
    /// </summary>
    /// <param name="configure">The configuration action</param>
    /// <returns>A builder object</returns>
    public NetsConfigurationBuilder ConfigureHttpClient(Action<HttpClient> configure)
    {
        httpBuilder.ConfigureHttpClient(configure);
        return this;
    }

    /// <summary>
    /// Configure the webhook encryption options from a configuration file
    /// </summary>
    /// <param name="configuration">The configuration properties</param>
    /// <returns>A builder object</returns>
    public NetsConfigurationBuilder ConfigureEncryptionOptions(IConfiguration configuration)
    {
        var section = configuration.GetSection(WebhookEncryptionOptions.WebhookEncryptionConfigurationSection);
        _ = services.Configure<WebhookEncryptionOptions>(section);
        return this;
    }

    internal static NetsConfigurationBuilder Create(IServiceCollection services)
    {
        // Add options
        _ = services.Configure<WebhookEncryptionOptions>(c => c.Hasher = new HmacSHA256Hasher());

        // Add factories

        // Add http clients
        var httpbuilder = services.AddHttpClient<NetsPaymentClient>((provider, client) =>
        {
            var opt = provider.GetRequiredService<NetsEasyOptions>();

            var baseUrl = opt.ClientMode switch
            {
                ClientMode.Test => NetsEndpoints.TestingBaseUri,
                ClientMode.Live => NetsEndpoints.LiveBaseUri,
                _ => throw new InvalidOperationException("Mode not supported")
            };
            client.BaseAddress = baseUrl;
        }) // Pipeline
            .AddHttpMessageHandler<NetsAuthorizationHandler>();

        // Add payment client
        services.TryAddScoped<IPaymentClient, PaymentClient>();
        services.TryAddScoped(typeof(PaymentClient));
        services.TryAddScoped<NetsPaymentBuilder>();

        // Add subscription client
        services.TryAddScoped<ISubscriptionClient, SubscriptionClient>();
        services.TryAddScoped(typeof(SubscriptionClient));

        // Add unscheduled subscription client
        services.TryAddScoped<IUnscheduledSubscriptionClient, UnscheduledSubscriptionClient>();
        services.TryAddScoped(typeof(UnscheduledSubscriptionClient));

        return new NetsConfigurationBuilder(services, httpbuilder);
    }
}
