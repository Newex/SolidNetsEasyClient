using System.Collections.Generic;

namespace SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;

public record BulkSubscriptionVerification
{
    public string? ExternalBulkVerificationId { get; init; }
    public IList<BaseSubscription>? Subscriptions { get; init; }
}