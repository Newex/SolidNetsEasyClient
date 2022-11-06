using System;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SolidNetsEasyClient.Constants;
using SolidNetsEasyClient.Models;
using SolidNetsEasyClient.Models.Requests;
using SolidNetsEasyClient.Models.Results;
using SolidNetsEasyClient.Validators;

namespace SolidNetsEasyClient.Clients;

public partial class PaymentClient : IPaymentClient
{
    /// <inheritdoc />
    public async Task<PaymentResult> CreatePaymentAsync(PaymentRequest payment, CancellationToken cancellationToken, bool validate = true)
    {
        var isValid = !validate || (PaymentValidator.IsValidPaymentObject(payment) && !string.IsNullOrWhiteSpace(apiKey));
        if (!isValid)
        {
            logger.LogError("Invalid {@Payment} or {ApiKey}", payment, apiKey);
            throw new ArgumentException("Invalid payment object state or api key", nameof(payment));
        }

        try
        {
            logger.LogTrace("Creating new {@Payment}", payment);
            var client = httpClientFactory.CreateClient(mode);

            AddHeaders(client);

            // Body
            var response = await client.PostAsJsonAsync(NetsEndpoints.Relative.Payment, payment, cancellationToken);
            var msg = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogTrace("Raw content: {@ResponseContent}", msg);
            response.EnsureSuccessStatusCode();

            // Response
            var result = await response.Content.ReadFromJsonAsync<PaymentResult>(cancellationToken: cancellationToken);
            logger.LogInformation("Created {@Payment} with a {@Result}", payment, result);
            return result!;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred trying to create a payment with {@Payment}", payment);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PaymentResult> CreatePaymentAsync(Order order, Integration integration, CancellationToken cancellationToken, bool charge = true, string? checkoutUrl = null, string? returnUrl = null, string? termsUrl = null, bool validate = true)
    {
        // load optional values
        var hostedReturnUrl = integration switch
        {
            Integration.EmbeddedCheckout => null,
            Integration.HostedPaymentPage => returnUrl ?? this.returnUrl,
            _ => throw new NotImplementedException()
        };
        var payment = new PaymentRequest
        {
            Order = order,
            Checkout = new Checkout
            {
                Url = checkoutUrl ?? this.checkoutUrl,
                ReturnUrl = hostedReturnUrl,
                TermsUrl = termsUrl ?? this.termsUrl,
                MerchantTermsUrl = merchantTermsUrl,
                IntegrationType = integration,
                Charge = charge
            }
        };
        var isValid = !validate || (PaymentValidator.IsValidPaymentObject(payment) && !string.IsNullOrWhiteSpace(apiKey));
        if (!isValid)
        {
            logger.LogError("Invalid {@Payment} or {ApiKey}", payment, apiKey);
            throw new ArgumentException("Invalid order object state or api key", nameof(order));
        }

        try
        {
            logger.LogTrace("Creating new {@Payment}", payment);
            var client = httpClientFactory.CreateClient(mode);

            AddHeaders(client);

            // Body
            var response = await client.PostAsJsonAsync(NetsEndpoints.Relative.Payment, payment, cancellationToken);
            var msg = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogTrace("Raw content: {@ResponseContent}", msg);
            response.EnsureSuccessStatusCode();

            // Response
            var result = await response.Content.ReadFromJsonAsync<PaymentResult>(cancellationToken: cancellationToken);
            logger.LogInformation("Created {@Payment} with a {@Result}", payment, result);
            return result!;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred trying to create a payment with {@Payment}", payment);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PaymentResult> CreatePaymentAsync(Order order, Consumer consumer, CancellationToken cancellationToken, bool charge = true, string? checkoutUrl = null, string? termsUrl = null, bool validate = true)
    {
        // Assume: Embedded integration
        var payment = new PaymentRequest
        {
            Order = order,
            Checkout = new Checkout
            {
                Url = checkoutUrl ?? this.checkoutUrl,
                TermsUrl = termsUrl ?? this.termsUrl,
                MerchantTermsUrl = merchantTermsUrl,
                IntegrationType = Integration.EmbeddedCheckout,
                Consumer = consumer,
                MerchantHandlesConsumerData = true,
                Charge = charge
            }
        };
        var isValid = !validate || (PaymentValidator.IsValidPaymentObject(payment) && !string.IsNullOrWhiteSpace(apiKey));
        if (!isValid)
        {
            logger.LogError("Invalid {@Payment} or {ApiKey}", payment, apiKey);
            throw new ArgumentException("Invalid order object state or api key", nameof(order));
        }

        try
        {
            logger.LogTrace("Creating new {@Payment}", payment);
            var client = httpClientFactory.CreateClient(mode);

            AddHeaders(client);

            // Body
            var response = await client.PostAsJsonAsync(NetsEndpoints.Relative.Payment, payment, cancellationToken);
            var msg = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogTrace("Raw content: {@ResponseContent}", msg);
            response.EnsureSuccessStatusCode();

            // Response
            var result = await response.Content.ReadFromJsonAsync<PaymentResult>(cancellationToken: cancellationToken);
            logger.LogInformation("Created {@Payment} with a {@Result}", payment, result);
            return result!;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred trying to create a payment with {@Payment}", payment);
            throw;
        }
    }
}
