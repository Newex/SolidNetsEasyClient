namespace SolidNetsEasyClient.Helpers.Invariants;

/// <summary>
/// Object is invariant serializable
/// </summary>
public interface IInvariantSerializable
{
    /// <summary>
    /// Get the byte invariants. Those that are the same for sending request and when receiving response
    /// </summary>
    /// <returns>The byte invariants</returns>
    byte[] GetBytes();
}
