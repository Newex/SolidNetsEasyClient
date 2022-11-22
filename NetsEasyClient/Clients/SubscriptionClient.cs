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

/// <summary>
/// Subscription client, responsible for mediating communication between NETS payment API and this client
/// </summary>
/// <remarks>
/// Subscriptions allow you to charge your customers on a regular basis, for example a monthly subscription for a product the customer must pay for every month. When a subscription is charged, a new payment object is created to represent the purchase of the subscription product. It is possible to verify and charge multiple subscriptions in bulk using the Bulk charge subscriptions method.
/// </remarks>
public class SubscriptionClient
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

    /// <summary>
    /// Retrieves an existing subscription by a subscriptionId. The subscriptionId can be obtained from the Retrieve payment method.
    /// </summary>
    /// <param name="subscriptionId">The subscription identifier (a UUID).</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Details of a subscription</returns>
    /// <exception cref="ArgumentException">Thrown if subscription id is empty</exception>
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

    /// <summary>
    /// Retrieves a subscription matching the specified externalReference. This method can only be used for retrieving subscriptions that have been imported from a payment platform other than Nets Easy. Subscriptions created within Nets Easy do not have an externalReference value set.
    /// </summary>
    /// <param name="externalReference">The external reference to search for.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Details of a subscription</returns>
    /// <exception cref="ArgumentException">Thrown if the external reference is null or only contains whitespaces</exception>
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
    }

    /// <summary>
    /// Charges multiple subscriptions at once. The request body must contain: (*) A unique string that identifies this bulk charge operation. (*) A set of subscription identifiers that should be charged. To get status updates about the bulk charge you can subscribe to the webhooks for charges and refunds (payment.charges.* and payments.refunds.*). See also the webhooks documentation.
    /// </summary>
    /// <param name="subscriptions">The list of subscriptions to charge in bulk</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <param name="externalBulkChargeId">A string that uniquely identifies the bulk charge operation. Use this property for enabling safe retries. Must be between 1 and 64 characters.</param>
    /// <param name="notifications">Subscribe to notifications</param>
    /// <returns>A bulk id</returns>
    /// <exception cref="ArgumentException">Thrown if subscriptions or external bulk charge id is invalid</exception>
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

    /// <summary>
    /// Retrieves charges associated with the specified bulk charge operation. The bulkId is returned from Nets in the response of the Bulk charge subscriptions method.
    /// </summary>
    /// <param name="bulkId">The identifier of the bulk charge operation that was returned from the Bulk charge subscriptions method.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A paginated set of subscriptions</returns>
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

    /// <summary>
    /// Retrieves charges associated with the specified bulk charge operation. The bulkId is returned from Nets in the response of the Bulk charge subscriptions method. This method supports pagination. Specify the range of subscriptions to retrieve by using either skip and take. The boolean property named more in the response body, indicates whether there are more subscriptions beyond the requested range.
    /// </summary>
    /// <param name="bulkId">The bulk id</param>
    /// <param name="skip">The number of subscription entries to skip from the start. Use this property in combination with the take property.</param>
    /// <param name="take">The maximum number of subscriptions to be retrieved. Use this property in combination with the skip property.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A paginated set of subscriptions</returns>
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

    /// <summary>
    /// Retrieves charges associated with the specified bulk charge operation. The bulkId is returned from Nets in the response of the Bulk charge subscriptions method. This method supports pagination. Specify the range of subscriptions to retrieve by using pageNumber together with pageSize. The boolean property named more in the response body, indicates whether there are more subscriptions beyond the requested range.
    /// </summary>
    /// <param name="bulkId">The identifier of the bulk charge operation that was returned from the Bulk charge subscriptions method.</param>
    /// <param name="pageSize">The size of each page when specify the range of subscriptions using the pageNumber property.</param>
    /// <param name="pageNumber">The page number to be retrieved. Use this property in combination with the pageSize property.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A paginated set of subscriptions</returns>
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

    /// <summary>
    /// Verifies the specified set of subscriptions in bulk. The bulkId returned from a successful request can be used for querying the status of the subscriptions.
    /// </summary>
    /// <param name="verifications">The set of subscriptions that should be verified. Each item in the array should define either a subscriptioId or an externalReference, but not both.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A bulk id</returns>
    /// <exception cref="ArgumentException">Thrown if invalid subscriptions or external bulk verification id</exception>
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
            logger.LogTrace("Content is: {@MessageContent}", msg);
            response.EnsureSuccessStatusCode();
            var result = JsonSerializer.Deserialize<BulkId>(msg);
            if (result is null)
            {
                logger.LogError("Could not deserialize {@Response} to BulkId", msg);
                throw new Exception("Could not deserialize response from the http client to a BulkId");
            }

            logger.LogInformation("Verified {@Subscriptions}, with {@BulkId}", verifications, result);
            return result;
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "An exception occurred trying to verify bulk subscriptions of {@Bulk}", verifications);
            throw;
        }
    }

    /// <summary>
    /// Retrieves verifications associated with the specified bulk verification operation. The bulkId is returned from Nets in the response of the Verify subscriptions method. This method supports pagination. Specify the range of subscriptions to retrieve by using either skip and take or pageNumber together with pageSize. The boolean property named more in the response body, indicates whether there are more subscriptions beyond the requested range.
    /// </summary>
    /// <param name="bulkId">The identifier of the bulk verification operation that was returned from the erfiy subscriptions method.</param>
    /// <param name="skip">The number of subscription entries to skip from the start. Use this property in combination with the take property.</param>
    /// <param name="take">The maximum number of subscriptions to be retrieved. Use this property in combination with the skip property.</param>
    /// <param name="pageNumber">The page number to be retrieved. Use this property in combination with the pageSize property.</param>
    /// <param name="pageSize">The size of each page when specify the range of subscriptions using the pageNumber property.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A paginated result set of subscriptions</returns>
    /// <exception cref="ArgumentException">Thrown if bulk id is empty or if skip, take, pageNumber or pageSize is negative</exception>
    public async Task<PaginatedSubscriptions> RetrieveBulkVerificationsAsync(Guid bulkId, int? skip, int? take, int? pageNumber, int? pageSize, CancellationToken cancellationToken)
    {
        bool isValid = bulkId != Guid.Empty;
        isValid &= skip is null || skip > 0;
        isValid &= take is null || take > 0;
        isValid &= pageNumber is null || pageNumber > 0;
        isValid &= pageSize is null || pageSize > 0;

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
            response.EnsureSuccessStatusCode();
            var result = JsonSerializer.Deserialize<PaginatedSubscriptions>(msg);
            if (result is null)
            {
                logger.LogError("Could not deserialize {@Response} to bulkId", msg);
                throw new Exception("Could not deserialize response from the http client to a PaginatedSubscription");
            }

            logger.LogInformation("Retrieved verified {@PaginatedSubscriptions}", result);
            return result;
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "An exception occurred trying to retrieve verifications of {@BulkId}", bulkId);
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
                sb.Append('?');
                isFirst = false;
            }
            else
            {
                sb.Append('&');
            }

            sb.Append(Key)
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