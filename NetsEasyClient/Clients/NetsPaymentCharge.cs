using System;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Logging.PaymentClientLogging;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;
using SolidNetsEasyClient.SerializationContexts;

namespace SolidNetsEasyClient.Clients;

public sealed partial class NetsPaymentClient : IChargeClient
{
    /// <inheritdoc />
    public async ValueTask<ChargeResult?> ChargePayment(Guid paymentId, Charge charge, string? idempotencyKey = null, CancellationToken cancellationToken = default)
    {
        var isValid = paymentId != Guid.Empty && charge.Amount > 0;
        if (!isValid)
        {
            return null;
        }

        var url = NetsEndpoints.Relative.Payment + "/" + paymentId.ToString("N") + "/charges";
        var response = await client.PostAsJsonAsync(url, charge, ChargeSerializationContext.Default.Charge, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize(body, ChargeSerializationContext.Default.ChargeResult);
            if (result is not null)
            {
                logger.LogInfoCharge(paymentId, charge, result);
                return result;
            }

            logger.LogUnexpectedResponse(body);
        }

        logger.LogErrorCharge(paymentId, charge, body);
        return null;
    }
}
