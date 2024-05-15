using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Logging.PaymentClientLogging;
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
}
