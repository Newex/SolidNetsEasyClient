using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;
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

    public async Task<PaginatedSubscriptions> RetrieveBulkChargesAsync(Guid bulkId, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogTrace("Retrieving bulk {BulkId}", bulkId);
            var response = await RetrieveBulkChargesAsync(bulkId, null, null, null, null, cancellationToken);
            var msg = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogTrace("Content is: {@MessageContent}", msg);
            response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<PaginatedSubscriptions>(msg);
            if (result is null)
            {
                logger.LogError("Could not deserialize {@Response} to PaginatedSubscriptions", msg);
                throw new Exception("Could not deserialize response from the http client to a PaginatedSubscriptions");
            }

            logger.LogInformation("Retrieved bulk subscriptions: {@PaginatedSubscriptions}", result);
            return result;
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "An exception occurred trying to bulk retrieve {@BulkId}", bulkId);
            throw;
        }
    }

    public async Task<PaginatedSubscriptions> RetrieveBulkChargesAsync(Guid bulkId, int skip, int take, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogTrace("Retrieving bulk {BulkId}, {Skipping} and {Taking}", bulkId, skip, take);
            var response = await RetrieveBulkChargesAsync(bulkId, skip, take, null, null, cancellationToken);
            var msg = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogTrace("Content is: {@MessageContent}", msg);
            response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<PaginatedSubscriptions>(msg);
            if (result is null)
            {
                logger.LogError("Could not deserialize {@Response} to PaginatedSubscriptions", msg);
                throw new Exception("Could not deserialize response from the http client to a PaginatedSubscriptions");
            }

            logger.LogInformation("Retrieved bulk subscriptions: {@PaginatedSubscriptions}", result);
            return result;
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "An exception occurred trying to bulk retrieve {@BulkId}", bulkId);
            throw;
        }
    }

    public async Task<PaginatedSubscriptions> RetrieveBulkChargesAsync(Guid bulkId, int pageSize, ushort pageNumber, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogTrace("Retrieving bulk {BulkId}, {PageSize} and {PageNumber}", bulkId, pageSize, pageNumber);
            var response = await RetrieveBulkChargesAsync(bulkId, null, null, pageSize, pageNumber, cancellationToken);
            var msg = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogTrace("Content is: {@MessageContent}", msg);
            response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<PaginatedSubscriptions>(msg);
            if (result is null)
            {
                logger.LogError("Could not deserialize {@Response} to PaginatedSubscriptions", msg);
                throw new Exception("Could not deserialize response from the http client to a PaginatedSubscriptions");
            }

            logger.LogInformation("Retrieved bulk subscriptions: {@PaginatedSubscriptions}", result);
            return result;
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "An exception occurred trying to bulk retrieve {@BulkId}", bulkId);
            throw;
        }
    }

    private async Task<HttpResponseMessage> RetrieveBulkChargesAsync(Guid bulkId, int? skip, int? take, int? pageSize, ushort? pageNumber, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(mode);
        AddHeaders(ref client);
        var sb = new StringBuilder();
        sb
            .Append(NetsEndpoints.Relative.Subscription)
            .Append("/charges/")
            .Append(bulkId);
        bool isFirst = true;
        if (skip.HasValue)
        {
            if (isFirst)
            {
                sb.Append('?');
                isFirst = false;
            }

            sb.Append("skip=")
            .Append(skip.Value);
        }
        if (take.HasValue)
        {
            if (isFirst)
            {
                sb.Append('?');
                isFirst = false;
            }
            else
            {
                sb.Append('&');
            }

            sb.Append("take=")
            .Append(take.Value);
        }
        if (pageSize.HasValue)
        {
            if (isFirst)
            {
                sb.Append('?');
                isFirst = false;
            }
            else
            {
                sb.Append('&');
            }

            sb.Append("pageSize=")
            .Append(pageSize.Value);
        }
        if (pageNumber.HasValue)
        {
            if (isFirst)
            {
                sb.Append('?');
            }
            else
            {
                sb.Append('&');
            }

            sb.Append("pageNumber=")
            .Append(pageNumber.Value);
        }

        var path = sb.ToString();
        var response = await client.GetAsync(path, cancellationToken);
        return response;
    }

    public Task<BulkId> VerifyBulkSubscriptionsAsync(BulkSubscriptionVerification verifications, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private void AddHeaders(ref HttpClient client)
    {
        client.DefaultRequestHeaders.Add(HeaderNames.Authorization, apiKey);
    }
}