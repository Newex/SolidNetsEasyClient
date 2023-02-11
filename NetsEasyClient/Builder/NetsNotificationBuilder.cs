using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Routing;
using SolidNetsEasyClient.Extensions;
using SolidNetsEasyClient.Helpers;
using SolidNetsEasyClient.Helpers.Encryption;
using SolidNetsEasyClient.Helpers.Encryption.Encodings;
using SolidNetsEasyClient.Helpers.Encryption.Flows;
using SolidNetsEasyClient.Helpers.Invariants;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Requests.Webhooks;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.Builder;

/// <summary>
/// Notification builder
/// </summary>
public sealed class NetsNotificationBuilder
{
    private readonly string baseUrl;
    private readonly LinkGenerator linkGenerator;
    private readonly IHasher hasher;
    private readonly byte[] key;
    private readonly int nonceLength;
    private readonly string authorizationKey;
    private readonly List<WebHook> notifications = new();
    private readonly string complementName;
    private readonly string nonceName;
    private readonly bool simpleAuthorization;
    private readonly string bulkName;

    internal NetsNotificationBuilder(string baseUrl, LinkGenerator linkGenerator, WebhookEncryptionOptions options)
    {
        this.baseUrl = baseUrl.TrimEnd('/');
        this.linkGenerator = linkGenerator;
        hasher = options.Hasher;
        key = options.Key;
        nonceLength = options.NonceLength;
        authorizationKey = options.AuthorizationKey;
        complementName = options.ComplementName;
        nonceName = options.NonceName;
        simpleAuthorization = options.UseSimpleAuthorization;
        bulkName = options.BulkIndicatorName;
    }

    /// <summary>
    /// Create notification using in-built functionality to construct url and the authorization header. Requires an existing Action handler for the event.
    /// </summary>
    /// <param name="eventName">The event to subscribe to</param>
    /// <param name="order">The order that is being paid</param>
    /// <param name="routeName">The optional route name</param>
    /// <param name="routeValues">The optional route values</param>
    /// <returns>A notification builder</returns>
    public NetsNotificationBuilder AddNotificationForSingleEvent(EventName eventName, Order order, string? routeName = null, object? routeValues = null)
    {
        if (!simpleAuthorization)
        {
            var nonceSource = CustomBase62Converter.Encode(RandomNumberGenerator.GetBytes(256));
            var nonce = nonceSource[..nonceLength];

            var invariant = InvariantConverter.GetInvariant(order, eventName, nonce);
            var authorization = AuthorizationHeaderFlow.CreateAuthorization(hasher, key, invariant);

            routeName ??= RouteNamesForAttributes.GetRouteNameByEvent(eventName);
            var webhookUrl = (linkGenerator.GetPathByName(routeName, routeValues) ?? string.Empty).TrimStart('/');
            var url = UrlQueryHelpers.AddQuery($"{baseUrl}/{webhookUrl}", (complementName, authorization.Complement), (nonceName, nonce));

            notifications.Add(new()
            {
                Authorization = authorization.Authorization,
                EventName = eventName,
                Url = url
            });
        }
        else
        {
            var url = CreateSimpleUrlToWebhook(eventName, routeName, routeValues);
            notifications.Add(new()
            {
                Authorization = authorizationKey,
                EventName = eventName,
                Url = url
            });
        }

        return this;
    }

    /// <summary>
    /// Create notification by hand
    /// </summary>
    /// <param name="eventName">The event to subscribe to</param>
    /// <param name="authorization">The authorization header</param>
    /// <param name="url">The url callback</param>
    /// <returns>A notification builder</returns>
    public NetsNotificationBuilder AddNotificationForSingleEvent(EventName eventName, string authorization, string url)
    {
        notifications.Add(new()
        {
            Authorization = authorization,
            EventName = eventName,
            Url = url
        });

        return this;
    }

    /// <summary>
    /// Create notification for bulk charges
    /// </summary>
    /// <param name="eventName">The event name</param>
    /// <param name="routeName">The optional route name</param>
    /// <param name="routeValues">The optional route values</param>
    /// <returns>A notification builder</returns>
    public NetsNotificationBuilder AddNotificationForBulkEvent(EventName eventName, string? routeName = null, object? routeValues = null)
    {
        var url = UrlQueryHelpers.AddQuery(CreateSimpleUrlToWebhook(eventName, routeName, routeValues), (bulkName, "true"));
        notifications.Add(new()
        {
            Authorization = authorizationKey,
            EventName = eventName,
            Url = url
        });

        return this;
    }

    /// <summary>
    /// Build the notifications
    /// </summary>
    /// <returns>A notifications object</returns>
    public Notification Build()
    {
        return new Notification
        {
            WebHooks = notifications,
        };
    }

    private string CreateSimpleUrlToWebhook(EventName eventName, string? routeName, object? routeValues)
    {
        var sb = new StringBuilder();
        routeName ??= RouteNamesForAttributes.GetRouteNameByEvent(eventName);
        var webhookUrl = (linkGenerator.GetPathByName(routeName, routeValues) ?? string.Empty).TrimStart('/');
        _ = sb.Append(baseUrl)
          .Append('/')
          .Append(webhookUrl);
        return sb.ToString();
    }
}
