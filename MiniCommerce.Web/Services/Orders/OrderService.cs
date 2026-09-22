using MiniCommerce.Web.Data.Entities;
using MiniCommerce.Web.Models.Orders;
using MiniCommerce.Web.Repositories;

namespace MiniCommerce.Web.Services.Orders;

public class OrderService(IOrderRepository orderRepository) : IOrderService
{
    public async Task<int> CreateOrder(OrderModel model, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            OrderGuid = model.OrderGuid,
            UserGuid = model.UserGuid,
            ProductGuid = model.ProductGuid,
            PurchasedPrice = model.PurchasedPrice,
            Quantity = model.Quantity,
            TotalPrice = model.TotalPrice,
            CreatedAt = model.CreatedAt
        };

        return await orderRepository.CreateOrder(order, cancellationToken);
    }
}
