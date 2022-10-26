using System;

namespace SolidNetsEasyClient.Constants;

public static class ClientConstants
{
    public const string Live = "LiveNetsEasyClient";

    public const string Test = "TestNetsEasyClient";
}

public enum ClientMode
{
    Live,
    Test
}