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
public sealed class NetsPaymentClient(
    HttpClient client,
    IOptions<NetsEasyOptions> options,
    ILogger<NetsPaymentClient>? logger = null
) : IDisposable
{
    private readonly HttpClient client = client;
    private readonly ILogger<NetsPaymentClient> logger = logger ?? NullLogger<NetsPaymentClient>.Instance;

    /// <summary>
    /// The checkout key
    /// </summary>
    public string? CheckoutKey => options.Value.CheckoutKey;

    /// <summary>
    /// Send the payment request to NETS to initiate a checkout process.
    /// This should be called before showing the payment page, since the response info should be used on the payment page.
    /// </summary>
    /// <param name="paymentRequest">The payment request</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The payment result</returns>
    /// <exception cref="InvalidOperationException">Thrown if invalid payment request.</exception>
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

    /// <summary>
    /// Retrieve payment details for a payment.
    /// </summary>
    /// <param name="paymentId">The payment id</param>
    /// <param name="cancellationToken">The optional cancellation token</param>
    /// <returns>Payment detail or null</returns>
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

    /// <summary>
    /// Updates the specified payment object with a new reference string and a
    /// checkoutUrl. If you instead want to update the order of a payment
    /// object, use the <see cref="UpdateOrderBeforePayment(Guid, OrderUpdate, CancellationToken)"/>
    /// </summary>
    /// <param name="paymentId">The payment id</param>
    /// <param name="references">The new updated reference information</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>True if updated references otherwise false</returns>
    public async ValueTask<bool> UpdateReferenceInformationBeforePayment(Guid paymentId, ReferenceInformation references, CancellationToken cancellationToken = default)
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

    /// <summary>
    /// Update order, before final payment. Updates the order for the specified
    /// payment. This endpoint makes it possible to change the order on the
    /// checkout page after the payment object has been created. This is
    /// typically used when managing destination-based shipping costs at the
    /// checkout. This endpoint can only be used as long as the checkout has not
    /// yet been completed by the customer. (See the payment.checkout.completed
    /// event.)
    /// </summary>
    /// <param name="paymentId">The payment id</param>
    /// <param name="update">The order updates</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>True if updated otherwise false</returns>
    public async ValueTask<bool> UpdateOrderBeforePayment(Guid paymentId,
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

    /// <summary>
    /// Terminate payment before checkout is completed.
    /// Terminates an ongoing checkout session. A payment can only be terminated
    /// before the checkout has completed (see the payment.checkout event). Use
    /// this method to prevent a customer from having multiple open payment
    /// sessions simultaneously.
    /// </summary>
    /// <param name="paymentId">The payment id</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>True if payment has been terminated otherwise false</returns>
    public async ValueTask<bool> TerminatePaymentBeforePayment(Guid paymentId, CancellationToken cancellationToken = default)
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

    /// <summary>
    /// Relinquishes the internal http client back to the pool.
    /// </summary>
    public void Dispose()
    {
        client.Dispose();
    }
}
