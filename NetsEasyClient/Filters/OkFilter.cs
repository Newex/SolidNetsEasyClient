using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SolidNetsEasyClient.Filters;

/// <summary>
/// Convert success to 200 OK
/// </summary>
public class OkFilter : IEndpointFilter
{
    /// <inheritdoc />
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        context.HttpContext.Response.OnStarting(obj =>
        {
            var ctx = (HttpContext)obj;
            var status = ctx.Response.StatusCode;
            if (status is not 200 and > 200 and < 300)
            {
                ctx.Response.StatusCode = 200;
            }

            return Task.CompletedTask;
        }, context.HttpContext);

        return await next(context);
    }
}
