using System.Text;

namespace SolidNetsEasyClient.Extensions;

internal static class UrlQueryHelpers
{
    public static string AddQuery(string root, params (string? Key, string? Value)[] query)
    {
        var sb = new StringBuilder(root);
        bool isFirst = true;

        foreach (var (Key, Value) in query)
        {
            if (Key is null || Value is null)
            {
                continue;
            }

            if (isFirst)
            {
                _ = sb.Append('?');
                isFirst = false;
            }
            else
            {
                _ = sb.Append('&');
            }

            _ = sb.Append(Key)
                .Append('=')
                .Append(Value);
        }

        var path = sb.ToString();
        return path;
    }
}
