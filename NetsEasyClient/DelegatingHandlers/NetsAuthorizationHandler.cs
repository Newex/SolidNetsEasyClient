using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.DelegatingHandlers;

/// <summary>
/// Adds the API key to the http request.
/// </summary>
/// <param name="options">The nets options</param>
public class NetsAuthorizationHandler(IOptions<NetsEasyOptions> options) : DelegatingHandler
{
    /// <inheritdoc />
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("Authorization", options.Value.ApiKey);
        return base.SendAsync(request, cancellationToken);
    }
}
