using System;
using System.Collections.Generic;
using System.Net.Http.Json;
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

namespace SolidNetsEasyClient.Clients;

public sealed partial class NexiClient : IUnscheduledSubscriptionClient
{
    /// <inheritdoc />
    public async ValueTask<UnscheduledSubscriptionDetails?> RetrieveUnscheduledSubscription(Guid unscheduledSubscriptionId, CancellationToken cancellationToken = default)
    {
        if (unscheduledSubscriptionId == Guid.Empty)
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();
        var url = NetsEndpoints.Relative.UnscheduledSubscriptions + "/" + unscheduledSubscriptionId.ToString("N");
        var response = await client.GetAsync(url, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize(body, UnscheduledSubscriptionSerializationContext.Default.UnscheduledSubscriptionDetails);
            if (result is not null)
            {
                logger.LogInfoRetrieveUnscheduledSubscription(unscheduledSubscriptionId, result);
                return result;
            }

            logger.LogUnexpectedResponse(body);
            return null;
        }

        logger.LogErrorRetrieveUnscheduledSubscription(unscheduledSubscriptionId, await response.Content.ReadAsStringAsync(cancellationToken));
        return null;
    }

    /// <inheritdoc />
    public async ValueTask<UnscheduledSubscriptionDetails?> RetrieveUnscheduledSubscriptionByExternalReference(string externalReference, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(externalReference))
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();
        var url = NetsEndpoints.Relative.UnscheduledSubscriptions + "?externalReference=" + externalReference;
        var response = await client.GetAsync(url, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize(body, UnscheduledSubscriptionSerializationContext.Default.UnscheduledSubscriptionDetails);
            if (result is not null)
            {
                logger.LogInfoRetrieveUnscheduledSubscription(externalReference, result);
                return result;
            }

            logger.LogUnexpectedResponse(body);
            return null;
        }

        logger.LogErrorRetrieveUnscheduledSubscription(externalReference, body);
        return null;
    }

    /// <inheritdoc />
    public async ValueTask<UnscheduledSubscriptionChargeResult?> ChargeUnscheduledSubscription(Guid unscheduledSubscriptionId, UnscheduledSubscriptionCharge charge, CancellationToken cancellationToken = default)
    {
        if (unscheduledSubscriptionId == Guid.Empty)
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();
        var url = NetsEndpoints.Relative.UnscheduledSubscriptions + "/" + unscheduledSubscriptionId.ToString("N") + "/charges";
        var response = await client.PostAsJsonAsync(url, charge, UnscheduledSubscriptionSerializationContext.Default.UnscheduledSubscriptionCharge, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize(body, UnscheduledSubscriptionSerializationContext.Default.UnscheduledSubscriptionChargeResult);
            if (result is not null)
            {
                logger.LogInfoChargeUnscheduledSubscription(unscheduledSubscriptionId, charge, result);
                return result;
            }

            logger.LogUnexpectedResponse(body);
            return null;
        }

        logger.LogErrorChargeUnscheduledSubscription(unscheduledSubscriptionId, charge, body);
        return null;
    }

    /// <inheritdoc />
    public async ValueTask<BaseBulkResult?> BulkChargeUnscheduledSubscriptions(string externalBulkChargeId,
                                                                               IList<ChargeUnscheduledSubscription> charges,
                                                                               Notification? notifications = null,
                                                                               CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(externalBulkChargeId))
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();
        var url = NetsEndpoints.Relative.UnscheduledSubscriptions + "/charges";
        var bulk = new BulkUnscheduledSubscriptionCharge()
        {
            ExternalBulkChargeId = externalBulkChargeId,
            Notifications = notifications,
            UnscheduledSubscriptions = charges
        };
        var response = await client.PostAsJsonAsync(url, bulk, UnscheduledSubscriptionSerializationContext.Default.BulkUnscheduledSubscriptionCharge, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize(body, UnscheduledSubscriptionSerializationContext.Default.BaseBulkResult);
            if (result is not null)
            {
                logger.LogInfoBulkChargeUnscheduledSubscriptions(charges, externalBulkChargeId, result.BulkId);
                return result;
            }

            logger.LogUnexpectedResponse(body);
            return null;
        }

        logger.LogErrorBulkChargeUnscheduledSubscriptions(externalBulkChargeId, charges, body);
        return null;
    }

    /// <inheritdoc />
    public async ValueTask<PageResult<UnscheduledSubscriptionProcessStatus>?> RetrieveBulkUnscheduledCharges(Guid bulkId,
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
        var url = NetsEndpoints.Relative.UnscheduledSubscriptions + "/charges/" + bulkId.ToString("N") + query;
        var response = await client.GetAsync(url, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize(body, UnscheduledSubscriptionSerializationContext.Default.PageResultUnscheduledSubscriptionProcessStatus);
            if (result is not null)
            {
                logger.LogInfoRetrieveBulkUnscheduledCharges(bulkId, result);
                return result;
            }

            logger.LogUnexpectedResponse(body);
            return null;
        }

        logger.LogErrorRetrieveBulkUnscheduledCharges(bulkId, body);
        return null;
    }

    /// <inheritdoc />
    public async ValueTask<BaseBulkResult?> VerifyCardsForUnscheduledSubscriptions(string externalBulkVerificationId, IList<UnscheduledSubscriptionInfo> subscriptions, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(externalBulkVerificationId))
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();
        var url = NetsEndpoints.Relative.UnscheduledSubscriptions + "/verifications";
        var verify = new VerifyBulkUnscheduledSubscriptions
        {
            ExternalBulkVerificationId = externalBulkVerificationId,
            UnscheduledSubscriptions = subscriptions
        };
        var response = await client.PostAsJsonAsync(url, verify, UnscheduledSubscriptionSerializationContext.Default.VerifyBulkUnscheduledSubscriptions, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize(body, UnscheduledSubscriptionSerializationContext.Default.BaseBulkResult);
            if (result is not null)
            {
                return result;
            }

            return null;
        }

        return null;
    }
}
