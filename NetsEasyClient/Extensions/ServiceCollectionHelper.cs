using System;
using Microsoft.Extensions.DependencyInjection;
using SolidNetsEasyClient.Builder;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.Extensions;

/// <summary>
/// Extension methods for the <see cref="IServiceCollection"/> class
/// </summary>
public static class ServiceCollectionHelper
{
    /// <summary>
    /// Add nets easy client to site
    /// </summary>
    /// <param name="services">The services</param>
    /// <returns>A builder configuration object</returns>
    public static NetsConfigurationBuilder AddNetsEasyClient(this IServiceCollection services)
    {
        return NetsConfigurationBuilder.Create(services);
    }

    /// <summary>
    /// Add nets easy client to the site
    /// </summary>
    /// <param name="services">The services</param>
    /// <param name="options">The nets easy options</param>
    /// <returns>A builder configuration object</returns>
    public static NetsConfigurationBuilder AddNetsEasyClient(this IServiceCollection services, Action<NetsEasyOptions> options)
    {
        return NetsConfigurationBuilder.Create(services).ConfigureNetsEasyOptions(options);
    }
}
