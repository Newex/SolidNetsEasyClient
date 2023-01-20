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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Extensions;
using SolidNetsEasyClient.Logging.UnscheduledSubscriptionClientLogging;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;
using SolidNetsEasyClient.Models.DTOs.Requests.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;
using SolidNetsEasyClient.Models.Options;
using SolidNetsEasyClient.Validators;

namespace SolidNetsEasyClient.Clients;

/// <summary>
/// Unscheduled subscription client, responsible for mediating communication between NETS payment API and this client
/// </summary>
/// <remarks>
/// Unscheduled subscriptions allow you to charge your customers at an unscheduled time interval with a variable amount, for example an automatic top-up agreement for a rail-card when the consumer drops below a certain stored value. When an unscheduled subscription is charged, a new payment object is created to represent the purchase of the unscheduled subscription product. It is possible to verify and charge multiple unscheduled subscriptions in bulk using the Bulk charge unscheduled subscriptions method.
/// </remarks>
public class UnscheduledSubscriptionClient : IUnscheduledSubscriptionClient
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

    /// <inheritdoc />
    public async Task<UnscheduledSubscriptionDetails> RetrieveUnscheduledSubscriptionByExternalReferenceAsync(string externalReference, CancellationToken cancellationToken)
    {
        var isValid = !string.IsNullOrWhiteSpace(externalReference);
        if (!isValid)
        {
            logger.ErrorInvalidExternalReference(externalReference);
            throw new ArgumentException("External cannot be empty or only whitespaces", nameof(externalReference));
        }

        try
        {
            logger.TraceRetrieveByExternal(externalReference);
            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(ref client);

            var path = NetsEndpoints.Relative.UnscheduledSubscriptions + $"?externalReference={HttpUtility.UrlEncode(externalReference)}";
            var response = await client.GetAsync(path, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            logger.TraceResponse(content);
            _ = response.EnsureSuccessStatusCode();
            var result = JsonSerializer.Deserialize<UnscheduledSubscriptionDetails>(content);
            if (result is null)
            {
                logger.ErrorDeserializeRetrieveByExternal(content);
                throw new SerializationException("Could not deserialize response to UnscheduledSubscriptionDetails object");
            }

            logger.InfoRetrievedByExternal(result);
            return result;
        }
        catch (Exception ex)
        {
            logger.ExceptionRetrieveByExternal(externalReference, ex);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<UnscheduledSubscriptionChargeResult> ChargeUnscheduledSubscriptionAsync(Guid unscheduledSubscriptionId, Order order, Notification notifications, CancellationToken cancellationToken)
    {
        var isValid = unscheduledSubscriptionId != Guid.Empty && order.Items.Any() && notifications.WebHooks.Count < 33;
        if (!isValid)
        {
            logger.ErrorInvalidCharge(unscheduledSubscriptionId, order);
            throw new ArgumentException("Unscheduled subscription must contain an ID and an order must have at least 1 item!", nameof(unscheduledSubscriptionId));
        }

        try
        {
            logger.TraceCharge(unscheduledSubscriptionId, order);
            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(ref client);

            var path = NetsEndpoints.Relative.UnscheduledSubscriptions + $"/{unscheduledSubscriptionId}/charges";
            var body = new UnscheduledSubscriptionCharge
            {
                Order = order,
                Notifications = notifications
            };
            var response = await client.PostAsJsonAsync(path, body, cancellationToken: cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.TraceResponse(content);
            _ = response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<UnscheduledSubscriptionChargeResult>(content);
            if (result is null)
            {
                logger.ErrorDeserializeCharge(content);
                throw new SerializationException("Could not deserialize response to UnscheduledSubscriptionResult");
            }

            logger.InfoCharge(body, result);
            return result;
        }
        catch (Exception ex)
        {
            logger.ExceptionCharge(unscheduledSubscriptionId, ex);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<BulkId> BulkChargeUnscheduledSubscriptionsAsync(BulkUnscheduledSubscriptionCharge bulk, CancellationToken cancellationToken)
    {
        var isValid = bulk.UnscheduledSubscriptions.All(SubscriptionValidator.OnlyEitherSubscriptionIdOrExternalRef) && !string.IsNullOrWhiteSpace(bulk.ExternalBulkChargeId) && PaymentValidator.CheckWebHooks(bulk.Notifications);
        if (!isValid)
        {
            logger.ErrorInvalidBulk(bulk);
            throw new ArgumentException("Invalid bulk");
        }

        try
        {
            logger.TraceBulkCharge(bulk);
            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(ref client);

            var path = NetsEndpoints.Relative.UnscheduledSubscriptions + "/charges";
            var response = await client.PostAsJsonAsync(path, bulk, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.TraceResponse(content);
            _ = response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<BulkId>(content);
            if (result is null)
            {
                logger.ErrorDeserializeBulkCharge(content);
                throw new SerializationException("Could not deserialize response to BulkId");
            }

            logger.InfoBulkCharged(bulk, result);
            return result;
        }
        catch (Exception ex)
        {
            logger.ExceptionBulkCharge(bulk, ex);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<BulkId> BulkChargeUnscheduledSubscriptionsAsync(IList<ChargeUnscheduledSubscription> bulk, string externalBulkChargeId, Notification? notifications, CancellationToken cancellationToken)
    {
        var isValid = bulk.All(SubscriptionValidator.OnlyEitherSubscriptionIdOrExternalRef) && !string.IsNullOrWhiteSpace(externalBulkChargeId) && PaymentValidator.CheckWebHooks(notifications);
        if (!isValid)
        {
            logger.ErrorInvalidBulkCharge(bulk, externalBulkChargeId, notifications);
            throw new ArgumentException("Invalid bulk, external bulk charge id or notifications");
        }

        try
        {
            logger.TraceBulkCharge(bulk, externalBulkChargeId);
            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(ref client);

            var path = NetsEndpoints.Relative.UnscheduledSubscriptions + "/charges";
            var body = new BulkUnscheduledSubscriptionCharge
            {
                ExternalBulkChargeId = externalBulkChargeId,
                Notifications = notifications,
                UnscheduledSubscriptions = bulk
            };
            var response = await client.PostAsJsonAsync(path, body, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.TraceResponse(content);
            _ = response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<BulkId>(content);
            if (result is null)
            {
                logger.ErrorDeserializeBulkCharge(content);
                throw new SerializationException("Could not deserialize response to BulkId");
            }

            logger.InfoBulkCharged(body, result);
            return result;
        }
        catch (Exception ex)
        {
            logger.ExceptionBulkCharge(bulk, externalBulkChargeId, ex);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PageResult<UnscheduledSubscriptionProcessStatus>> RetrieveBulkUnscheduledSubscriptionChargesAsync(Guid bulkId, CancellationToken cancellationToken)
    {
        try
        {
            logger.TraceRetrieveBulk(bulkId);
            var response = await RetrieveBulkChargesAsync(bulkId, null, null, null, null, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.TraceResponse(content);
            _ = response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<PageResult<UnscheduledSubscriptionProcessStatus>>(content);
            if (result is null)
            {
                logger.ErrorDeserializeRetrieveBulk(content);
                throw new SerializationException("Could not deserialize response from the http client to a paginated unscheduled subscription");
            }

            logger.InfoRetrieveBulk(result);
            return result;
        }
        catch (Exception ex)
        {
            logger.ExceptionRetrieveBulk(bulkId, ex);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PageResult<UnscheduledSubscriptionProcessStatus>> RetrieveBulkUnscheduledSubscriptionChargesAsync(Guid bulkId, int skip, int take, CancellationToken cancellationToken)
    {
        try
        {
            logger.TraceRetrieveBulkSkipTake(bulkId, skip, take);
            var response = await RetrieveBulkChargesAsync(bulkId, skip, take, null, null, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            logger.TraceResponse(content);
            _ = response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<PageResult<UnscheduledSubscriptionProcessStatus>>(content);
            if (result is null)
            {
                logger.ErrorDeserializeRetrieveBulk(content);
                throw new SerializationException("Could not deserialize response from the http client to a paginated unscheduled subscription");
            }

            logger.InfoRetrieveBulk(result);
            return result;
        }
        catch (Exception ex)
        {
            logger.ExceptionRetrieveBulk(bulkId, ex);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PageResult<UnscheduledSubscriptionProcessStatus>> RetrieveBulkUnscheduledSubscriptionChargesAsync(Guid bulkId, int pageNumber, ushort pageSize, CancellationToken cancellationToken)
    {
        try
        {
            logger.TraceRetrieveBulkSizeNumber(bulkId, pageSize, pageNumber);
            var response = await RetrieveBulkChargesAsync(bulkId, null, null, pageNumber, pageSize, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            logger.TraceResponse(content);
            _ = response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<PageResult<UnscheduledSubscriptionProcessStatus>>(content);
            if (result is null)
            {
                logger.ErrorDeserializeRetrieveBulk(content);
                throw new SerializationException("Could not deserialize response from the http client to a paginated unscheduled subscription");
            }

            logger.InfoRetrieveBulk(result);
            return result;
        }
        catch (Exception ex)
        {
            logger.ExceptionRetrieveBulk(bulkId, ex);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<BulkId> VerifyBulkSubscriptionsAsync(IList<UnscheduledSubscriptionInfo> subscriptions, string externalBulkVerificationId, CancellationToken cancellationToken)
    {
        var isValid = subscriptions.All(SubscriptionValidator.OnlyEitherSubscriptionIdOrExternalRef) && !string.IsNullOrWhiteSpace(externalBulkVerificationId) && externalBulkVerificationId.Length < 65;
        if (!isValid)
        {
            logger.ErrorInvalidVerifyBulk(subscriptions, externalBulkVerificationId);
            throw new ArgumentException("Invalid subscriptions or external verification id");
        }

        try
        {
            logger.TraceVerifyBulk(subscriptions);
            var path = NetsEndpoints.Relative.UnscheduledSubscriptions + "/verifications";
            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(ref client);
            var body = new VerifyBulkUnscheduledSubscriptions
            {
                ExternalBulkVerificationId = externalBulkVerificationId,
                UnscheduledSubscriptions = subscriptions
            };
            var response = await client.PostAsJsonAsync(path, body, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            logger.TraceResponse(content);
            _ = response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<BulkId>(content);
            if (result is null)
            {
                logger.ErrorDeserializeVerifyBulk(content);
                throw new SerializationException("Could not deserialize response from the http client to a bulk id");
            }

            logger.InfoVerifyBulk(result);
            return result;
        }
        catch (Exception ex)
        {
            logger.ExceptionVerifyBulk(subscriptions, ex);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PageResult<UnscheduledSubscriptionProcessStatus>> RetrieveBulkVerificationsAsync(Guid bulkId, CancellationToken cancellationToken, int? skip = null, int? take = null, int? pageNumber = null, int? pageSize = null)
    {
        bool isValid = bulkId != Guid.Empty;
        isValid &= skip is null or >= 0;
        isValid &= take is null or >= 0;
        isValid &= pageNumber is null or >= 0;
        if (!isValid)
        {
            logger.ErrorInvalidRetrieveBulkVerification(bulkId);
            throw new ArgumentException("Invalid bulk id or paging parameters.", nameof(bulkId));
        }

        try
        {
            logger.TraceRetrieveBulkVerification(bulkId);
            var root = NetsEndpoints.Relative.UnscheduledSubscriptions + $"/verifications/{bulkId}";
            var path = UrlQueryHelpers.AddQuery(root,
                (nameof(skip), skip?.ToString(CultureInfo.InvariantCulture)),
                (nameof(take), take?.ToString(CultureInfo.InvariantCulture)),
                (nameof(pageSize), pageSize?.ToString(CultureInfo.InvariantCulture)),
                (nameof(pageNumber), pageNumber?.ToString(CultureInfo.InvariantCulture)));

            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(ref client);

            var response = await client.GetAsync(path, cancellationToken);
            var content = await response.Content.ReadAsStringAsync();

            logger.TraceResponse(content);
            _ = response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<PageResult<UnscheduledSubscriptionProcessStatus>>(content);
            if (result is null)
            {
                logger.ErrorDeserializeRetrieveBulk(content);
                throw new SerializationException("Could not deserialize response to page result");
            }

            logger.InfoRetrieveBulk(result);
            return result;
        }
        catch (Exception ex)
        {
            logger.ExceptionRetrieveBulkVerifications(bulkId, ex);
            throw;
        }
    }

    private async Task<HttpResponseMessage> RetrieveBulkChargesAsync(Guid bulkId, int? skip, int? take, int? pageNumber, ushort? pageSize, CancellationToken cancellationToken)
    {
        bool isValid = bulkId != Guid.Empty;
        isValid &= skip is null or >= 0;
        isValid &= take is null or >= 0;
        isValid &= pageNumber is null or >= 0;

        if (!isValid)
        {
            logger.ErrorInvalidRetrieveBulk(bulkId);
            throw new ArgumentException("Invalid bulk id or paging parameters");
        }

        var client = httpClientFactory.CreateClient(mode);
        AddHeaders(ref client);
        var root = NetsEndpoints.Relative.UnscheduledSubscriptions + $"/charges/{bulkId}";
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
