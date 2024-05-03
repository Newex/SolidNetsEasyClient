using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;
using SolidNetsEasyClient.Models.Options;
using SolidNetsEasyClient.Logging.PaymentClientLogging;

namespace SolidNetsEasyClient.Clients;

/// <inheritdoc cref="IPaymentClient" />
public partial class PaymentClient : IPaymentClient
{
    private readonly string merchantTermsUrl;
    private readonly string checkoutUrl;
    private readonly string termsUrl;
    private readonly string? returnUrl;
    private readonly string? cancelUrl;
    private readonly string mode;
    private readonly string apiKey;
    private readonly string? platformId;
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ILogger<PaymentClient> logger;

    /// <summary>
    /// Instantiate a new object of <see cref="PaymentClient"/>
    /// </summary>
    /// <param name="options">The platform payment options</param>
    /// <param name="httpClientFactory">The http client factory</param>
    /// <param name="logger">The optional logger</param>
    public PaymentClient(
        IOptions<NetsEasyOptions> options,
        IHttpClientFactory httpClientFactory,
        ILogger<PaymentClient>? logger = null
    )
    {
        mode = options.Value.ClientMode switch
        {
            ClientMode.Live => ClientConstants.Live,
            ClientMode.Test => ClientConstants.Test,
            _ => throw new NotSupportedException("Client mode must be either in Live or Test mode")
        };
        checkoutUrl = options.Value.CheckoutUrl ?? string.Empty;
        termsUrl = options.Value.TermsUrl;
        returnUrl = options.Value.ReturnUrl;
        cancelUrl = options.Value.CancelUrl;
        merchantTermsUrl = options.Value.PrivacyPolicyUrl;
        apiKey = options.Value.ApiKey;
        CheckoutKey = options.Value.CheckoutKey;
        platformId = options.Value.CommercePlatformTag;
        this.httpClientFactory = httpClientFactory;
        this.logger = logger ?? NullLogger<PaymentClient>.Instance;
    }

    /// <inheritdoc />
    public string CheckoutKey { get; }

