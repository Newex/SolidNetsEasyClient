using SolidNetsEasyClient.Models.DTOs.Requests.Payments;

namespace SolidNetsEasyClient.Validators;

internal static class SubscriptionValidator
{
    internal static bool OnlyEitherSubscriptionIdOrExternalRef(SubscriptionCharge subscription)
    {
        var xor = subscription.SubscriptionId.HasValue ^ !string.IsNullOrWhiteSpace(subscription.ExternalReference);
        return xor;
    }
}
