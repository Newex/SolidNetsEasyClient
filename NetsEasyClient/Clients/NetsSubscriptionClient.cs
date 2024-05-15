using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Logging.PaymentClientLogging;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;
using SolidNetsEasyClient.Models.DTOs.Requests.Webhooks;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments.Subscriptions;
using SolidNetsEasyClient.SerializationContexts;
using SolidNetsEasyClient.Validators;

namespace SolidNetsEasyClient.Clients;

public sealed partial class NetsPaymentClient : IBulkSubscriptionClient
{
    /// <inheritdoc />
    public async ValueTask<SubscriptionDetails?> RetrieveSubscription(Guid subscriptionId, CancellationToken cancellationToken = default)
    {
        if (subscriptionId == Guid.Empty)
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();
        var url = NetsEndpoints.Relative.Subscription + "/" + subscriptionId.ToString("N");
        var response = await client.GetAsync(url, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize(body, SubscriptionSerializationContext.Default.SubscriptionDetails);
            if (result is not null)
            {
                logger.LogInfoRetrieveSubscription(subscriptionId, result);
                return result;
            }

            logger.LogUnexpectedResponse(body);
            return null;
        }

        logger.LogErrorRetrieveSubscription(subscriptionId);
        return null;
    }

    /// <inheritdoc />
    public async ValueTask<SubscriptionDetails?> RetrieveSubscriptionByExternalReference(string externalReference, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(externalReference))
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();
        var url = NetsEndpoints.Relative.Subscription + "?externalReference=" + externalReference;
        var response = await client.GetAsync(url, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize(body, SubscriptionSerializationContext.Default.SubscriptionDetails);
            if (result is not null)
            {
                logger.LogInfoRetrieveSubscription(externalReference, result);
                return result;
            }

            logger.LogUnexpectedResponse(body);
            return null;
        }

        return null;
    }

    /// <inheritdoc />
    public async ValueTask<BulkSubscriptionResult?> BulkChargeSubscriptions(string externalBulkChargeId, IList<SubscriptionCharge> subscriptions, Notification notification, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(externalBulkChargeId))
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();
        var url = NetsEndpoints.Relative.Subscription + "/charges";
        var requestBody = new BulkCharge
        {
            ExternalBulkChargeId = externalBulkChargeId,
            Subscriptions = subscriptions,
            Notifications = notification
        };
        var response = await client.PostAsJsonAsync(url, requestBody, SubscriptionSerializationContext.Default.BulkCharge, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize(body, SubscriptionSerializationContext.Default.BulkSubscriptionResult);
            if (result is not null)
            {
                logger.LogInfoBulkCharge(requestBody, result);
                return result;
            }

            logger.LogUnexpectedResponse(body);
            return null;
        }

        logger.LogErrorBulkCharge(requestBody, await response.Content.ReadAsStringAsync(cancellationToken));
        return null;
    }

    /// <inheritdoc />
    public async ValueTask<PageResult<SubscriptionProcessStatus>?> RetrieveBulkCharges(Guid bulkId,
                                                                                       (int skip, int take)? range = null,
                                                                                       (int pageNumber, int pageSize)? page = null,
                                                                                       CancellationToken cancellationToken = default)
    {
        if (bulkId == Guid.Empty)
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();
        var query = QueryBuilder(range, page);
        var url = NetsEndpoints.Relative.Subscription + "/charges/" + bulkId.ToString("N") + query;
        var response = await client.GetAsync(url, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize(body, SubscriptionSerializationContext.Default.PageResultSubscriptionProcessStatus);
            if (result is not null)
            {
                logger.LogInfoRetrieveBulkCharge(bulkId, result);
                return result;
            }

            logger.LogUnexpectedResponse(body);
            return null;
        }

        logger.LogErrorRetrieveBulkCharge(bulkId, await response.Content.ReadAsStringAsync(cancellationToken));
        return null;
    }

    /// <inheritdoc />
    public async ValueTask<BulkSubscriptionResult?> VerifySubscriptions(string externalBulkVerificationId, IList<SubscriptionCharge> subscriptions, CancellationToken cancellationToken = default)
    {
        var valid = true;
        foreach (var s in subscriptions)
        {
            valid = valid && SubscriptionValidator.ValidateSubscriptionCharge(s);
            if (!valid)
            {
                logger.LogErrorInvalidSubscription(s);
                break;
            }
        }
        if (!valid)
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();
        var url = NetsEndpoints.Relative.Subscription + "/verifications";
        var requestBody = new BulkSubscriptionVerification()
        {
            ExternalBulkVerificationId = externalBulkVerificationId,
            Subscriptions = subscriptions
        };
        var response = await client.PostAsJsonAsync(url,
                                                    requestBody,
                                                    SubscriptionSerializationContext.Default.BulkSubscriptionVerification,
                                                    cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize(body, SubscriptionSerializationContext.Default.BulkSubscriptionResult);
            if (result is not null)
            {
                logger.LogInfoVerifyBulk(subscriptions, externalBulkVerificationId, result);
                return result;
            }

            logger.LogUnexpectedResponse(body);
            return null;
        }

        logger.LogErrorVerifyBulk(externalBulkVerificationId, subscriptions, await response.Content.ReadAsStringAsync(cancellationToken));
        return null;
    }

    /// <inheritdoc />
    public async ValueTask<PageResult<SubscriptionVerificationStatus>?> RetrieveBulkVerifications(Guid bulkId, (int skip, int take)? range = null, (int pageNumber, int pageSize)? page = null, CancellationToken cancellationToken = default)
    {
        if (bulkId == Guid.Empty)
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();
        var query = QueryBuilder(range, page);
        var url = NetsEndpoints.Relative.Subscription + "/verifications/" + bulkId.ToString("N") + query;
        var response = await client.GetAsync(url, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize(body, SubscriptionSerializationContext.Default.PageResultSubscriptionVerificationStatus);
            if (result is not null)
            {
                logger.LogInfoRetrieveBulkVerification(bulkId, result);
                return result;
            }

            logger.LogUnexpectedResponse(body);
            return null;
        }

        logger.LogErrorRetrieveBulkVerification(bulkId, await response.Content.ReadAsStringAsync(cancellationToken));
        return null;
    }

    private static string QueryBuilder((int skip, int take)? range, (int pageNumber, int pageSize)? page)
    {
        if (!range.HasValue && !page.HasValue)
        {
            return string.Empty;
        }

        var sb = new StringBuilder();
        sb.Append('?');
        if (range.HasValue)
        {
            sb.Append("skip=");
            sb.Append(range.Value.skip);
            sb.Append('&');
            sb.Append("take=");
            sb.Append(range.Value.take);
            sb.Append('&');
        }

        if (page.HasValue)
        {
            sb.Append("pageNumber=");
            sb.Append(page.Value.pageNumber);
            sb.Append('&');
            sb.Append("pageSize=");
            sb.Append(page.Value.pageSize);
        }

        // Remove the trailing '&' if it exists
        if (sb[^1] == '&')
        {
            sb.Remove(sb.Length - 1, 1);
        }

        return sb.ToString();
    }
}
