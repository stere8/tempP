using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Repositories;
using ModelsAddOrder = Northwind.Orders.WebApi.Models.AddOrder;
using ModelsBriefOrder = Northwind.Orders.WebApi.Models.BriefOrder;
using ModelsFullOrder = Northwind.Orders.WebApi.Models.FullOrder;

namespace Northwind.Orders.WebApi.Controllers;

public sealed class OrdersController : ControllerBase
{
    public OrdersController(IOrderRepository orderRepository, ILogger<OrdersController> logger)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult<ModelsFullOrder>> GetOrderAsync(long orderId)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult<IEnumerable<ModelsBriefOrder>>> GetOrdersAsync(int? skip, int? count)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult<ModelsAddOrder>> AddOrderAsync(ModelsBriefOrder order)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult> RemoveOrderAsync(long orderId)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult> UpdateOrderAsync(long orderId, ModelsBriefOrder order)
    {
        throw new NotImplementedException();
    }
}
