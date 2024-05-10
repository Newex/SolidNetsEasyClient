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
    /// Relinquishes the internal http client back to the pool.
    /// </summary>
    public void Dispose()
    {
        client.Dispose();
    }
}
