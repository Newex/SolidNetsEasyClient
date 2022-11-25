using System;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.Clients;

/// <summary>
/// Unscheduled subscription client, responsible for mediating communication between NETS payment API and this client
/// </summary>
/// <remarks>
/// Unscheduled subscriptions allow you to charge your customers at an unscheduled time interval with a variable amount, for example an automatic top-up agreement for a rail-card when the consumer drops below a certain stored value. When an unscheduled subscription is charged, a new payment object is created to represent the purchase of the unscheduled subscription product. It is possible to verify and charge multiple unscheduled subscriptions in bulk using the Bulk charge unscheduled subscriptions method.
/// </remarks>
public class UnscheduledSubscriptionClient
{
    private readonly IHttpClientFactory httpClientFactory;

    private readonly ILogger<UnscheduledSubscriptionClient> logger;
    private readonly string mode;
    private readonly string apiKey;

    /// <summary>
    /// Instantiate a new object of <see cref="UnscheduledSubscriptionClient"/>
    /// </summary>
    /// <param name="options">The options</param>
    /// <param name="httpClientFactory">The http client factory</param>
    /// <param name="logger">The optional logger</param>
    /// <exception cref="NotSupportedException"></exception>
    public UnscheduledSubscriptionClient(
        IOptions<PlatformPaymentOptions> options,
        IHttpClientFactory httpClientFactory,
        ILogger<UnscheduledSubscriptionClient>? logger = null
    )
    {
        this.httpClientFactory = httpClientFactory;
        this.logger = logger ?? NullLogger<UnscheduledSubscriptionClient>.Instance;
        mode = options.Value.ClientMode switch
        {
            ClientMode.Live => ClientConstants.Live,
            ClientMode.Test => ClientConstants.Test,
            _ => throw new NotSupportedException("Client mode must be either in Live or Test mode")
        };
        apiKey = options.Value.ApiKey;
    }

    /// <summary>
    /// Retrieves an unscheduled subscription matching the specified externalReference. This method can only be used for retrieving unscheduled subscriptions that have been imported from a payment platform other than Nets Easy. Unscheduled subscriptions created within Nets Easy do not have an externalReference value set.
    /// </summary>
    /// <param name="externalReference">The external reference to search for.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Information about the unscheduled subscription</returns>
    /// <exception cref="ArgumentException">Thrown if external reference is empty or null</exception>
    /// <exception cref="HttpRequestException">Thrown if response is not successfull</exception>
    public async Task<UnscheduledSubscriptionDetails> RetrieveUnscheduledSubscriptionByExternalReferenceAsync(string externalReference, CancellationToken cancellationToken)
    {
        var isValid = !string.IsNullOrWhiteSpace(externalReference);
        if (!isValid)
        {
            logger.LogError("Invalid {ExternalReference}", externalReference);
            throw new ArgumentException("External cannot be empty or only whitespaces", nameof(externalReference));
        }

        try
        {
            logger.LogTrace("Retrieving unscheduled subscription with {ExternalReference}", externalReference);
            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(ref client);

            var path = NetsEndpoints.Relative.UnscheduledSubscriptions + $"?externalReference={HttpUtility.UrlEncode(externalReference)}";
            var response = await client.GetAsync(path, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogDebug("Raw response is: {ResponseContent}", content);
            _ = response.EnsureSuccessStatusCode();
            var result = JsonSerializer.Deserialize<UnscheduledSubscriptionDetails>(content);
            if (result is null)
            {
                logger.LogError("Could not deserialize response from {@HttpClient} with {Content} to UnscheduledSubscriptionDetails", client, content);
                throw new SerializationException("Could not deserialize response to UnscheduledSubscriptionDetails object");
            }

            logger.LogInformation("Retrieved unscheduled subscription {@UnscheduledSubscription}", result);
            return result;
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "An exception occurred trying to retrieve unscheduled subscription with {ExternalReference}", externalReference);
            throw;
        }
    }

    private void AddHeaders(ref HttpClient client)
    {
        client.DefaultRequestHeaders.Add(HeaderNames.Authorization, apiKey);
    }
}
