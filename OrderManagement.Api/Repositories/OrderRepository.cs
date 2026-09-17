using Microsoft.EntityFrameworkCore;
using OrderManagement.Api.Models;

namespace OrderManagement.Api.Repositories;

public class OrderRepository(ApplicationDbContext context) : IOrderRepository
{
    public async Task<IEnumerable<Order>> GetOrders(CancellationToken cancellationToken)
    {
        return await context.Orders
            .AsNoTracking()
            .Include(order => order.LineItems)
            .ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetOrderById(int orderId, CancellationToken cancellationToken)
    {
        return await context.Orders
            .AsNoTracking()
            .Include(order => order.LineItems)
            .SingleOrDefaultAsync(order => order.OrderId == orderId, cancellationToken);
    }

    public async Task<int> CreateOrder(Order order, CancellationToken cancellationToken)
    {
        if (order.OrderGuid == Guid.Empty)
        {
            order.OrderGuid = Guid.NewGuid();
        }

        order.TotalPrice = CalculateTotal(order);
        context.Orders.Add(order);
        await context.SaveChangesAsync(cancellationToken);

        return order.OrderId;
    }

    public async Task<bool> UpdateOrder(Order order, CancellationToken cancellationToken)
    {
        var existingOrder = await context.Orders
            .Include(existing => existing.LineItems)
            .SingleOrDefaultAsync(existing => existing.OrderId == order.OrderId, cancellationToken);

        if (existingOrder is null)
        {
            return false;
        }

        existingOrder.OrderGuid = order.OrderGuid;
        existingOrder.TotalPrice = CalculateTotal(order);
        existingOrder.LineItems.Clear();
        existingOrder.LineItems.AddRange(order.LineItems);

        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteOrder(int orderId, CancellationToken cancellationToken)
    {
        var order = await context.Orders
            .SingleOrDefaultAsync(existing => existing.OrderId == orderId, cancellationToken);

        if (order is null)
        {
            return false;
        }

        context.Orders.Remove(order);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static decimal CalculateTotal(Order order)
    {
        return order.LineItems.Sum(lineItem => lineItem.Price * lineItem.Quantity);
    }
}
