using System;

namespace SolidNetsEasyClient.Constants;

/// <summary>
/// Constant Nets endpoint addresses
/// </summary>
public static class NetsEndpoints
{
    /// <summary>
    /// Live base URI environment base address
    /// </summary>
    /// <remarks>
    /// https://api.dibspayment.eu
    /// </remarks>
    public static Uri LiveBaseUri => new("https://api.dibspayment.eu");

    /// <summary>
    /// Testing base URI environment base address
    /// </summary>
    /// <remarks>
    /// https://test.api.dibspayment.eu
    /// </remarks>
    public static Uri TestingBaseUri => new("https://test.api.dibspayment.eu");

    /// <summary>
    /// Relative address endpoints
    /// </summary>
    public static class Relative
    {
        /// <summary>
        /// The payment endpoint
        /// </summary>
        public const string Payment = "/v1/payments";

        /// <summary>
        /// The charge endpoint
        /// </summary>
        public const string Charge = "/v1/charges";

        /// <summary>
        /// The refund endpoint
        /// </summary>
        public const string Refund = "/v1/refunds";

        /// <summary>
        /// The pending refunds endpoint
        /// </summary>
        public const string PendingRefunds = "/v1/pending-refunds";
    }

    /// <summary>
    /// The ip range for where the webhook callback originate from
    /// </summary>
    public static class WebhookIPs
    {
        /// <summary>
        /// Default IP range for the live Nets Easy webhook callback client
        /// </summary>
        public const string LiveIPRange = "20.103.218.104/30";

        /// <summary>
        /// Default IP range for the test Nets Easy webhook callback client
        /// </summary>
        public const string TestIPRange = "20.31.57.60/30";
    }
}