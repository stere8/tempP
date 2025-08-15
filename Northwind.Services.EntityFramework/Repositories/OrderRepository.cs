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
            throw new ArgumentOutOfRangeException(nameof(skip));
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count));

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
        // Implementation to convert Entity Order to Repository Order
        // This involves mapping all properties and related objects
    }

    private Entities.Order MapRepositoryToEntity(RepositoryOrder repositoryOrder)
    {
        // Implementation to convert Repository Order to Entity Order
    }

    private void UpdateEntityFromRepository(Entities.Order entityOrder, RepositoryOrder repositoryOrder)
    {
        // Implementation to update entity properties from repository model
    }
}
