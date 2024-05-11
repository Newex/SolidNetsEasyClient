using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Logging.PaymentClientLogging;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;
using SolidNetsEasyClient.Models.Options;
using SolidNetsEasyClient.SerializationContexts;
using SolidNetsEasyClient.Validators;

namespace SolidNetsEasyClient.Clients;

/// <summary>
/// Typed http client for the Nets Easy Payment API.
/// </summary>
/// <remarks>
/// <![CDATA[ NETS Easy Payment API (2024): https://developer.nexigroup.com/nexi-checkout/en-EU/api/payment-v1/ ]]> <br />
/// <![CDATA[ Do not use this in a singleton class. See https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory#avoid-typed-clients-in-singleton-services ]]>
/// </remarks>
public sealed partial class NetsPaymentClient(
    HttpClient client,
    IOptions<NetsEasyOptions> options,
    ILogger<NetsPaymentClient>? logger = null
) : IDisposable, ICheckoutClient
{
    private readonly HttpClient client = client;
    private readonly ILogger<NetsPaymentClient> logger = logger ?? NullLogger<NetsPaymentClient>.Instance;

    /// <inheritdoc />
    public string? CheckoutKey => options.Value.CheckoutKey;

    /// <inheritdoc />
    public async ValueTask<PaymentResult?> StartCheckoutPayment(PaymentRequest paymentRequest, CancellationToken cancellationToken = default)
    {
        // Validate the payment request
        var isValid = PaymentValidator.IsValidPaymentObject(paymentRequest, logger);
        if (!isValid)
        {
            throw new InvalidOperationException("Invalid payment request.");
        }

        cancellationToken.ThrowIfCancellationRequested();
        var result = await client.PostAsJsonAsync(NetsEndpoints.Relative.Payment, paymentRequest, PaymentRequestSerializationContext.Default.PaymentRequest, cancellationToken);

        // Documentation says, it returns 201.
        if (result.IsSuccessStatusCode)
        {
            var response = await result.Content.ReadFromJsonAsync(PaymentResultSerializationContext.Default.PaymentResult, cancellationToken);

            if (response is not null)
            {
                logger.LogPaymentRequestSuccess(paymentRequest, response);
                return response;
            }

            logger.LogUnexpectedResponse(await result.Content.ReadAsStringAsync(cancellationToken));
            return null;
        }

        var error = await result.Content.ReadAsStringAsync(cancellationToken);
        logger.LogPaymentRequestError(paymentRequest, error);
        return null;
    }

    /// <inheritdoc />
    public async ValueTask<PaymentStatus?> RetrievePaymentDetails(Guid paymentId, CancellationToken cancellationToken = default)
    {
        if (paymentId == Guid.Empty)
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();
        var url = NetsEndpoints.Relative.Payment + "/" + paymentId.ToString("N");
        var response = await client.GetAsync(url, cancellationToken);

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize(body, PaymentSerializationContext.Default.PaymentStatus);
            if (result is null)
            {
                logger.LogUnexpectedResponse(await response.Content.ReadAsStringAsync(cancellationToken));
                return null;
            }

            logger.LogInfoPaymentDetails(paymentId, result);
            return result;
        }

        logger.LogUnexpectedResponse(await response.Content.ReadAsStringAsync(cancellationToken));
        return null;
    }

    /// <inheritdoc />
    public async ValueTask<bool> UpdateReferenceInformationBeforeCheckout(Guid paymentId, ReferenceInformation references, CancellationToken cancellationToken = default)
    {
        if (paymentId == Guid.Empty)
        {
            return false;
        }

        cancellationToken.ThrowIfCancellationRequested();
        var url = NetsEndpoints.Relative.Payment + "/" + paymentId.ToString("N") + "/referenceinformation";
        var response = await client.PutAsJsonAsync(url,
                                                   references,
                                                   ReferenceInformationSerializationContext.Default.ReferenceInformation,
                                                   cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            logger.LogInfoReferenceInformation(paymentId, references);
            return true;
        }

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        logger.LogErrorReferenceInformation(paymentId, references, body);
        return false;
    }

    /// <inheritdoc />
    public async ValueTask<bool> UpdateOrderBeforeCheckout(Guid paymentId,
                                                          OrderUpdate update,
                                                          CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var url = NetsEndpoints.Relative.Payment + "/" + paymentId.ToString("N") + "/orderItems";
        var response = await client.PutAsJsonAsync(url, update, OrderUpdateSerializationContext.Default.OrderUpdate, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            logger.LogInfoOrderUpdated(paymentId, update);
            return true;
        }

        logger.LogErrorOrderUpdate(paymentId, update, await response.Content.ReadAsStringAsync(cancellationToken));
        return false;
    }

    /// <inheritdoc />
    public async ValueTask<bool> TerminatePaymentBeforeCheckout(Guid paymentId, CancellationToken cancellationToken = default)
    {
        if (paymentId == Guid.Empty)
        {
            return false;
        }

        cancellationToken.ThrowIfCancellationRequested();
        var url = NetsEndpoints.Relative.Payment + "/" + paymentId.ToString("N") + "/terminate";
        var response = await client.PutAsync(url, null, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            logger.LogInfoTerminatePayment(paymentId);
            return true;
        }

        logger.LogErrorTerminatePayment(paymentId, await response.Content.ReadAsStringAsync(cancellationToken));
        return false;
    }

    /// <inheritdoc />
    public async ValueTask<bool> CancelPaymentBeforeCharge(Guid paymentId,
                                                           CancelOrder order,
                                                           CancellationToken cancellationToken = default)
    {
        if (paymentId == Guid.Empty)
        {
            return false;
        }

        cancellationToken.ThrowIfCancellationRequested();
        var url = NetsEndpoints.Relative.Payment + "/" + paymentId.ToString("N") + "/cancels";
        var response = await client.PostAsJsonAsync(url, order, CancelOrderSerializationContext.Default.CancelOrder, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            logger.LogInfoOrderCanceled(paymentId, order);
            return true;
        }

        logger.LogErrorOrderCanceled(paymentId, order, await response.Content.ReadAsStringAsync(cancellationToken));
        return false;
    }

    /// <inheritdoc />
    public async ValueTask<bool> UpdateMyReference(Guid paymentId, PaymentReference myReference, CancellationToken cancellationToken = default)
    {
        if (paymentId == Guid.Empty && string.IsNullOrWhiteSpace(myReference.MyReference))
        {
            return false;
        }

        var url = NetsEndpoints.Relative.Payment + "/" + paymentId.ToString("N") + "/myreference";
        var response = await client.PutAsJsonAsync(url, myReference, PaymentSerializationContext.Default.PaymentReference, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            logger.LogInfoUpdatedMyReference(paymentId, myReference);
            return true;
        }

        logger.LogErrorUpdateMyReference(myReference.MyReference!, paymentId, await response.Content.ReadAsStringAsync(cancellationToken));
        return false;
    }

    /// <summary>
    /// Relinquishes the internal http client back to the pool.
    /// </summary>
    public void Dispose()
    {
        client.Dispose();
    }
}
