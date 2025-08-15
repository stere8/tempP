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
            var result = new ModelsAddOrder { OrderId = orderId }; // or whatever the property is called
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

    Copy code
private ModelsFullOrder MapToFullOrder(RepositoryOrder order)
    {
        return new ModelsFullOrder
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            EmployeeId = order.EmployeeId,
            OrderDate = order.OrderDate,
            RequiredDate = order.RequiredDate,
            ShippedDate = order.ShippedDate,
            Freight = order.Freight,
            Customer = new ModelsCustomer
            {
                Id = order.Customer?.Id ?? string.Empty,
                CompanyName = order.Customer?.CompanyName ?? string.Empty
            },
            Employee = new ModelsEmployee
            {
                Id = order.Employee?.Id ?? 0,
                FirstName = order.Employee?.FirstName ?? string.Empty,
                LastName = order.Employee?.LastName ?? string.Empty
            },
            Shipper = new ModelsShipper
            {
                Id = order.Shipper?.Id ?? 0,
                CompanyName = order.Shipper?.CompanyName ?? string.Empty
            },
            ShippingAddress = new ModelsShippingAddress
            {
                ShipName = order.ShippingAddress?.ShipName ?? string.Empty,
                ShipAddress = order.ShippingAddress?.ShipAddress ?? string.Empty,
                ShipCity = order.ShippingAddress?.ShipCity ?? string.Empty,
                ShipRegion = order.ShippingAddress?.ShipRegion,
                ShipPostalCode = order.ShippingAddress?.ShipPostalCode ?? string.Empty,
                ShipCountry = order.ShippingAddress?.ShipCountry ?? string.Empty
            },
            OrderDetails = order.OrderDetails.Select(od => new ModelsFullOrderDetail
            {
                ProductId = od.ProductId,
                UnitPrice = od.UnitPrice,
                Quantity = od.Quantity,
                Discount = od.Discount
            }).ToList()
        };
    }

    private RepositoryOrder MapFromBriefOrder(ModelsBriefOrder briefOrder)
    {
        return new RepositoryOrder(briefOrder.Id)
        {
            CustomerId = briefOrder.CustomerId,
            EmployeeId = briefOrder.EmployeeId,
            OrderDate = briefOrder.OrderDate,
            RequiredDate = briefOrder.RequiredDate,
            ShippedDate = briefOrder.ShippedDate,
            ShipperId = briefOrder.ShippingAddress != null ? 1 : 1, // You'll need to determine the shipper ID logic
            Freight = briefOrder.Freight,
            ShippingAddress = briefOrder.ShippingAddress != null ? new ShippingAddress
            {
                ShipName = briefOrder.ShippingAddress.ShipName,
                ShipAddress = briefOrder.ShippingAddress.ShipAddress,
                ShipCity = briefOrder.ShippingAddress.ShipCity,
                ShipRegion = briefOrder.ShippingAddress.ShipRegion,
                ShipPostalCode = briefOrder.ShippingAddress.ShipPostalCode,
                ShipCountry = briefOrder.ShippingAddress.ShipCountry
            } : new ShippingAddress(),
            OrderDetails = briefOrder.OrderDetails?.Select(od => new RepositoryOrderDetail
            {
                ProductId = od.ProductId,
                UnitPrice = od.UnitPrice,
                Quantity = od.Quantity,
                Discount = od.Discount
            }).ToList() ?? new List<RepositoryOrderDetail>()
        };
    }


    private Northwind.Services.Repositories.Order MapFromBriefOrder(ModelsBriefOrder briefOrder)
    {
        // Implementation
    }
}
