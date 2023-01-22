using System;
using System.Collections.Generic;
using SolidNetsEasyClient.Models.DTOs;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;
using SolidNetsEasyClient.Models.DTOs.Requests.Webhooks;
using SolidNetsEasyClient.Validators;

namespace SolidNetsEasyClient.Builder;

/// <summary>
/// Bulk builder for unscheduled subscriptions
/// </summary>
public sealed class NetsBulkUnscheduledSubscriptionBuilder
{
    private readonly List<ChargeUnscheduledSubscription> subscriptions = new();
    private readonly List<WebHook> webhooks = new(32);
    private string externalBulkChargeId = string.Empty;

    private NetsBulkUnscheduledSubscriptionBuilder()
    {
    }

    /// <summary>
    /// Set the external bulk charge id, for idempotency (safe retries)
    /// </summary>
    /// <param name="externalBulkChargeId">The external bulk charge id</param>
    /// <returns>A builder</returns>
    public NetsBulkUnscheduledSubscriptionBuilder SetExternalBulkChargeId(string externalBulkChargeId)
    {
        this.externalBulkChargeId = externalBulkChargeId;
        return this;
    }

    /// <summary>
    /// Add an unscheduled subscription to the bulk
    /// </summary>
    /// <param name="unscheduledSubscription">The unscheduled subscription</param>
    /// <returns>A builder</returns>
    public NetsBulkUnscheduledSubscriptionBuilder AddUnscheduledSubscription(ChargeUnscheduledSubscription unscheduledSubscription)
    {
        subscriptions.Add(unscheduledSubscription);
        return this;
    }

    /// <summary>
    /// Add an collection of unscheduled subscriptions to the bulk
    /// </summary>
    /// <param name="bulk">The collection of unschedulec subscriptions</param>
    /// <returns>A builder</returns>
    public NetsBulkUnscheduledSubscriptionBuilder AddBulkUnscheduledSubscriptions(IEnumerable<ChargeUnscheduledSubscription> bulk)
    {
        subscriptions.AddRange(bulk);
        return this;
    }

    /// <summary>
    /// Create a single unscheduled subscription using fluent API
    /// </summary>
    /// <returns>A single unscheduled subscription builder</returns>
    public SingleUnscheduledSubscriptionBuilder CreateSingleUnscheduledSubscription()
    {
        return new SingleUnscheduledSubscriptionBuilder(this);
    }

    /// <summary>
    /// Subscribe to an event
    /// </summary>
    /// <remarks>
    /// To encode the authorization message, a key must be included to encode otherwise the raw authorization message will be used
    /// </remarks>
    /// <param name="eventName">The event to subscribe to</param>
    /// <param name="callbackUrl">The url in which to listen for the event</param>
    /// <param name="authorization">The credentials that will be sent in the HTTP Authorization request header of the callback. Must be between 8 and 32 characters long and contain alphanumeric characters.</param>
    /// <returns>A builder</returns>
    /// <exception cref="ArgumentException">Thrown when invalid authorization</exception>
    public NetsBulkUnscheduledSubscriptionBuilder SubscribeToEvent(EventName eventName, string callbackUrl, string authorization)
    {
        var validAuthorization = PaymentValidator.ProperAuthorization(callbackUrl);
        if (!validAuthorization)
        {
            throw new ArgumentException("Authorization must be between 8 and 32 long and contain alphanumeric characters");
        }
        var webhook = new WebHook()
        {
            EventName = eventName,
            Url = callbackUrl,
            Authorization = authorization
        };

        webhooks.Add(webhook);
        return this;
    }

    /// <summary>
    /// Create an unscheduled subscription builder
    /// </summary>
    /// <returns>A builder</returns>
    public static NetsBulkUnscheduledSubscriptionBuilder CreateBulkBuilder()
    {
        return new NetsBulkUnscheduledSubscriptionBuilder();
    }

    /// <summary>
    /// Build bulk unscheduled subscriptions
    /// </summary>
    /// <returns>A bulk of unscheduled subscriptions</returns>
    public BulkUnscheduledSubscriptionCharge BuildBulk()
    {
        return new BulkUnscheduledSubscriptionCharge()
        {
            ExternalBulkChargeId = externalBulkChargeId,
            Notifications = new()
            {
                WebHooks = webhooks
            },
            UnscheduledSubscriptions = subscriptions
        };
    }

    /// <summary>
    /// A single unscheduled subscription builder
    /// </summary>
    public sealed class SingleUnscheduledSubscriptionBuilder
    {
        private readonly List<Item> items = new();
        private readonly NetsBulkUnscheduledSubscriptionBuilder parentBuilder;
        private Currency currency;
        private string? orderReference;

        internal SingleUnscheduledSubscriptionBuilder(NetsBulkUnscheduledSubscriptionBuilder parentBuilder)
        {
            this.parentBuilder = parentBuilder;
        }

        /// <summary>
        /// Set the currency for the unscheduled subscription
        /// </summary>
        /// <param name="currency">The currency</param>
        /// <returns>A builder</returns>
        public SingleUnscheduledSubscriptionBuilder SetCurrencyForOrder(Currency currency)
        {
            this.currency = currency;
            return this;
        }

        /// <summary>
        /// Set the order reference, usually an order number
        /// </summary>
        /// <param name="orderReference">The order reference</param>
        /// <returns>A builder</returns>
        public SingleUnscheduledSubscriptionBuilder SetOrderReference(string orderReference)
        {
            this.orderReference = orderReference;
            return this;
        }

        /// <summary>
        /// Add an item
        /// </summary>
        /// <param name="reference">The reference to recognize this item</param>
        /// <param name="name">The item name</param>
        /// <param name="unitPrice">The unit price</param>
        /// <param name="quantity">The quantity</param>
        /// <param name="unit">The unit denominator</param>
        /// <param name="taxRate">The tax rate</param>
        /// <returns>A builder</returns>
        public SingleUnscheduledSubscriptionBuilder AddItem(string reference, string name, int unitPrice, double quantity, string unit, int? taxRate = null)
        {
            items.Add(new()
            {
                Reference = reference,
                Name = name,
                Unit = unit,
                UnitPrice = unitPrice,
                Quantity = quantity,
                TaxRate = taxRate,
            });

            return this;
        }

        /// <summary>
        /// Add an item
        /// </summary>
        /// <param name="item">The item</param>
        /// <returns>A builder</returns>
        public SingleUnscheduledSubscriptionBuilder AddItem(Item item)
        {
            items.Add(item);
            return this;
        }

        /// <summary>
        /// Finish building the single unscheduled subscription and add it to the bulk
        /// </summary>
        /// <returns>A bulk builder</returns>
        public NetsBulkUnscheduledSubscriptionBuilder AddFinishedUnscheduledSubscription()
        {
            var subscription = new ChargeUnscheduledSubscription()
            {
                Order = new()
                {
                    Items = items,
                    Currency = currency,
                    Reference = orderReference
                }
            };

            parentBuilder.subscriptions.Add(subscription);
            return parentBuilder;
        }
    }
}
