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
using SolidNetsEasyClient.Models;
using SolidNetsEasyClient.Models.Options;
using SolidNetsEasyClient.Models.Results;
using SolidNetsEasyClient.Models.Status;

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
        IOptions<PlatformPaymentOptions> options,
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
        checkoutUrl = options.Value.CheckoutUrl;
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
    public async Task<PaymentStatus?> GetPaymentStatusAsync(Guid paymentID, CancellationToken cancellationToken)
    {
        var isValid = paymentID != Guid.Empty;
        if (!isValid)
        {
            logger.LogError("Invalid {PaymentID}", paymentID);
            return null;
        }

        try
        {
            logger.LogTrace("Retrieving payment status for {PaymentID}", paymentID);

            var client = httpClientFactory.CreateClient(mode);

            // Headers
            AddHeaders(client);

            // Send
            var path = NetsEndpoints.Relative.Payment + $"/{paymentID}";
            var response = await client.GetAsync(path, cancellationToken);
            var msg = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogTrace("Raw content: {@ResponseContent}", msg);
            response.EnsureSuccessStatusCode();

            // Success response
            var result = await response.Content.ReadFromJsonAsync<PaymentStatus>(cancellationToken: cancellationToken);
            logger.LogInformation("Retrieved {@PaymentStatus}", result);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred trying to retrieve payment status for {PaymentID}", paymentID);
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
            logger.LogError("Invalid {@Payment} or {CheckoutUrl} or {Reference}", payment, checkoutUrl, reference);
            throw new ArgumentException("Invalid payment, checkout url or reference");
        }

        try
        {
            logger.LogTrace("Updating references for {@Payment} to {CheckoutUrl} and {Reference}", payment, checkoutUrl, reference);

            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(client);

            var path = NetsEndpoints.Relative.Payment + $"/{payment.PaymentId}" + "/referenceinformation";
            var response = await client.PutAsJsonAsync(path, new
            {
                checkoutUrl = checkoutUrl,
                reference = reference
            }, cancellationToken);

            var msg = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogTrace("Raw content: {@ResponseContent}", msg);

            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred trying to update payment references for {@Payment} and {CheckoutUrl} and {Reference}", payment, checkoutUrl, reference);
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
            logger.LogError("Invalid {@OrderUpdate} or {PaymentID}", updates, paymentID);
            return false;
        }

        try
        {
            logger.LogTrace("Updating order for {PaymentID} to {@OrderUpdates}", paymentID, updates);

            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(client);

            var path = NetsEndpoints.Relative.Payment + $"/{paymentID}/orderitems";
            var response = await client.PutAsJsonAsync(path, updates, cancellationToken);

            var msg = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogTrace("Raw content: {@ResponseContent}", msg);

            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred updating order items for {PaymentID} and {@OrderUpdates}", paymentID, updates);
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<bool> TerminatePaymentAsync(Guid paymentID, CancellationToken cancellationToken)
    {
        var isValid = paymentID != Guid.Empty;
        if (!isValid)
        {
            logger.LogError("Missing id: {PaymentID}", paymentID);
            return false;
        }

        try
        {
            logger.LogTrace("Terminating checkout for {PaymentID}", paymentID);

            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(client);

            var path = NetsEndpoints.Relative.Payment + $"/{paymentID}/terminate";
            var response = await client.PutAsync(path, null, cancellationToken);

            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred terminating checkout for {PaymentID}", paymentID);
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<bool> CancelPaymentAsync(Guid paymentID, Order order, CancellationToken cancellationToken)
    {
        var isValid = paymentID != Guid.Empty && order.Items.Any();
        if (!isValid)
        {
            logger.LogError("Missing id: {PaymentID}", paymentID);
            return false;
        }

        try
        {
            logger.LogTrace("Cancelling checkout for {PaymentID}", paymentID);

            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(client);

            var path = NetsEndpoints.Relative.Payment + $"/{paymentID}/cancels";
            var response = await client.PutAsJsonAsync(path, new
            {
                Amount = order.Amount,
                OrderItems = order.Items
            }, cancellationToken);

            var msg = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogTrace("Raw content: {@ResponseContent}", msg);

            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred cancelling checkout for {PaymentID}", paymentID);
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<ChargeResult?> ChargePaymentAsync(Guid paymentID, Charge charge, CancellationToken cancellationToken, string? idempotencyKey = null)
    {
        var isValid = paymentID != Guid.Empty && (idempotencyKey is null || (idempotencyKey.Length > 0 && idempotencyKey.Length < 64));
        if (!isValid)
        {
            logger.LogError("Invalid {PaymentID} or {IdempotencyKey}", paymentID, idempotencyKey);
            return null;
        }

        try
        {
            logger.LogTrace("Charging payment for {PaymentID}", paymentID);

            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(client);
            if (idempotencyKey is not null)
            {
                client.DefaultRequestHeaders.Add("Idempotency-Key", idempotencyKey);
            }

            var path = NetsEndpoints.Relative.Payment + $"/{paymentID}/charges";
            var response = await client.PostAsJsonAsync(path, charge, cancellationToken);
            response.EnsureSuccessStatusCode();

            // Success response
            var result = await response.Content.ReadFromJsonAsync<ChargeResult>(cancellationToken: cancellationToken);
            logger.LogInformation("Charged {PaymentID} with {@Charge} and got {@Result}", paymentID, charge, result);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred finalizing charge for {PaymentID}", paymentID);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<ChargeDetailsInfo?> RetrieveChargeAsync(Guid chargeId, CancellationToken cancellationToken)
    {
        var isValid = chargeId != Guid.Empty;
        if (!isValid)
        {
            logger.LogError("Invalid {ChargeID}", chargeId);
            return null;
        }

        try
        {
            logger.LogTrace("Retrieving charge for {ChargeID}", chargeId);

            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(client, withCommercePlatform: false);

            var path = NetsEndpoints.Relative.Charge + $"/{chargeId}";
            var response = await client.GetAsync(path, cancellationToken);

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ChargeDetailsInfo>(cancellationToken: cancellationToken);
            logger.LogInformation("Retrieved charge: {@ChargeDetails}", result);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred retrieving charge for {ChargeID}", chargeId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<RefundResult?> RefundChargeAsync(Guid chargeId, Refund refund, CancellationToken cancellationToken, string? idempotencyKey = null)
    {
        var isValid = chargeId != Guid.Empty && (idempotencyKey is null || (idempotencyKey.Length > 0 && idempotencyKey.Length < 64));
        if (!isValid)
        {
            logger.LogError("Invalid {ChargeID} or {IdempotencyKey}", chargeId, idempotencyKey);
            return null;
        }

        try
        {
            logger.LogTrace("Refunding charge {ChargeID}", chargeId);

            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(client, withCommercePlatform: false);

            var path = NetsEndpoints.Relative.Charge + $"/{chargeId}/refunds";
            var response = await client.PostAsJsonAsync(path, refund, cancellationToken);

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<RefundResult>(cancellationToken: cancellationToken);
            logger.LogInformation("Refunded: {ChargeID} for {@Refund}", chargeId, refund);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred refunding charge for {ChargeID}", chargeId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<RetrieveRefund?> RetrieveRefundAsync(Guid refundId, CancellationToken cancellationToken)
    {
        var isValid = refundId != Guid.Empty;
        if (!isValid)
        {
            logger.LogError("Invalid {RefundId}", refundId);
            return null;
        }

        try
        {
            logger.LogTrace("Retrieving refund for {RefundId}", refundId);

            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(client, withCommercePlatform: false);

            var path = NetsEndpoints.Relative.Refund + $"/{refundId}";
            var response = await client.GetAsync(path, cancellationToken);

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<RetrieveRefund>(cancellationToken: cancellationToken);
            logger.LogInformation("Refund result: {@Refund}", result);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred retrieving refund {RefundId}", refundId);
            return null;
        }
    }

    /// <inheritdoc />
    public async Task<bool> CancelPendingRefundAsync(Guid refundId, CancellationToken cancellationToken)
    {
        var isValid = refundId != Guid.Empty;
        if (!isValid)
        {
            logger.LogError("Invalid {RefundId}", refundId);
            return false;
        }

        try
        {
            logger.LogTrace("Cancelling pending refund for {RefundId}", refundId);

            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(client, withCommercePlatform: false);

            var path = NetsEndpoints.Relative.PendingRefunds + $"/{refundId}/cancel";
            var response = await client.PostAsync(path, null, cancellationToken);

            response.EnsureSuccessStatusCode();
            logger.LogInformation("Cancelled pending refund {RefundId}", refundId);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred cancelling pending refund {RefundId}", refundId);
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<bool> UpdatePaymentReferenceAsync(Guid paymentId, string? myReference, CancellationToken cancellationToken)
    {
        var isValid = paymentId != Guid.Empty;
        if (!isValid)
        {
            logger.LogError("Invalid {PaymentId}", paymentId);
            throw new ArgumentException("Missing refund id", nameof(paymentId));
        }

        try
        {
            logger.LogTrace("Updating my reference for {PaymentId}", paymentId);

            var client = httpClientFactory.CreateClient(mode);
            AddHeaders(client);

            var path = NetsEndpoints.Relative.Payment + $"/{paymentId}/myreference";
            var response = await client.PutAsJsonAsync(path, new
            {
                myReference = myReference
            }, cancellationToken);

            response.EnsureSuccessStatusCode();
            logger.LogInformation("Updated my reference to {MyReference} for {PaymentId}", myReference, paymentId);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred updating my reference {MyReference} for {PaymentId}", paymentId);
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
