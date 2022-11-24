using SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;

namespace SolidNetsEasyClient.Validators;

internal static class SubscriptionValidator
{
    internal static bool OnlyEitherSubscriptionIdOrExternalRef(BaseSubscription subscription)
    {
        var xor = subscription.SubscriptionId.HasValue ^ !string.IsNullOrWhiteSpace(subscription.ExternalReference);
        return xor;
    }
}
