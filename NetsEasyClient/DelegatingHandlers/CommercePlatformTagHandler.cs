using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SolidNetsEasyClient.Models.Options;

namespace SolidNetsEasyClient.DelegatingHandlers;

/// <summary>
/// Adds the CommercePlatformTag to the http request header.
/// </summary>
/// <param name="options">The nets options</param>
public class CommercePlatformTagHandler(IOptions<NetsEasyOptions> options) : DelegatingHandler
{
    /// <inheritdoc />
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var commercePlatformTag = options.Value.CommercePlatformTag;
        if (!string.IsNullOrWhiteSpace(commercePlatformTag))
        {
            request.Headers.Add("CommercePlatformTag", commercePlatformTag);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
