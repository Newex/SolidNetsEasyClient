using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Extensions;
using SolidNetsEasyClient.Logging.PaymentClientLogging;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;
using SolidNetsEasyClient.SerializationContexts;

namespace SolidNetsEasyClient.Clients;

public sealed partial class NetsPaymentClient : IChargeClient
{
    /// <inheritdoc />
    public async ValueTask<ChargeResult?> ChargePayment(Guid paymentId, Charge charge, string? idempotencyKey = null, CancellationToken cancellationToken = default)
    {
        var isValid = paymentId != Guid.Empty
                      && charge.Amount > 0;
        if (!isValid)
        {
            return null;
        }

        var url = NetsEndpoints.Relative.Payment + "/" + paymentId.ToString("N") + "/charges";
        var response = await client.PostAsJsonWithHeadersAsync(url, charge, ChargeSerializationContext.Default.Charge, ("Idempotency-Key", idempotencyKey), cancellationToken);
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
            return null;
        }

        logger.LogErrorCharge(paymentId, charge, body);
        return null;
    }

    /// <inheritdoc />
    public async ValueTask<ChargeDetailsInfo?> RetrieveCharge(Guid chargeId, CancellationToken cancellationToken = default)
    {
        if (chargeId == Guid.Empty)
        {
            return null;
        }

        var url = NetsEndpoints.Relative.Charge + "/" + chargeId.ToString("N");
        var response = await client.GetAsync(url, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize(body, ChargeSerializationContext.Default.ChargeDetailsInfo);
            if (result is not null)
            {
                logger.LogInfoChargeRetrieved(result);
                return result;
            }

            logger.LogUnexpectedResponse(body);
            return null;
        }

        logger.LogErrorChargeRetrieval(chargeId, body);
        return null;
    }

    /// <inheritdoc />
    public async ValueTask<RefundResult?> RefundCharge(Guid chargeId, CancelOrder charge, string? idempotencyKey = null, CancellationToken cancellationToken = default)
    {
        if (chargeId == Guid.Empty && charge.Amount == 0)
        {
            return null;
        }

        var url = NetsEndpoints.Relative.Charge + "/" + chargeId.ToString("N") + "/refunds";
        var response = await client.PostAsJsonWithHeadersAsync(url, charge, CancelOrderSerializationContext.Default.CancelOrder, ("Idempotency-Key", idempotencyKey), cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize(body, RefundSerializationContext.Default.RefundResult);
            if (result is not null)
            {
                logger.LogInfoRefundResult(chargeId, charge, result);
                return result;
            }

            logger.LogUnexpectedResponse(body);
            return null;
        }

        logger.LogErrorRefundCharge(chargeId, charge, body);
        return null;
    }
}
