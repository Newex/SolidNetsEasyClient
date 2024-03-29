namespace SolidNetsEasyClient.Constants;

/// <summary>
/// Constants for client mode
/// </summary>
public static class ClientConstants
{
    /// <summary>
    /// The live mode client operation
    /// </summary>
    public const string Live = "LiveNetsEasyClient";

    /// <summary>
    /// The test mode client operation
    /// </summary>
    public const string Test = "TestNetsEasyClient";
}

/// <summary>
/// An enumeration of client mode operations
/// </summary>
public enum ClientMode
{
    /// <summary>
    /// Test mode
    /// </summary>
    Test = 0,

    /// <summary>
    /// Live mode
    /// </summary>
    Live = 1,
}
