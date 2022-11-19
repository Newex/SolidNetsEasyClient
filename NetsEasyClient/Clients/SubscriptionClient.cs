using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;
using SolidNetsEasyClient.Models.DTOs.Requests.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;
using SolidNetsEasyClient.Models.Options;
using SolidNetsEasyClient.Validators;

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

    public async Task<SubscriptionDetails> RetrieveSubscriptionByExternalReferenceAsync(string externalReference, CancellationToken cancellationToken)
    {
        var isValid = !string.IsNullOrWhiteSpace(externalReference);
        if (!isValid)
        {
            logger.LogError("Invalid {ExternalReference}", externalReference);
            throw new ArgumentException("External reference cannot be empty", nameof(externalReference));
        }

        try
        {
            logger.LogTrace("Retrieving subscription with external reference {ID}", externalReference);
            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(ref client);

            var path = NetsEndpoints.Relative.Subscription + $"?externalReference={externalReference}";
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
            logger.LogError(ex, "An exception occurred trying to retrieve subscription with {@ExternalReference}", externalReference);
            throw;
        }
        throw new NotImplementedException();
    }

    public async Task<BulkId> BulkChargeSubscriptionsAsync(IList<SubscriptionCharge> subscriptions, CancellationToken cancellationToken, string? externalBulkChargeId = null, Notification? notifications = null)
    {
        // Subscription must either have a subscriptionId or an externalReference but not both!
        var isValid = subscriptions.All(SubscriptionValidator.OnlyEitherSubscriptionIdOrExternalRef) && subscriptions.Count > 0;
        if (!isValid)
        {
            logger.LogError("Invalid {@Subscriptions}", subscriptions);
            throw new ArgumentException("Must have at least 1 subscription where each subscription can only have either an id or an external reference not both");
        }

        try
        {
            logger.LogTrace("Bulk charge {@Subscriptions}", subscriptions);
            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(ref client);

            var path = NetsEndpoints.Relative.Subscription + "/charges";
            var bulk = new BulkCharge
            {
                ExternalBulkChargeId = externalBulkChargeId,
                Notifications = notifications,
                Subscriptions = subscriptions
            };
            var response = await client.PostAsJsonAsync(path, bulk, cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<BulkId>();
            if (result is null)
            {
                logger.LogError("Could not deserialize response from {@HttpClient}", client);
                throw new Exception("Could not deserialize response from the http client to a BulkId");
            }
            logger.LogInformation("Bulk charge id: {@BulkChargeId}", result);
            return result;
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "An exception occurred trying to bulk charge {@Subscriptions}", subscriptions);
            throw;
        }
    }

    public Task<object> RetrieveBulkVerificationForUnscheduledSubscriptionsAsync(Guid bulkId)
    {
        throw new NotImplementedException();
    }

    private void AddHeaders(ref HttpClient client)
    {
        client.DefaultRequestHeaders.Add(HeaderNames.Authorization, apiKey);
    }
}