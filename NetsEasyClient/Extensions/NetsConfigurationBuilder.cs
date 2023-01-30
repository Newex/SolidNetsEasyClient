using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly;
using SolidNetsEasyClient.Builder;
using SolidNetsEasyClient.Clients;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Helpers.Encryption;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.Extensions;

/// <summary>
/// The nets configuration builder
/// </summary>
public sealed class NetsConfigurationBuilder
{
    /// <summary>
    /// private readonly ServiceDescriptor
    /// </summary>
    private readonly IServiceCollection services;

    private NetsConfigurationBuilder(IServiceCollection services)
    {
        this.services = services;
    }

    /// <summary>
    /// Add an API key, commerce platform tag and set the client operating mode from a configuration source, such as: environment variable or appsettings.json file
    /// </summary>
    /// <param name="configuration">The configuration object</param>
    /// <returns>A builder object</returns>
    public NetsConfigurationBuilder ConfigureFromConfiguration(IConfiguration configuration)
    {
        var section = configuration.GetSection(NetsEasyOptions.NetsEasyConfigurationSection);
        _ = services.Configure<NetsEasyOptions>(section);
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
    /// Configure the http client for a specific mode
    /// </summary>
    /// <param name="mode">The client mode</param>
    /// <param name="options">The http client options</param>
    /// <param name="transientHttpErrorPolicy">The retry policy configuration for the Polly library</param>
    /// <returns>A builder object</returns>
    /// <exception cref="NotSupportedException">If client mode is not supported</exception>
    public NetsConfigurationBuilder ConfigureHttpClient(ClientMode mode, Action<HttpClient> options, Func<PolicyBuilder<HttpResponseMessage>, IAsyncPolicy<HttpResponseMessage>>? transientHttpErrorPolicy = null)
    {
        var clientMode = mode switch
        {
            ClientMode.Live => ClientConstants.Live,
            ClientMode.Test => ClientConstants.Test,
            _ => throw new NotSupportedException()
        };

        _ = transientHttpErrorPolicy is not null
            ? services
                .AddHttpClient(clientMode, options)
                .AddTransientHttpErrorPolicy(transientHttpErrorPolicy)
            : services.AddHttpClient(clientMode, options);
        return this;
    }

    /// <summary>
    /// Configure the http client transient retry policy for a specific mode
    /// </summary>
    /// <param name="mode">The client mode</param>
    /// <param name="transientHttpErrorPolicy">The retry policy configuration using the Polly library</param>
    /// <returns>A builder object</returns>
    /// <exception cref="NotSupportedException">Thrown if client mode is not supported</exception>
    public NetsConfigurationBuilder ConfigureRetryPolicy(ClientMode mode, Func<PolicyBuilder<HttpResponseMessage>, IAsyncPolicy<HttpResponseMessage>> transientHttpErrorPolicy)
    {
        var clientMode = mode switch
        {
            ClientMode.Live => ClientConstants.Live,
            ClientMode.Test => ClientConstants.Test,
            _ => throw new NotSupportedException()
        };

        _ = services.AddHttpClient(clientMode).AddTransientHttpErrorPolicy(transientHttpErrorPolicy);
        return this;
    }

    /// <summary>
    /// Configure the webhook encryption options
    /// </summary>
    /// <param name="configure">The options to configure</param>
    /// <returns>A builder object</returns>
    public NetsConfigurationBuilder ConfigureEncryptionOptions(Action<WebhookEncryptionOptions> configure)
    {
        _ = services.Configure(configure);
        return this;
    }

    internal static NetsConfigurationBuilder Create(IServiceCollection services)
    {
        var instance = new NetsConfigurationBuilder(services);

        // Factories
        services.TryAddScoped<NetsPaymentFactory>();

        // Add http clients
        _ = services.AddHttpClient(ClientConstants.Live, client => client.BaseAddress = NetsEndpoints.LiveBaseUri);
        _ = services.AddHttpClient(ClientConstants.Test, client => client.BaseAddress = NetsEndpoints.TestingBaseUri);

        // Add payment client
        services.TryAddScoped<IPaymentClient, PaymentClient>();
        services.TryAddScoped(typeof(PaymentClient));

        // Add subscription client
        services.TryAddScoped<ISubscriptionClient, SubscriptionClient>();
        services.TryAddScoped(typeof(SubscriptionClient));

        // Add unscheduled subscription client
        services.TryAddScoped<IUnscheduledSubscriptionClient, UnscheduledSubscriptionClient>();
        services.TryAddScoped(typeof(UnscheduledSubscriptionClient));

        // Add payment options
        _ = services.Configure<NetsEasyOptions>(_ => { });

        // Add default encryption option
        _ = services.Configure<WebhookEncryptionOptions>(c => c.Hasher = new HmacSHA256Hasher());

        return instance;
    }
}
