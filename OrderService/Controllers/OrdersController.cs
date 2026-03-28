using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using OrderService.Services;
using System.Text.Json;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly RabbitMQPublisher _publisher;

        public OrdersController()
        {
            // Use hostname and queue name
            _publisher = new RabbitMQPublisher("localhost", "orders-queue");
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            // This service currently publishes orders to RabbitMQ and does not persist them.
            return Ok(new
            {
                Message = "OrderService is reachable.",
                Data = Array.Empty<Order>()
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            // Serialize the order object to JSON
            var message = JsonSerializer.Serialize(order);

            // Publish asynchronously
            await _publisher.PublishAsync(message);

            return Ok("Order created and event published.");
        }
    }
}