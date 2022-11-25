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
using SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;
using SolidNetsEasyClient.Models.DTOs.Requests.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;
using SolidNetsEasyClient.Models.Options;
using SolidNetsEasyClient.Validators;

namespace SolidNetsEasyClient.Clients;

/// <inheritdoc />
public class SubscriptionClient : ISubscriptionClient
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ILogger<SubscriptionClient> logger;
    private readonly string mode;
    private readonly string apiKey;

    /// <summary>
    /// Instantiate a new object of <see cref="SubscriptionClient"/>
    /// </summary>
    /// <param name="options">The options</param>
    /// <param name="httpClientFactory">The http client factory</param>
    /// <param name="logger">The optional logger</param>
    /// <exception cref="NotSupportedException">Thrown if client mode is not supported</exception>
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

    /// <inheritdoc />
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
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred trying to retrieve subscription with {SubscriptionID}", subscriptionId);
            throw;
        }
    }

    /// <inheritdoc />
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
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred trying to retrieve subscription with {ExternalReference}", externalReference);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<BulkId> BulkChargeSubscriptionsAsync(IList<SubscriptionCharge> subscriptions, CancellationToken cancellationToken, string? externalBulkChargeId = null, Notification? notifications = null)
    {
        // Subscription must either have a subscriptionId or an externalReference but not both!
        var isValid = subscriptions.All(SubscriptionValidator.OnlyEitherSubscriptionIdOrExternalRef) && subscriptions.Count > 0 && (externalBulkChargeId is null || (externalBulkChargeId.Length > 1 && externalBulkChargeId.Length < 64));
        if (!isValid)
        {
            logger.LogError("Invalid {@Subscriptions}", subscriptions);
            throw new ArgumentException("Must have at least 1 subscription where each subscription can only have either an id or an external reference not both. Also external bulk charge id must be between 1 - 64 characters");
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
            _ = response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<BulkId>();
            if (result is null)
            {
                logger.LogError("Could not deserialize response from {@HttpClient}", client);
                throw new Exception("Could not deserialize response from the http client to a BulkId");
            }
            logger.LogInformation("Bulk charge id: {BulkChargeId}", result);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred trying to bulk charge {@Subscriptions}", subscriptions);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PaginatedSubscriptions> RetrieveBulkChargesAsync(Guid bulkId, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogTrace("Retrieving bulk {BulkId}", bulkId);
            var response = await RetrieveBulkChargesAsync(bulkId, null, null, null, null, cancellationToken);
            var msg = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogTrace("Content is: {@MessageContent}", msg);
            _ = response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<PaginatedSubscriptions>(msg);
            if (result is null)
            {
                logger.LogError("Could not deserialize {@Response} to PaginatedSubscriptions", msg);
                throw new Exception("Could not deserialize response from the http client to a PaginatedSubscriptions");
            }

            logger.LogInformation("Retrieved bulk subscriptions: {@PaginatedSubscriptions}", result);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred trying to bulk retrieve {BulkId}", bulkId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PaginatedSubscriptions> RetrieveBulkChargesAsync(Guid bulkId, int skip, int take, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogTrace("Retrieving bulk {BulkId}, {Skipping} and {Taking}", bulkId, skip, take);
            var response = await RetrieveBulkChargesAsync(bulkId, skip, take, null, null, cancellationToken);
            var msg = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogTrace("Content is: {@MessageContent}", msg);
            _ = response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<PaginatedSubscriptions>(msg);
            if (result is null)
            {
                logger.LogError("Could not deserialize {Response} to PaginatedSubscriptions", msg);
                throw new Exception("Could not deserialize response from the http client to a PaginatedSubscriptions");
            }

            logger.LogInformation("Retrieved bulk subscriptions: {@PaginatedSubscriptions}", result);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred trying to bulk retrieve {BulkId}", bulkId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PaginatedSubscriptions> RetrieveBulkChargesAsync(Guid bulkId, int pageSize, ushort pageNumber, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogTrace("Retrieving bulk {BulkId}, {PageSize} and {PageNumber}", bulkId, pageSize, pageNumber);
            var response = await RetrieveBulkChargesAsync(bulkId, null, null, pageSize, pageNumber, cancellationToken);
            var msg = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogTrace("Content is: {@MessageContent}", msg);
            _ = response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<PaginatedSubscriptions>(msg);
            if (result is null)
            {
                logger.LogError("Could not deserialize {Response} to PaginatedSubscriptions", msg);
                throw new Exception("Could not deserialize response from the http client to a PaginatedSubscriptions");
            }

            logger.LogInformation("Retrieved bulk subscriptions: {@PaginatedSubscriptions}", result);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred trying to bulk retrieve {BulkId}", bulkId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<BulkId> VerifyBulkSubscriptionsAsync(BulkSubscriptionVerification verifications, CancellationToken cancellationToken)
    {
        var isValid = (verifications.ExternalBulkVerificationId is null || (verifications.ExternalBulkVerificationId?.Length > 1 && verifications.ExternalBulkVerificationId.Length < 64))
            && (verifications.Subscriptions?.All(SubscriptionValidator.OnlyEitherSubscriptionIdOrExternalRef) ?? verifications.Subscriptions?.Any() ?? false);
        if (!isValid)
        {
            logger.LogError("Invalid subscription {@Verifications}", verifications);
            throw new ArgumentException("Invalid bulk verifications. Must have at least 1 verification and/or exernal bulk verification id must be between 1-64 characters long");
        }

        try
        {
            logger.LogTrace("Verifying subscriptions {@Subscriptions}", verifications);
            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(ref client);

            var path = NetsEndpoints.Relative.Subscription + "/verifications";
            var response = await client.PostAsJsonAsync(path, verifications, cancellationToken);
            var msg = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogTrace("Content is: {MessageContent}", msg);
            _ = response.EnsureSuccessStatusCode();
            var result = JsonSerializer.Deserialize<BulkId>(msg);
            if (result is null)
            {
                logger.LogError("Could not deserialize {Response} to BulkId", msg);
                throw new Exception("Could not deserialize response from the http client to a BulkId");
            }

            logger.LogInformation("Verified {@Subscriptions}, with {BulkId}", verifications, result);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred trying to verify bulk subscriptions of {@Bulk}", verifications);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PaginatedSubscriptions> RetrieveBulkVerificationsAsync(Guid bulkId, int? skip, int? take, int? pageNumber, int? pageSize, CancellationToken cancellationToken)
    {
        bool isValid = bulkId != Guid.Empty;
        isValid &= skip is null or > 0;
        isValid &= take is null or > 0;
        isValid &= pageNumber is null or > 0;
        isValid &= pageSize is null or > 0;

        if (!isValid)
        {
            logger.LogError("Invalid {BulkId} or paging parameters must have values greater than or equal to zero", bulkId);
            throw new ArgumentException("Invalid bulk id or paging parameters; must be greater than or equal to zero (or null)");
        }

        try
        {
            logger.LogTrace("Retrieving verifications of {@BulkId}", bulkId);
            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(ref client);

            var root = NetsEndpoints.Relative.Subscription + "/verifications/" + bulkId;
            var path = AddQuery(root,
                                (nameof(skip), skip?.ToString()),
                                (nameof(take), take?.ToString()),
                                (nameof(pageSize), pageSize?.ToString()),
                                (nameof(pageNumber), pageNumber?.ToString()));
            var response = await client.GetAsync(path, cancellationToken);
            var msg = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogTrace("Content is: {@MessageContent}", msg);
            _ = response.EnsureSuccessStatusCode();
            var result = JsonSerializer.Deserialize<PaginatedSubscriptions>(msg);
            if (result is null)
            {
                logger.LogError("Could not deserialize {Response} to bulkId", msg);
                throw new Exception("Could not deserialize response from the http client to a PaginatedSubscription");
            }

            logger.LogInformation("Retrieved verified {@PaginatedSubscriptions}", result);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred trying to retrieve verifications of {BulkId}", bulkId);
            throw;
        }
    }

    private async Task<HttpResponseMessage> RetrieveBulkChargesAsync(Guid bulkId, int? skip, int? take, int? pageSize, ushort? pageNumber, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(mode);
        AddHeaders(ref client);
        var root = NetsEndpoints.Relative.Subscription + "/charges/" + bulkId.ToString();
        var path = AddQuery(root,
                            (nameof(skip), skip?.ToString()),
                            (nameof(take), take?.ToString()),
                            (nameof(pageSize), pageSize?.ToString()),
                            (nameof(pageNumber), pageNumber?.ToString()));
        var response = await client.GetAsync(path, cancellationToken);
        return response;
    }

    private static string AddQuery(string root, params (string? Key, string? Value)[] query)
    {
        var sb = new StringBuilder(root);
        bool isFirst = true;

        foreach (var (Key, Value) in query)
        {
            if (Key is null || Value is null)
            {
                continue;
            }

            if (isFirst)
            {
                _ = sb.Append('?');
                isFirst = false;
            }
            else
            {
                _ = sb.Append('&');
            }

            _ = sb.Append(Key)
                .Append('=')
                .Append(Value);
        }

        var path = sb.ToString();
        return path;
    }

    private void AddHeaders(ref HttpClient client)
    {
        client.DefaultRequestHeaders.Add(HeaderNames.Authorization, apiKey);
    }
}