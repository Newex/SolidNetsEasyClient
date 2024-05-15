using System;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Logging.PaymentClientLogging;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;
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
}
