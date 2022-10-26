using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SolidNetsEasyClient.Clients;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.Extensions;

/// <summary>
/// The nets configuration builder
/// </summary>
public sealed class NetsConfigurationBuilder
{
    private readonly IServiceCollection services;

    private NetsConfigurationBuilder(IServiceCollection services)
    {
        this.services = services;
    }

    /// <summary>
    /// Add an API key from a configuration source, such as: Environment variable
    /// </summary>
    /// <param name="configuration">The configuration object</param>
    /// <returns>A builder object</returns>
    public NetsConfigurationBuilder AddApiKey(IConfiguration configuration)
    {
        // TODO: load api key from configuration
        return this;
    }

    /// <summary>
    /// Add an API key directly
    /// </summary>
    /// <param name="apiKey">The API key to use for each request</param>
    /// <returns>A builder object</returns>
    public NetsConfigurationBuilder AddApiKey(string apiKey)
    {
        // TODO: load api key
        return this;
    }

    /// <summary>
    /// Configure the options for the nets easy payment requests
    /// </summary>
    /// <param name="options">The options to set</param>
    /// <returns>A builder object</returns>
    public NetsConfigurationBuilder Configure(Action<PlatformPaymentOptions> options)
    {
        services.Configure(options);
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

        // Add payment options
        services.Configure<PlatformPaymentOptions>(_ => { });

        return instance;
    }
}
