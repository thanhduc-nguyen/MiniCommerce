using StoreFront.Web.Models.Checkout;

namespace StoreFront.Web.Services.Orders;

public interface IOrderService
{
    Task<HttpResponseMessage> CreateOrder(CheckoutModel model, CancellationToken cancellationToken);
}
