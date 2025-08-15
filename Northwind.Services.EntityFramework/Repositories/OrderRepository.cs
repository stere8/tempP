using Microsoft.EntityFrameworkCore;
using Northwind.Services.EntityFramework.Entities;
using Northwind.Services.Repositories;
using RepositoryOrder = Northwind.Services.Repositories.Order;

namespace Northwind.Services.EntityFramework.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    private readonly NorthwindContext _context;

    public OrderRepository(NorthwindContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<RepositoryOrder> GetOrderAsync(long orderId)
    {
        var entityOrder = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Employee)
            .Include(o => o.Shipper)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);

        if (entityOrder == null)
        {
            throw new OrderNotFoundException($"Order with ID {orderId} not found.");
        }

        return MapEntityToRepository(entityOrder);
    }

    public async Task<IList<RepositoryOrder>> GetOrdersAsync(int skip, int count)
    {
        if (skip < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(skip));
        }

        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        var entityOrders = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Employee)
            .Include(o => o.Shipper)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
            .OrderBy(o => o.OrderId)
            .Skip(skip)
            .Take(count)
            .ToListAsync();

        return entityOrders.Select(MapEntityToRepository).ToList();
    }

    public async Task<long> AddOrderAsync(RepositoryOrder order)
    {
        try
        {
            var entityOrder = MapRepositoryToEntity(order);
            _context.Orders.Add(entityOrder);
            await _context.SaveChangesAsync();
            return entityOrder.OrderId;
        }
        catch (Exception ex)
        {
            throw new RepositoryException("Failed to add order", ex);
        }
    }

    public async Task RemoveOrderAsync(long orderId)
    {
        var entityOrder = await _context.Orders
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);

        if (entityOrder == null)
        {
            throw new OrderNotFoundException($"Order with ID {orderId} not found.");
        }

        _context.Orders.Remove(entityOrder);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateOrderAsync(RepositoryOrder order)
    {
        var existingOrder = await _context.Orders
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.OrderId == order.Id);

        if (existingOrder == null)
        {
            throw new OrderNotFoundException($"Order with ID {order.Id} not found.");
        }

        UpdateEntityFromRepository(existingOrder, order);
        await _context.SaveChangesAsync();
    }

    // Helper methods for mapping between entity and repository models
    private RepositoryOrder MapEntityToRepository(Entities.Order entityOrder)
    {
        return new RepositoryOrder(entityOrder.OrderId)
        {
            CustomerId = entityOrder.CustomerId,
            EmployeeId = entityOrder.EmployeeId,
            OrderDate = entityOrder.OrderDate,
            RequiredDate = entityOrder.RequiredDate,
            ShippedDate = entityOrder.ShippedDate,
            ShipperId = entityOrder.ShipperId,
            Freight = entityOrder.Freight,
            ShippingAddress = new ShippingAddress
            {
                ShipName = entityOrder.ShipName,
                ShipAddress = entityOrder.ShipAddress,
                ShipCity = entityOrder.ShipCity,
                ShipRegion = entityOrder.ShipRegion,
                ShipPostalCode = entityOrder.ShipPostalCode,
                ShipCountry = entityOrder.ShipCountry
            },
            OrderDetails = entityOrder.OrderDetails.Select(od => new RepositoryOrderDetail
            {
                ProductId = od.ProductId,
                UnitPrice = od.UnitPrice,
                Quantity = od.Quantity,
                Discount = od.Discount
            }).ToList()
        };
    }

    private Entities.Order MapRepositoryToEntity(RepositoryOrder repositoryOrder)
    {
        return new Entities.Order
        {
            OrderId = repositoryOrder.Id,
            CustomerId = repositoryOrder.CustomerId,
            EmployeeId = repositoryOrder.EmployeeId,
            OrderDate = repositoryOrder.OrderDate,
            RequiredDate = repositoryOrder.RequiredDate,
            ShippedDate = repositoryOrder.ShippedDate,
            ShipperId = repositoryOrder.ShipperId,
            Freight = repositoryOrder.Freight,
            ShipName = repositoryOrder.ShippingAddress?.ShipName ?? string.Empty,
            ShipAddress = repositoryOrder.ShippingAddress?.ShipAddress ?? string.Empty,
            ShipCity = repositoryOrder.ShippingAddress?.ShipCity ?? string.Empty,
            ShipRegion = repositoryOrder.ShippingAddress?.ShipRegion,
            ShipPostalCode = repositoryOrder.ShippingAddress?.ShipPostalCode ?? string.Empty,
            ShipCountry = repositoryOrder.ShippingAddress?.ShipCountry ?? string.Empty,
            OrderDetails = repositoryOrder.OrderDetails.Select(od => new Entities.OrderDetail
            {
                OrderId = repositoryOrder.Id,
                ProductId = od.ProductId,
                UnitPrice = od.UnitPrice,
                Quantity = od.Quantity,
                Discount = od.Discount
            }).ToList()
        };
    }

    private void UpdateEntityFromRepository(Entities.Order entityOrder, RepositoryOrder repositoryOrder)
    {
        entityOrder.CustomerId = repositoryOrder.CustomerId;
        entityOrder.EmployeeId = repositoryOrder.EmployeeId;
        entityOrder.OrderDate = repositoryOrder.OrderDate;
        entityOrder.RequiredDate = repositoryOrder.RequiredDate;
        entityOrder.ShippedDate = repositoryOrder.ShippedDate;
        entityOrder.ShipperId = repositoryOrder.ShipperId;
        entityOrder.Freight = repositoryOrder.Freight;
        entityOrder.ShipName = repositoryOrder.ShippingAddress?.ShipName ?? string.Empty;
        entityOrder.ShipAddress = repositoryOrder.ShippingAddress?.ShipAddress ?? string.Empty;
        entityOrder.ShipCity = repositoryOrder.ShippingAddress?.ShipCity ?? string.Empty;
        entityOrder.ShipRegion = repositoryOrder.ShippingAddress?.ShipRegion;
        entityOrder.ShipPostalCode = repositoryOrder.ShippingAddress?.ShipPostalCode ?? string.Empty;
        entityOrder.ShipCountry = repositoryOrder.ShippingAddress?.ShipCountry ?? string.Empty;

        // Update order details
        entityOrder.OrderDetails.Clear();
        foreach (var detail in repositoryOrder.OrderDetails)
        {
            entityOrder.OrderDetails.Add(new Entities.OrderDetail
            {
                OrderId = repositoryOrder.Id,
                ProductId = detail.ProductId,
                UnitPrice = detail.UnitPrice,
                Quantity = detail.Quantity,
                Discount = detail.Discount
            });
        }
    }
}
