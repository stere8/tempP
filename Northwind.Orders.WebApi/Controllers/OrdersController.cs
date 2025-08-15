using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Repositories;
using ModelsAddOrder = Northwind.Orders.WebApi.Models.AddOrder;
using ModelsBriefOrder = Northwind.Orders.WebApi.Models.BriefOrder;
using ModelsFullOrder = Northwind.Orders.WebApi.Models.FullOrder;
using ModelsCustomer = Northwind.Orders.WebApi.Models.Customer;
using ModelsEmployee = Northwind.Orders.WebApi.Models.Employee;
using ModelsShipper = Northwind.Orders.WebApi.Models.Shipper;
using ModelsShippingAddress = Northwind.Orders.WebApi.Models.ShippingAddress;
using ModelsBriefOrderDetail = Northwind.Orders.WebApi.Models.BriefOrderDetail;
using ModelsFullOrderDetail = Northwind.Orders.WebApi.Models.FullOrderDetail;

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
            // Set the ID to match the URL parameter
            var orderToUpdate = new Northwind.Services.Repositories.Order(orderId)
            {
                Customer = repositoryOrder.Customer,
                Employee = repositoryOrder.Employee,
                OrderDate = repositoryOrder.OrderDate,
                RequiredDate = repositoryOrder.RequiredDate,
                ShippedDate = repositoryOrder.ShippedDate,
                Shipper = repositoryOrder.Shipper,
                Freight = repositoryOrder.Freight,
                ShipName = repositoryOrder.ShipName,
                ShippingAddress = repositoryOrder.ShippingAddress
            };

            // Copy order details
            foreach (var detail in repositoryOrder.OrderDetails)
            {
                orderToUpdate.OrderDetails.Add(detail);
            }

            await _orderRepository.UpdateOrderAsync(orderToUpdate);
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
        return new ModelsFullOrder
        {
            Id = order.Id,
            Customer = new ModelsCustomer
            {
                Id = order.Customer.Id,
                CompanyName = order.Customer.CompanyName
            },
            Employee = new ModelsEmployee
            {
                Id = order.Employee.Id,
                FirstName = order.Employee.FirstName,
                LastName = order.Employee.LastName
            },
            OrderDate = order.OrderDate,
            RequiredDate = order.RequiredDate,
            ShippedDate = order.ShippedDate,
            Shipper = new ModelsShipper
            {
                Id = order.Shipper.Id,
                CompanyName = order.Shipper.CompanyName
            },
            Freight = order.Freight,
            ShipName = order.ShipName,
            ShippingAddress = new ModelsShippingAddress(
                order.ShippingAddress.Address,
                order.ShippingAddress.City,
                order.ShippingAddress.Region,
                order.ShippingAddress.PostalCode,
                order.ShippingAddress.Country
            ),
            OrderDetails = order.OrderDetails.Select(od => new ModelsFullOrderDetail
            {
                ProductId = od.Product.Id,
                UnitPrice = od.UnitPrice,
                Quantity = od.Quantity,
                Discount = od.Discount
            }).ToList()
        };
    }

    private ModelsBriefOrder MapToBriefOrder(Northwind.Services.Repositories.Order order)
    {
        return new ModelsBriefOrder
        {
            Id = order.Id,
            CustomerId = order.Customer.Id,
            EmployeeId = order.Employee.Id,
            OrderDate = order.OrderDate,
            RequiredDate = order.RequiredDate,
            ShippedDate = order.ShippedDate,
            ShipperId = order.Shipper.Id,
            Freight = order.Freight,
            ShipName = order.ShipName,
            ShipAddress = order.ShippingAddress.Address,
            ShipCity = order.ShippingAddress.City,
            ShipRegion = order.ShippingAddress.Region,
            ShipPostalCode = order.ShippingAddress.PostalCode,
            ShipCountry = order.ShippingAddress.Country,
            OrderDetails = order.OrderDetails.Select(od => new ModelsBriefOrderDetail
            {
                ProductId = od.Product.Id,
                UnitPrice = od.UnitPrice,
                Quantity = od.Quantity,
                Discount = od.Discount
            }).ToList()
        };
    }

    private Northwind.Services.Repositories.Order MapFromBriefOrder(ModelsBriefOrder briefOrder)
    {
        var order = new Northwind.Services.Repositories.Order(briefOrder.Id)
        {
            Customer = new Northwind.Services.Repositories.Customer(briefOrder.CustomerId),
            Employee = new Northwind.Services.Repositories.Employee(briefOrder.EmployeeId),
            OrderDate = briefOrder.OrderDate,
            RequiredDate = briefOrder.RequiredDate,
            ShippedDate = briefOrder.ShippedDate,
            Shipper = new Northwind.Services.Repositories.Shipper(briefOrder.ShipperId),
            Freight = briefOrder.Freight,
            ShipName = briefOrder.ShipName,
            ShippingAddress = new ShippingAddress(
                briefOrder.ShipAddress,
                briefOrder.ShipCity,
                briefOrder.ShipRegion,
                briefOrder.ShipPostalCode,
                briefOrder.ShipCountry
            )
        };

        // Add order details
        foreach (var detail in briefOrder.OrderDetails)
        {
            var orderDetail = new Northwind.Services.Repositories.OrderDetail(order)
            {
                Product = new Northwind.Services.Repositories.Product(detail.ProductId),
                UnitPrice = detail.UnitPrice,
                Quantity = detail.Quantity,
                Discount = detail.Discount
            };
            order.OrderDetails.Add(orderDetail);
        }

        return order;
    }
}
