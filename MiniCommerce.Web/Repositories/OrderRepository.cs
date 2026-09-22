using Microsoft.EntityFrameworkCore;
using MiniCommerce.Web.Data;
using MiniCommerce.Web.Data.Entities;

namespace MiniCommerce.Web.Repositories;

public class OrderRepository(AppDbContext context) : IOrderRepository
{
    public async Task<IEnumerable<Order>> GetOrders(CancellationToken cancellationToken)
    {
        return await context.Orders
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetOrderById(int orderId, CancellationToken cancellationToken)
    {
        return await context.Orders
            .AsNoTracking()
            .SingleOrDefaultAsync(order => order.OrderId == orderId, cancellationToken);
    }

    public async Task<int> CreateOrder(Order order, CancellationToken cancellationToken)
    {
        context.Orders.Add(order);
        await context.SaveChangesAsync(cancellationToken);

        return order.OrderId;
    }

    public async Task<bool> UpdateOrder(Order order, CancellationToken cancellationToken)
    {
        var existingOrder = await context.Orders
            .SingleOrDefaultAsync(existing => existing.OrderId == order.OrderId, cancellationToken);

        if (existingOrder is null)
        {
            return false;
        }

        existingOrder.OrderGuid = order.OrderGuid;
        existingOrder.ProductGuid = order.ProductGuid;
        existingOrder.PurchasedPrice = order.PurchasedPrice;
        existingOrder.Quantity = order.Quantity;
        existingOrder.TotalPrice = CalculateTotal(order);

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
        return order.PurchasedPrice * order.Quantity;
    }
}
