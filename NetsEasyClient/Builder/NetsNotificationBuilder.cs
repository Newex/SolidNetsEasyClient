using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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
    private readonly List<WebHook> notifications = new();
    private readonly string complementName;
    private readonly string nonceName;

    internal NetsNotificationBuilder(string baseUrl, string complementName, string nonceName, LinkGenerator linkGenerator, IHasher hasher, byte[] key, int nonceLength)
    {
        if (nonceLength > 256)
        {
            throw new ArgumentOutOfRangeException(nameof(nonceLength));
        }

        this.baseUrl = baseUrl.TrimEnd('/');
        this.linkGenerator = linkGenerator;
        this.hasher = hasher;
        this.key = key;
        this.nonceLength = nonceLength;
        this.complementName = complementName;
        this.nonceName = nonceName;
    }

    /// <summary>
    /// Create notification using in-built functionality to construct url and the authorization header. Requires an existing Action handler for the event.
    /// </summary>
    /// <param name="eventName">The event to subscribe to</param>
    /// <param name="order">The order that is being paid</param>
    /// <param name="routeName">The optional route name</param>
    /// <param name="routeValues">The optional route values</param>
    /// <returns>A notification builder</returns>
    public NetsNotificationBuilder AddNotificationForEvent(EventName eventName, Order order, string? routeName = null, object? routeValues = null)
    {
        var nonceSource = CustomBase62Converter.Encode(RandomNumberGenerator.GetBytes(256));
        var nonce = nonceSource[..nonceLength];

        var invariant = InvariantConverter.GetInvariant(order, eventName, nonce);
        var authorization = AuthorizationHeaderFlow.CreateAuthorization(hasher, key, invariant);

        routeName ??= RouteNamesForAttributes.GetRouteNameByEvent(eventName);
        var webhookUrl = (linkGenerator.GetPathByName(routeName, routeValues) ?? string.Empty).TrimStart('/');
        var url = UrlQueryHelpers.AddQuery($"{baseUrl}/{webhookUrl}", (complementName, authorization.Complement), (nonceName, nonce));

        notifications.Add(new WebHook
        {
            Authorization = authorization.Authorization,
            EventName = eventName,
            Url = url
        });

        return this;
    }

    /// <summary>
    /// Create notification by hand
    /// </summary>
    /// <param name="eventName">The event to subscribe to</param>
    /// <param name="authorization">The authorization header</param>
    /// <param name="url">The url callback</param>
    /// <returns>A notification builder</returns>
    public NetsNotificationBuilder AddNotificationForEvent(EventName eventName, string authorization, string url)
    {
        notifications.Add(new WebHook
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
}