    /// <inheritdoc />
    public async Task<PaymentStatus?> RetrievePaymentAsync(Guid paymentID, CancellationToken cancellationToken)
    {
        var isValid = paymentID != Guid.Empty;
        if (!isValid)
        {
            logger.ErrorInvalidPaymentID(paymentID);
            return null;
        }

        try
        {
            logger.TracePaymentRetrieval(paymentID);

            var client = httpClientFactory.CreateClient(mode);

            // Headers
            AddHeaders(client);

            // Send
            var path = NetsEndpoints.Relative.Payment + $"/{paymentID}";
            var response = await client.GetAsync(path, cancellationToken);
            var msg = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.TraceRawResponse(msg);
            _ = response.EnsureSuccessStatusCode();

            // Success response
            var result = await response.Content.ReadFromJsonAsync<PaymentStatus>(cancellationToken: cancellationToken);
            logger.InfoRetrievedPayment(result!);
            return result;
        }
        catch (Exception ex)
        {
            logger.ErrorRetrievalException(paymentID, ex);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<bool> UpdateReferences(Payment payment, string checkoutUrl, string reference, CancellationToken cancellationToken)
    {
        var oldHost = new Uri(payment.Checkout.Url);
        var newHost = new Uri(checkoutUrl);
        var isValid = payment.PaymentId != Guid.Empty && !string.IsNullOrWhiteSpace(checkoutUrl) && !string.IsNullOrWhiteSpace(reference) && string.Equals(oldHost.Host, newHost.Host, StringComparison.OrdinalIgnoreCase);
        if (!isValid)
        {
            logger.ErrorInvalidPaymentReferenceUpdates(payment, checkoutUrl, reference);
            throw new ArgumentException("Invalid payment, checkout url or reference");
        }

        try
        {
            logger.TraceUpdatingReferences(payment, checkoutUrl, reference);

            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(client);

            var path = NetsEndpoints.Relative.Payment + $"/{payment.PaymentId}" + "/referenceinformation";
            var response = await client.PutAsJsonAsync(path, new
            {
                checkoutUrl = checkoutUrl,
                reference = reference
            }, cancellationToken);

            var msg = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.TraceRawResponse(msg);

            _ = response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            logger.ExceptionUpdatingReferences(payment, checkoutUrl, reference, ex);
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<bool> UpdateOrderAsync(Guid paymentID, OrderUpdate updates, CancellationToken cancellationToken)
    {
        // If has payment method the:
        // * Name
        // * Reference
        // * Unit
        // Are required!
        var validPayment = true;
        if (updates.PaymentMethods is not null)
        {
            validPayment = updates.PaymentMethods.All(x => !string.IsNullOrWhiteSpace(x.Fee.Name)
                                                           && !string.IsNullOrWhiteSpace(x.Fee.Reference)
                                                           && !string.IsNullOrWhiteSpace(x.Fee.Unit))
                           || (updates.PaymentMethods is null);
        }
        var isValid = paymentID != Guid.Empty && validPayment;
        if (!isValid)
        {
            logger.ErrorInvalidOrderOrPayment(updates, paymentID);
            return false;
        }

        try
        {
            logger.TracePaymentUpdate(paymentID, updates);

            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(client);

            var path = NetsEndpoints.Relative.Payment + $"/{paymentID}/orderitems";
            var response = await client.PutAsJsonAsync(path, updates, cancellationToken);

            var msg = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.TraceRawResponse(msg);

            _ = response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            logger.ExceptionUpdatingOrder(paymentID, updates, ex);
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<bool> TerminatePaymentAsync(Guid paymentID, CancellationToken cancellationToken)
    {
        var isValid = paymentID != Guid.Empty;
        if (!isValid)
        {
            logger.ErrorMissingPaymentID(paymentID);
            return false;
        }

        try
        {
            logger.TraceTermination(paymentID);

            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(client);

            var path = NetsEndpoints.Relative.Payment + $"/{paymentID}/terminate";
            var response = await client.PutAsync(path, null, cancellationToken);

            _ = response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            logger.ExceptionTerminatingPayment(paymentID, ex);
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<bool> CancelPaymentAsync(Guid paymentID, Order order, CancellationToken cancellationToken)
    {
        var isValid = paymentID != Guid.Empty && order.Items.Any();
        if (!isValid)
        {
            logger.ErrorMissingPaymentID(paymentID);
            return false;
        }

        try
        {
            logger.TraceCancellation(paymentID);

            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(client);

            var path = NetsEndpoints.Relative.Payment + $"/{paymentID}/cancels";
            var response = await client.PutAsJsonAsync(path, new
            {
                Amount = order.Amount,
                OrderItems = order.Items
            }, cancellationToken);

            var msg = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.TraceRawResponse(msg);

            _ = response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            logger.ExceptionCancellingPayment(paymentID, ex);
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<ChargeResult?> ChargePaymentAsync(Guid paymentID, Charge charge, CancellationToken cancellationToken, string? idempotencyKey = null)
    {
        var isValid = paymentID != Guid.Empty && (idempotencyKey is null || (idempotencyKey.Length > 0 && idempotencyKey.Length < 64));
        if (!isValid)
        {
            logger.ErrorInvalidPaymentOrIdempotencyKey(paymentID, idempotencyKey);
            return null;
        }

        try
        {
            logger.TraceChargingPayment(paymentID);

            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(client);
            if (idempotencyKey is not null)
            {
                client.DefaultRequestHeaders.Add("Idempotency-Key", idempotencyKey);
            }

            var path = NetsEndpoints.Relative.Payment + $"/{paymentID}/charges";
            var response = await client.PostAsJsonAsync(path, charge, cancellationToken);
            _ = response.EnsureSuccessStatusCode();

            // Success response
            var result = await response.Content.ReadFromJsonAsync<ChargeResult>(cancellationToken: cancellationToken);
            logger.InfoChargeResult(paymentID, charge, result!);
            return result;
        }
        catch (Exception ex)
        {
            logger.ExceptionChargingPayment(paymentID, ex);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<ChargeDetailsInfo?> RetrieveChargeAsync(Guid chargeId, CancellationToken cancellationToken)
    {
        var isValid = chargeId != Guid.Empty;
        if (!isValid)
        {
            logger.ErrorInvalidChargeID(chargeId);
            return null;
        }

        try
        {
            logger.TraceChargeRetrieval(chargeId);

            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(client, withCommercePlatform: false);

            var path = NetsEndpoints.Relative.Charge + $"/{chargeId}";
            var response = await client.GetAsync(path, cancellationToken);

            _ = response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ChargeDetailsInfo>(cancellationToken: cancellationToken);
            logger.InfoChargeDetails(result!);
            return result;
        }
        catch (Exception ex)
        {
            logger.ExceptionRetrievingCharge(chargeId, ex);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<RefundResult?> RefundChargeAsync(Guid chargeId, Refund refund, CancellationToken cancellationToken, string? idempotencyKey = null)
    {
        var isValid = chargeId != Guid.Empty && (idempotencyKey is null || (idempotencyKey.Length > 0 && idempotencyKey.Length < 64));
        if (!isValid)
        {
            logger.ErrorInvalidChargeOrIdempotencyKey(chargeId, idempotencyKey);
            return null;
        }

        try
        {
            logger.TraceRefund(chargeId);

            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(client, withCommercePlatform: false);

            var path = NetsEndpoints.Relative.Charge + $"/{chargeId}/refunds";
            var response = await client.PostAsJsonAsync(path, refund, cancellationToken);

            _ = response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<RefundResult>(cancellationToken: cancellationToken);
            logger.InfoRefundCharge(chargeId, refund);
            return result;
        }
        catch (Exception ex)
        {
            logger.ExceptionRefundCharge(chargeId, ex);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<RetrieveRefund?> RetrieveRefundAsync(Guid refundId, CancellationToken cancellationToken)
    {
        var isValid = refundId != Guid.Empty;
        if (!isValid)
        {
            logger.ErrorInvalidRefundID(refundId);
            return null;
        }

        try
        {
            logger.TraceRetrievingRefund(refundId);

            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(client, withCommercePlatform: false);

            var path = NetsEndpoints.Relative.Refund + $"/{refundId}";
            var response = await client.GetAsync(path, cancellationToken);

            _ = response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<RetrieveRefund>(cancellationToken: cancellationToken);
            logger.InfoRetrieveRefund(result!);
            return result;
        }
        catch (Exception ex)
        {
            logger.ExceptionRetrieveRefund(refundId, ex);
            return null;
        }
    }

    /// <inheritdoc />
    public async Task<bool> CancelPendingRefundAsync(Guid refundId, CancellationToken cancellationToken)
    {
        var isValid = refundId != Guid.Empty;
        if (!isValid)
        {
            logger.ErrorInvalidRefundID(refundId);
            return false;
        }

        try
        {
            logger.TraceCancelPendingRefund(refundId);

            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(client, withCommercePlatform: false);

            var path = NetsEndpoints.Relative.PendingRefunds + $"/{refundId}/cancel";
            var response = await client.PostAsync(path, null, cancellationToken);

            _ = response.EnsureSuccessStatusCode();
            logger.InfoCancelledPendingRefund(refundId);
            return true;
        }
        catch (Exception ex)
        {
            logger.ExceptionCancellingRefund(refundId, ex);
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<bool> UpdatePaymentReferenceAsync(Guid paymentId, string? myReference, CancellationToken cancellationToken)
    {
        var isValid = paymentId != Guid.Empty;
        if (!isValid)
        {
            logger.ErrorInvalidPaymentID(paymentId);
            throw new ArgumentException("Missing refund id", nameof(paymentId));
        }

        try
        {
            logger.TraceUpdatePaymentReference(paymentId);

            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(client);

            var path = NetsEndpoints.Relative.Payment + $"/{paymentId}/myreference";
            var response = await client.PutAsJsonAsync(path, new
            {
                myReference = myReference
            }, cancellationToken);

            _ = response.EnsureSuccessStatusCode();
            logger.InfoUpdatedReferences(myReference, paymentId);
            return true;
        }
        catch (Exception ex)
        {
            logger.ExceptionUpdatingPaymentReferences(myReference, paymentId, ex);
            return false;
        }
    }

    private void AddHeaders(HttpClient client, bool withCommercePlatform = true)
    {
        // Headers
        if (!string.IsNullOrWhiteSpace(platformId) && withCommercePlatform)
        {
            client.DefaultRequestHeaders.Add("CommercePlatformTag", platformId);
        }
        client.DefaultRequestHeaders.Add("Authorization", apiKey);
    }
}
