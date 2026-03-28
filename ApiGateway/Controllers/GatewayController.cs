using Microsoft.AspNetCore.Mvc;

[ApiController]
public class GatewayController : ControllerBase
{
    [HttpGet("/gateway/orders")]
    public IActionResult GetOrders() => Ok();

    [HttpPost("/gateway/orders")]
    public IActionResult CreateOrder([FromBody] object body) => Ok();
}