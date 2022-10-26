using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SolidNetsEasyClient.Clients;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.Extensions;

public sealed class NetsConfigurationBuilder
{
    private readonly IServiceCollection services;

    private NetsConfigurationBuilder(IServiceCollection services)
    {
        this.services = services;
    }

    public NetsConfigurationBuilder AddApiKey(IConfiguration configuration)
    {
        // TODO: load api key from configuration
        return this;
    }

    public NetsConfigurationBuilder AddApiKey(string apiKey)
    {
        // TODO: load api key
        return this;
    }

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
