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
    /// Add nets easy client as an embedded checkout. The customer will only interact with your website.
    /// </summary>
    /// <remarks>
    /// Must also remember to set the secret API key, and the checkout key in the <see cref="NetsEasyOptions"/> options.
    /// </remarks>
    /// <param name="services">The services collection</param>
    /// <param name="options">The nets easy options</param>
    /// <returns>A configuration builder</returns>
    public static NetsConfigurationBuilder AddNetsEasy(this IServiceCollection services, Action<NetsEasyOptions>? options = null)
    {
        if (options is not null)
            return NetsConfigurationBuilder.Create(services).ConfigureNetsEasyOptions(options);
        else
            return NetsConfigurationBuilder.Create(services);
    }
}
