using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Repositories;
using ModelsAddOrder = Northwind.Orders.WebApi.Models.AddOrder;
using ModelsBriefOrder = Northwind.Orders.WebApi.Models.BriefOrder;
using ModelsFullOrder = Northwind.Orders.WebApi.Models.FullOrder;

namespace Northwind.Orders.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrderRepository orderRepository, ILogger<OrdersController> logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("{orderId}")]
    public async Task<ActionResult<ModelsFullOrder>> GetOrderAsync(long orderId)
    {
        try
        {
            _logger.LogInformation("Getting order with ID: {OrderId}", orderId);
            var order = await _orderRepository.GetOrderAsync(orderId);
            var result = MapToFullOrder(order);
            return Ok(result);
        }
        catch (OrderNotFoundException)
        {
            _logger.LogWarning("Order with ID {OrderId} not found", orderId);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting order with ID: {OrderId}", orderId);
            return StatusCode(500);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ModelsBriefOrder>>> GetOrdersAsync(
        [FromQuery] int? skip = 0,
        [FromQuery] int? count = 10)
    {
        try
        {
            _logger.LogInformation("Getting orders with skip: {Skip}, count: {Count}", skip, count);
            var orders = await _orderRepository.GetOrdersAsync(skip ?? 0, count ?? 10);
            var result = orders.Select(MapToBriefOrder);
            return Ok(result);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogWarning("Invalid parameters: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting orders");
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<ActionResult<ModelsAddOrder>> AddOrderAsync([FromBody] ModelsBriefOrder order)
    {
        try
        {
            _logger.LogInformation("Adding new order");
            var repositoryOrder = MapFromBriefOrder(order);
            var orderId = await _orderRepository.AddOrderAsync(repositoryOrder);
            var result = new ModelsAddOrder { Id = orderId };
            return Ok(result);
        }
        catch (RepositoryException ex)
        {
            _logger.LogError(ex, "Error adding order");
            return StatusCode(500);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error adding order");
            return StatusCode(500);
        }
    }

    [HttpDelete("{orderId}")]
    public async Task<ActionResult> RemoveOrderAsync(long orderId)
    {
        try
        {
            _logger.LogInformation("Removing order with ID: {OrderId}", orderId);
            await _orderRepository.RemoveOrderAsync(orderId);
            return NoContent();
        }
        catch (OrderNotFoundException)
        {
            _logger.LogWarning("Order with ID {OrderId} not found for removal", orderId);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing order with ID: {OrderId}", orderId);
            return StatusCode(500);
        }
    }

    [HttpPut("{orderId}")]
    public async Task<ActionResult> UpdateOrderAsync(long orderId, [FromBody] ModelsBriefOrder order)
    {
        try
        {
            _logger.LogInformation("Updating order with ID: {OrderId}", orderId);
            var repositoryOrder = MapFromBriefOrder(order);
            repositoryOrder = new Northwind.Services.Repositories.Order(orderId)
            {
                // Set properties from order
            };
            await _orderRepository.UpdateOrderAsync(repositoryOrder);
            return NoContent();
        }
        catch (OrderNotFoundException)
        {
            _logger.LogWarning("Order with ID {OrderId} not found for update", orderId);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating order with ID: {OrderId}", orderId);
            return StatusCode(500);
        }
    }

    // Helper methods for mapping between API models and repository models
    private ModelsFullOrder MapToFullOrder(Northwind.Services.Repositories.Order order)
    {
        // Implementation
    }

    private ModelsBriefOrder MapToBriefOrder(Northwind.Services.Repositories.Order order)
    {
        // Implementation
    }

    private Northwind.Services.Repositories.Order MapFromBriefOrder(ModelsBriefOrder briefOrder)
    {
        // Implementation
    }
}
