using Microsoft.Extensions.DependencyInjection;
using SolidNetsEasyClient.Builder;

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
}
