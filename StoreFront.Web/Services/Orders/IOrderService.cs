using StoreFront.Web.Models.Orders;

namespace StoreFront.Web.Services.Orders;

public interface IOrderService
{
    Task<HttpResponseMessage> CreateOrder(OrderModel model, CancellationToken cancellationToken);
}
