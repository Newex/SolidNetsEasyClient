using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly;
using SolidNetsEasyClient.Clients;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.Extensions;

/// <summary>
/// The nets configuration builder
/// </summary>
public sealed class NetsConfigurationBuilder
{
    // private readonly ServiceDescriptor 
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
    public NetsConfigurationBuilder Configure(IConfiguration configuration)
    {
        var section = configuration.GetSection(PlatformPaymentOptions.NetsEasyConfigurationSection);
        services.Configure<PlatformPaymentOptions>(section);
        return this;
    }

    /// <summary>
    /// Configure the options for the nets easy payment requests directly instead of loading them from a configuration source
    /// </summary>
    /// <param name="options">The options to set</param>
    /// <returns>A builder object</returns>
    public NetsConfigurationBuilder Configure(Action<PlatformPaymentOptions> options)
    {
        services.Configure(options);
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

        if (transientHttpErrorPolicy is not null)
        {
            services
                .AddHttpClient(clientMode, options)
                .AddTransientHttpErrorPolicy(transientHttpErrorPolicy);
        }
        else
        {
            services.AddHttpClient(clientMode, options);
        }
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

        services.AddHttpClient(clientMode).AddTransientHttpErrorPolicy(transientHttpErrorPolicy);
        return this;
    }

    internal static NetsConfigurationBuilder Create(IServiceCollection services)
    {
        var instance = new NetsConfigurationBuilder(services);

        // Add http clients
        services.AddHttpClient(ClientConstants.Live, client => client.BaseAddress = NetsEndpoints.LiveBaseUri);
        services.AddHttpClient(ClientConstants.Test, client => client.BaseAddress = NetsEndpoints.TestingBaseUri);

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
        services.Configure<PlatformPaymentOptions>(_ => { });

        return instance;
    }
}
