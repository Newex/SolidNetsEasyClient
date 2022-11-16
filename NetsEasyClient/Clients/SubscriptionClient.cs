using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.Clients;

public class SubscriptionClient
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ILogger<SubscriptionClient> logger;
    private readonly string mode;
    private readonly string apiKey;

    public SubscriptionClient(
        IOptions<PlatformPaymentOptions> options,
        IHttpClientFactory httpClientFactory,
        ILogger<SubscriptionClient>? logger = null
    )
    {
        this.httpClientFactory = httpClientFactory;
        this.logger = logger ?? NullLogger<SubscriptionClient>.Instance;
        mode = options.Value.ClientMode switch
        {
            ClientMode.Live => ClientConstants.Live,
            ClientMode.Test => ClientConstants.Test,
            _ => throw new NotSupportedException("Client mode must be either in Live or Test mode")
        };
        apiKey = options.Value.ApiKey;
    }

    public async Task<SubscriptionDetails> RetrieveSubscriptionAsync(Guid subscriptionId, CancellationToken cancellationToken)
    {
        var isValid = subscriptionId != Guid.Empty;
        if (!isValid)
        {
            logger.LogError("Invalid {SubscriptionID}", subscriptionId);
            throw new ArgumentException("Guid cannot be empty", nameof(subscriptionId));
        }

        try
        {
            logger.LogTrace("Retrieving subscription with {ID}", subscriptionId);
            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(ref client);

            var path = NetsEndpoints.Relative.Subscription + $"/{subscriptionId}";
            var result = await client.GetFromJsonAsync<SubscriptionDetails>(path, cancellationToken);

            if (result is null)
            {
                logger.LogError("Could not deserialize response from {@HttpClient}", client);
                throw new Exception("Could not deserialize response from the http client to a SubscriptionDetails");
            }

            logger.LogInformation("Retrieved {@Subscription}", result);
            return result;
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "An exception occurred trying to retrieve subscription with {@SubscriptionID}", subscriptionId);
            throw;
        }
    }

    private void AddHeaders(ref HttpClient client)
    {
        client.DefaultRequestHeaders.Add(HeaderNames.Authorization, apiKey);
    }
}
