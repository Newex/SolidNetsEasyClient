
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace SolidNetsEasyClient.Extensions;

/// <summary>
/// Helper extension methods for <see cref="IServiceProvider"/>
/// </summary>
public static class ServiceProviderExtensions
{
    /// <summary>
    /// Get the logger for the given class
    /// </summary>
    /// <typeparam name="T">The class type</typeparam>
    /// <param name="services">The context services provider</param>
    /// <returns>A logger</returns>
    public static ILogger<T> GetLogger<T>(IServiceProvider services)
    {
        return (services.GetService(typeof(ILogger<T>)) as ILogger<T>) ?? NullLogger<T>.Instance;
    }

    /// <summary>
    /// Get options of the given type
    /// </summary>
    /// <typeparam name="T">The option type</typeparam>
    /// <param name="services">The context services provider</param>
    /// <returns>An option of T</returns>
    public static IOptions<T>? GetOptions<T>(IServiceProvider services)
        where T : class
    {
        return services.GetService(typeof(IOptions<T>)) as IOptions<T>;
    }
}
