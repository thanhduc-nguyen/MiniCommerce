using Orders.Api.Models;

namespace Orders.Api.Repositories;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetOrders(CancellationToken cancellationToken);
    Task<Order?> GetOrderById(int orderId, CancellationToken cancellationToken);
    Task<int> CreateOrder(Order order, CancellationToken cancellationToken);
    Task<bool> UpdateOrder(Order order, CancellationToken cancellationToken);
    Task<bool> DeleteOrder(int orderId, CancellationToken cancellationToken);
}
