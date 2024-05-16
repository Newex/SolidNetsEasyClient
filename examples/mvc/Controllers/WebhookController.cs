using System.Threading;
using Microsoft.AspNetCore.Mvc;
using SolidNetsEasyClient.Filters;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;

namespace ExampleSite.Controllers;

[Route("/webhook")]
[SolidNetsEasyIPFilter]
public sealed class WebhookController : Controller
{
    /// <summary>
    /// default hooks to implement custom logic handler
    /// </summary>
    /// <param name="charge">The charge webhook event</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>200 OK if handled event - note that other results (i.e. 201 or 404) will make Nets renotify for a total of 10 times.</returns>
    [HttpPost("nets/my_charge")]
    [SolidNetsEasyIPFilter]
    public ActionResult ChargeCallback([FromBody] ChargeCreated charge, CancellationToken cancellationToken)
    {
        // Handle charge...
        return Ok();
    }
}