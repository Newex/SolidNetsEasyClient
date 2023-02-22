using System;
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
    private readonly List<WebHook> notifications = new(32);
    private readonly string complementName;
    private readonly string nonceName;
    private readonly bool simpleAuthorization;

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
    }

    /// <summary>
    /// Create notifications for all events <see cref="EventName"/> using simple authorization.
    /// </summary>
    /// <remarks>
    /// Must have all default webhook endpoints configured
    /// </remarks>
    /// <returns>A notification builder</returns>
    public NetsNotificationBuilder AddNotificationForAllEvents()
    {
        foreach (var eventName in Enum.GetValues<EventName>())
        {
            var url = CreateSimpleUrlToWebhook(eventName, null, null, linkGenerator, baseUrl);
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

            var url = CreateAdvancedUrlToWebhook(eventName, routeName, routeValues, linkGenerator, baseUrl, authorization.Complement, nonce, complementName, nonceName);

            notifications.Add(new()
            {
                Authorization = authorization.Authorization,
                EventName = eventName,
                Url = url
            });
        }
        else
        {
            var url = CreateSimpleUrlToWebhook(eventName, routeName, routeValues, linkGenerator, baseUrl);
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

    internal static string CreateSimpleUrlToWebhook(EventName eventName, string? routeName, object? routeValues, LinkGenerator linkGenerator, string baseUrl)
    {
        var sb = new StringBuilder();
        routeName ??= RouteNamesForAttributes.GetRouteNameByEvent(eventName);
        var webhookUrl = (linkGenerator.GetPathByName(routeName, routeValues) ?? string.Empty).TrimStart('/');
        _ = sb.Append(baseUrl)
          .Append('/')
          .Append(webhookUrl);
        return sb.ToString();
    }

    internal static string CreateAdvancedUrlToWebhook(EventName eventName, string? routeName, object? routeValues, LinkGenerator linkGenerator, string baseUrl, string? complement, string? nonce, string? complementName, string? nonceName)
    {
        var sb = new StringBuilder();
        routeName ??= RouteNamesForAttributes.GetRouteNameByEvent(eventName);
        var webhookUrl = (linkGenerator.GetPathByName(routeName, routeValues) ?? string.Empty).TrimStart('/');
        _ = sb.Append(baseUrl)
          .Append('/')
          .Append(webhookUrl);
        var root = sb.ToString();
        var url = UrlQueryHelpers.AddQuery(root, (complementName, complement), (nonceName, nonce));
        return url;
    }
}
