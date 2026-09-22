using MiniCommerce.Web.Models.Orders;

namespace MiniCommerce.Web.Services.Orders;

public interface IOrderService
{
    Task<int> CreateOrder(OrderModel model, CancellationToken cancellationToken);
}
