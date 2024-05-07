using VerifyTests;

namespace SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests.SnapshotTests;

public static class SnapshotSettings
{
    public static VerifySettings Settings
    {
        get
        {
            var settings = new VerifySettings();
            settings.UseDirectory("./JsonResults");
            return settings;
        }
    }
}
