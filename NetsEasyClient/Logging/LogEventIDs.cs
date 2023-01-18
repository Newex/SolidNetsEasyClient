namespace SolidNetsEasyClient.Logging;

/// <summary>
/// Log event IDs
/// </summary>
public static class LogEventIDs
{
    /// <summary>
    /// Neutral events
    /// </summary>
    public static class Neutral
    {
        /// <summary>
        /// Informational event
        /// </summary>
        public const int Info = 2004;
    }

    /// <summary>
    /// Success events
    /// </summary>
    public static class Success
    {
        /// <summary>
        /// Correct event. Generic catch-all event.
        /// </summary>
        public const int Correct = 2000;
    }

    /// <summary>
    /// Error events
    /// </summary>
    public static class Errors
    {
        /// <summary>
        /// Invalid or malformed input event
        /// </summary>
        public const int Invalid = 4000;

        /// <summary>
        /// Forbidden or unauthorized event
        /// </summary>
        public const int Forbidden = 4003;

        /// <summary>
        /// Missing input or output event
        /// </summary>
        public const int Missing = 4004;

        /// <summary>
        /// Error event. Generic catch-all event.
        /// </summary>
        public const int Error = 5000;
    }
}
