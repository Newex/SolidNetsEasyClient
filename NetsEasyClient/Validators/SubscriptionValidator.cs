using System.Linq;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments.Subscriptions;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;

namespace SolidNetsEasyClient.Validators;

/// <summary>
/// Subscription validator
/// </summary>
public static class SubscriptionValidator
{
    private static bool OnlyEitherSubscriptionIdOrExternalRef(BaseSubscription subscription)
    {
        var xor = subscription.SubscriptionId.HasValue ^ ValidExternalRef(subscription.ExternalReference);
        return xor;
    }

    private static bool ValidExternalRef(string? externalReference)
    {
        return !string.IsNullOrWhiteSpace(externalReference);
    }

    /// <summary>
    /// Only has either an ID or an external reference. Not both.
    /// </summary>
    /// <param name="subscription">The subscription info</param>
    /// <returns>True if valid otherwise false</returns>
    public static bool OnlyEitherSubscriptionIdOrExternalRef(UnscheduledSubscriptionInfo subscription)
    {
        var xor = subscription.UnscheduledSubscriptionId.HasValue ^ ValidExternalRef(subscription.ExternalReference);
        return xor;
    }

    /// <summary>
    /// External bulk charge must be between 1 and 64 characters long
    /// </summary>
    /// <param name="externalBulkChargeId">The bulk charge id</param>
    /// <returns>True if valid otherwise false</returns>
    public static bool ValidateExternalBulkChargeId(string? externalBulkChargeId)
    {
        return !string.IsNullOrWhiteSpace(externalBulkChargeId) && externalBulkChargeId.Length is > 0 and < 64;
    }

    /// <summary>
    /// Validate subscription charge. Must have at least 1 <see cref="ChargeUnscheduledSubscription.Order"/> item and only a <see cref="UnscheduledSubscriptionInfo.UnscheduledSubscriptionId"/> or <see cref="UnscheduledSubscriptionInfo.ExternalReference"/> not both.
    /// </summary>
    /// <param name="subscriptionCharge">The unscheduled subscription charge</param>
    /// <returns>True if valid otherwise false</returns>
    public static bool ValidateSubscriptionCharge(ChargeUnscheduledSubscription subscriptionCharge)
    {
        var valid = subscriptionCharge.Order.Items.Any();
        valid &= OnlyEitherSubscriptionIdOrExternalRef(subscriptionCharge);
        return valid;
    }

    /// <summary>
    /// Validate subscription charge. Must have at least 1 <see cref="SubscriptionCharge.Order"/> item and only a <see cref="BaseSubscription.SubscriptionId"/> or <see cref="BaseSubscription.ExternalReference"/> not both.
    /// </summary>
    /// <param name="subscriptionCharge">The subscription charge</param>
    /// <returns>True if valid otherwise false</returns>
    public static bool ValidateSubscriptionCharge(SubscriptionCharge subscriptionCharge)
    {
        var valid = subscriptionCharge.Order.Items.Any();
        valid &= OnlyEitherSubscriptionIdOrExternalRef(subscriptionCharge);
        return valid;
    }
}
