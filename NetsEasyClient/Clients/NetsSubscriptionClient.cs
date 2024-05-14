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
using SolidNetsEasyClient.SerializationContexts;

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
}
