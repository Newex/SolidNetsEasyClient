using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace SolidNetsEasyClient.Extensions;

internal static class HttpClientExtensions
{
    public static Task<HttpResponseMessage> PostAsJsonWithHeadersAsync<T>(this HttpClient client, string path, T payload,
                                                                        JsonTypeInfo<T> serializationContext,
                                                                        (string, string?) header,
                                                                        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(path),
            Content = new StringContent(JsonSerializer.Serialize(payload, serializationContext),
                                        Encoding.UTF8,
                                        new MediaTypeHeaderValue("application/json"))
        };
        if (!string.IsNullOrWhiteSpace(header.Item2))
        {
            request.Headers.Add(header.Item1, header.Item2);
        }
        return client.SendAsync(request, cancellationToken);
    }
}
