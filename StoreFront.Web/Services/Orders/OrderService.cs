using StoreFront.Web.Models.Orders;

namespace StoreFront.Web.Services.Orders;

public class OrderService(HttpClient httpClient) : IOrderService
{
    public async Task<HttpResponseMessage> CreateOrder(OrderModel model, CancellationToken cancellationToken)
    {
        // TODO: use message broker
        return await httpClient.PostAsJsonAsync("orders", model, cancellationToken)
                ?? throw new InvalidOperationException("Failed to create order.");
    }
}
