using StoreFront.Web.Models.Checkout;

namespace StoreFront.Web.Services.Orders;

public class OrderService(HttpClient httpClient) : IOrderService
{
    public async Task<HttpResponseMessage> CreateOrder(CheckoutModel model, CancellationToken cancellationToken)
    {
        // Send the checkout model to the Order Management API endpoint for processing
        // TODO: use message broker
        return await httpClient.PostAsJsonAsync("orders", model, cancellationToken)
                ?? throw new InvalidOperationException("Failed to create order.");
    }
}
