using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SolidNetsEasyClient.Filters;

namespace ExampleSite.Controllers;

public class WebhookController : Controller
{
    private readonly ILogger<WebhookController> logger;

    public WebhookController(
        ILogger<WebhookController>? logger = null
    )
    {
        this.logger = logger ?? NullLogger<WebhookController>.Instance;
    }

    [WebhookIPFilter]
    [HttpPost("/webhook")]
    public ActionResult Post([FromBody] dynamic jsonData)
    {
        string data = JsonSerializer.Deserialize<dynamic>(jsonData.ToString()).ToString();
        logger.LogInformation("The data: {@Json}", data);
        return Ok();
    }
}
