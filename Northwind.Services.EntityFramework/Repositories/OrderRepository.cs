using Northwind.Services.EntityFramework.Entities;
using Northwind.Services.Repositories;
using RepositoryOrder = Northwind.Services.Repositories.Order;

namespace Northwind.Services.EntityFramework.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    public OrderRepository(NorthwindContext context)
    {
    }

    public Task<RepositoryOrder> GetOrderAsync(long orderId)
    {
        throw new NotImplementedException();
    }

    public Task<IList<RepositoryOrder>> GetOrdersAsync(int skip, int count)
    {
        throw new NotImplementedException();
    }

    public Task<long> AddOrderAsync(RepositoryOrder order)
    {
        throw new NotImplementedException();
    }

    public Task RemoveOrderAsync(long orderId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateOrderAsync(RepositoryOrder order)
    {
        throw new NotImplementedException();
    }
}
