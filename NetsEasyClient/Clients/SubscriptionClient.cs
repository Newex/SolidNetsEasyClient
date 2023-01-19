using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Extensions;
using SolidNetsEasyClient.Logging.SubscriptionClientLogging;
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
            logger.ErrorInvalidSubscriptionID(subscriptionId);
            throw new ArgumentException("Guid cannot be empty", nameof(subscriptionId));
        }

        try
        {
            logger.TraceSubscriptionRetrieval(subscriptionId);
            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(ref client);

            var path = NetsEndpoints.Relative.Subscription + $"/{subscriptionId}";
            var result = await client.GetFromJsonAsync<SubscriptionDetails>(path, cancellationToken);

            if (result is null)
            {
                logger.ErrorSubscriptionDetailSerialization(subscriptionId);
                throw new SerializationException("Could not deserialize response from the http client to a SubscriptionDetails");
            }

            logger.InfoRetrievedSubscription(result);
            return result;
        }
        catch (Exception ex)
        {
            logger.ExceptionRetrieveSubscription(subscriptionId, ex);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<SubscriptionDetails> RetrieveSubscriptionByExternalReferenceAsync(string externalReference, CancellationToken cancellationToken)
    {
        var isValid = !string.IsNullOrWhiteSpace(externalReference);
        if (!isValid)
        {
            logger.ErrorInvalidExternalReference(externalReference);
            throw new ArgumentException("External reference cannot be empty", nameof(externalReference));
        }

        try
        {
            logger.TraceRetrieveByExternal(externalReference);
            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(ref client);

            var path = NetsEndpoints.Relative.Subscription + $"?externalReference={HttpUtility.UrlEncode(externalReference)}";
            var result = await client.GetFromJsonAsync<SubscriptionDetails>(path, cancellationToken);

            if (result is null)
            {
                logger.ErrorRetrieveByExternal(externalReference);
                throw new SerializationException("Could not deserialize response from the http client to a SubscriptionDetails");
            }

            logger.InfoRetrieveByExternal(result);
            return result;
        }
        catch (Exception ex)
        {
            logger.ExceptionRetrieveByExternal(externalReference, ex);
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
            logger.ErrorInvalidBulkCharge(subscriptions);
            throw new ArgumentException("Must have at least 1 subscription where each subscription can only have either an id or an external reference not both. Also external bulk charge id must be between 1 - 64 characters");
        }

        try
        {
            logger.TraceBulkCharge(subscriptions);
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
                logger.ErrorBulkCharge(subscriptions);
                throw new SerializationException("Could not deserialize response from the http client to a BulkId");
            }

            logger.InfoBulkCharge(result);
            return result;
        }
        catch (Exception ex)
        {
            logger.ExceptionBulkCharge(subscriptions, ex);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PageResult<SubscriptionProcessStatus>> RetrieveBulkChargesAsync(Guid bulkId, CancellationToken cancellationToken)
    {
        try
        {
            logger.TraceBulkId(bulkId);
            var response = await RetrieveBulkChargesAsync(bulkId, null, null, null, null, cancellationToken);
            var msg = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.TraceMessageContent(msg);
            _ = response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<PageResult<SubscriptionProcessStatus>>(msg);
            if (result is null)
            {
                logger.ErrorPaginatedSubscription(msg);
                throw new SerializationException("Could not deserialize response from the http client to a PaginatedSubscriptions");
            }

            logger.InfoPaginatedSubscriptions(result);
            return result;
        }
        catch (Exception ex)
        {
            logger.ExceptionPaginatedSubscriptions(bulkId, ex);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PageResult<SubscriptionProcessStatus>> RetrieveBulkChargesAsync(Guid bulkId, int skip, int take, CancellationToken cancellationToken)
    {
        try
        {
            logger.TracePageSkipTake(bulkId, skip, take);
            var response = await RetrieveBulkChargesAsync(bulkId, skip, take, null, null, cancellationToken);
            var msg = await response.Content.ReadAsStringAsync(cancellationToken);

            logger.TraceMessageContent(msg);
            _ = response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<PageResult<SubscriptionProcessStatus>>(msg);
            if (result is null)
            {
                logger.ErrorPaginatedSubscription(msg);
                throw new SerializationException("Could not deserialize response from the http client to a PaginatedSubscriptions");
            }

            logger.InfoPaginatedSubscriptions(result);
            return result;
        }
        catch (Exception ex)
        {
            logger.ExceptionPaginatedSubscriptions(bulkId, ex);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PageResult<SubscriptionProcessStatus>> RetrieveBulkChargesAsync(Guid bulkId, int pageSize, ushort pageNumber, CancellationToken cancellationToken)
    {
        try
        {
            logger.TracePageSizeNumber(bulkId, pageSize, pageNumber);
            var response = await RetrieveBulkChargesAsync(bulkId, null, null, pageSize, pageNumber, cancellationToken);
            var msg = await response.Content.ReadAsStringAsync(cancellationToken);

            logger.TraceMessageContent(msg);
            _ = response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<PageResult<SubscriptionProcessStatus>>(msg);
            if (result is null)
            {
                logger.ErrorPaginatedSubscription(msg);
                throw new SerializationException("Could not deserialize response from the http client to a PaginatedSubscriptions");
            }

            logger.InfoPaginatedSubscriptions(result);
            return result;
        }
        catch (Exception ex)
        {
            logger.ExceptionPaginatedSubscriptions(bulkId, ex);
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
            logger.ErrorInvalidSubscriptionVerifications(verifications);
            throw new ArgumentException("Invalid bulk verifications. Must have at least 1 verification and/or exernal bulk verification id must be between 1-64 characters long");
        }

        try
        {
            logger.TraceVerification(verifications);
            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(ref client);

            var path = NetsEndpoints.Relative.Subscription + "/verifications";
            var response = await client.PostAsJsonAsync(path, verifications, cancellationToken);
            var msg = await response.Content.ReadAsStringAsync(cancellationToken);

            logger.TraceMessageContent(msg);
            _ = response.EnsureSuccessStatusCode();
            var result = JsonSerializer.Deserialize<BulkId>(msg);
            if (result is null)
            {
                logger.ErrorResponseToBulkID(msg);
                throw new SerializationException("Could not deserialize response from the http client to a BulkId");
            }

            logger.InfoVerifications(verifications, result);
            return result;
        }
        catch (Exception ex)
        {
            logger.ExceptionVerifications(verifications, ex);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PageResult<SubscriptionProcessStatus>> RetrieveBulkVerificationsAsync(Guid bulkId, int? skip, int? take, int? pageNumber, int? pageSize, CancellationToken cancellationToken)
    {
        bool isValid = bulkId != Guid.Empty;
        isValid &= skip is null or >= 0;
        isValid &= take is null or >= 0;
        isValid &= pageNumber is null or >= 0;
        isValid &= pageSize is null or >= 0;

        if (!isValid)
        {
            logger.ErrorInvalidBulkIdOrPage(bulkId);
            throw new ArgumentException("Invalid bulk id or paging parameters; must be greater than or equal to zero (or null)");
        }

        try
        {
            logger.TracePageVerifications(bulkId);
            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(ref client);

            var root = NetsEndpoints.Relative.Subscription + "/verifications/" + bulkId;
            var path = UrlQueryHelpers.AddQuery(root,
                                (nameof(skip), skip?.ToString(CultureInfo.InvariantCulture)),
                                (nameof(take), take?.ToString(CultureInfo.InvariantCulture)),
                                (nameof(pageSize), pageSize?.ToString(CultureInfo.InvariantCulture)),
                                (nameof(pageNumber), pageNumber?.ToString(CultureInfo.InvariantCulture)));
            var response = await client.GetAsync(path, cancellationToken);
            var msg = await response.Content.ReadAsStringAsync(cancellationToken);

            logger.TraceMessageContent(msg);
            _ = response.EnsureSuccessStatusCode();
            var result = JsonSerializer.Deserialize<PageResult<SubscriptionProcessStatus>>(msg);
            if (result is null)
            {
                logger.ErrorResponseToBulkID(msg);
                throw new SerializationException("Could not deserialize response from the http client to a PaginatedSubscription");
            }

            logger.InfoPaginatedSubscriptions(result);
            return result;
        }
        catch (Exception ex)
        {
            logger.ExceptionPageVerifications(bulkId, ex);
            throw;
        }
    }

    private async Task<HttpResponseMessage> RetrieveBulkChargesAsync(Guid bulkId, int? skip, int? take, int? pageSize, ushort? pageNumber, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(mode);
        AddHeaders(ref client);
        var root = NetsEndpoints.Relative.Subscription + "/charges/" + bulkId.ToString();
        var path = UrlQueryHelpers.AddQuery(root,
                            (nameof(skip), skip?.ToString(CultureInfo.InvariantCulture)),
                            (nameof(take), take?.ToString(CultureInfo.InvariantCulture)),
                            (nameof(pageSize), pageSize?.ToString(CultureInfo.InvariantCulture)),
                            (nameof(pageNumber), pageNumber?.ToString(CultureInfo.InvariantCulture)));
        var response = await client.GetAsync(path, cancellationToken);
        return response;
    }

    private void AddHeaders(ref HttpClient client)
    {
        client.DefaultRequestHeaders.Add(HeaderNames.Authorization, apiKey);
    }
}
