using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SolidNetsEasyClient.Clients;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Converters;
using SolidNetsEasyClient.DelegatingHandlers;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;
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
    private readonly OptionsBuilder<NetsEasyOptions> optionsBuilder;

    private NetsConfigurationBuilder(IServiceCollection services, IHttpClientBuilder httpBuilder, OptionsBuilder<NetsEasyOptions> optionsBuilder)
    {
        this.services = services;
        this.httpBuilder = httpBuilder;
        this.optionsBuilder = optionsBuilder;
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
        optionsBuilder.Bind(section);
        return this;
    }

    /// <summary>
    /// Configure the options for the nets easy payment requests directly instead of loading them from a configuration source
    /// </summary>
    /// <param name="options">The options to set</param>
    /// <returns>A builder object</returns>
    public NetsConfigurationBuilder ConfigureNetsEasyOptions(Action<NetsEasyOptions> options)
    {
        optionsBuilder.Configure(options);
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

    internal static NetsConfigurationBuilder Create(IServiceCollection services)
    {
        // Add options
        services.ConfigureHttpJsonOptions(options =>
        {
            // Add all the converters for webhook
            options.SerializerOptions.Converters.Add(new IWebhookConverter());
            options.SerializerOptions.Converters.Add(new PaymentCreatedConverter());
            options.SerializerOptions.Converters.Add(new PaymentCancelledConverter());
            options.SerializerOptions.Converters.Add(new ChargeCreatedConverter());
            options.SerializerOptions.Converters.Add(new CheckoutCompletedConverter());
            options.SerializerOptions.Converters.Add(new PaymentCancellationFailedConverter());
            options.SerializerOptions.Converters.Add(new RefundCompletedConverter());
            options.SerializerOptions.Converters.Add(new RefundFailedConverter());
            options.SerializerOptions.Converters.Add(new RefundInitiatedConverter());
            options.SerializerOptions.Converters.Add(new ReservationCreatedV1Converter());
            options.SerializerOptions.Converters.Add(new ReservationCreatedV2Converter());
            options.SerializerOptions.Converters.Add(new ReservationFailedConverter());
        });
        var optionsBuilder = services.AddOptions<NetsEasyOptions>().Validate(config =>
        {
            if (string.IsNullOrWhiteSpace(config.ApiKey))
            {
                return false;
            }

            // Conditions:
            var isValid = true;
            if (config.IntegrationType == Integration.EmbeddedCheckout)
            {
                // Embedded
                // 1. CheckoutUrl
                isValid &= !string.IsNullOrWhiteSpace(config.CheckoutUrl);

                // 2. CheckoutKey
                isValid &= !string.IsNullOrWhiteSpace(config.CheckoutKey);
            }
            else if (config.IntegrationType == Integration.HostedPaymentPage)
            {
                // Hosted
                // 1. ReturnUrl
                isValid &= !string.IsNullOrWhiteSpace(config.ReturnUrl);

                // 2. CancelUrl
                isValid &= !string.IsNullOrWhiteSpace(config.CancelUrl);
            }

            // Common
            // 1. TermsUrl
            isValid &= !string.IsNullOrWhiteSpace(config.TermsUrl);

            // 2. PrivacyPolicyUrl
            isValid &= !string.IsNullOrWhiteSpace(config.PrivacyPolicyUrl);

            return isValid;
        }, "NetsEasyOptions is not configured correctly. Either missing or misconfigured options.")
        .ValidateOnStart();

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

        return new NetsConfigurationBuilder(services, httpbuilder, optionsBuilder);
    }
}
