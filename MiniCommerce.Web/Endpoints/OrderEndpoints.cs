using MiniCommerce.Web.Data.Entities;
using MiniCommerce.Web.Repositories;

namespace MiniCommerce.Web.Endpoints;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/orders");

        group.MapGet("/", async (IOrderRepository orderRepository, CancellationToken cancellationToken) =>
        {
            var orders = await orderRepository.GetOrders(cancellationToken);
            return Results.Ok(orders);
        });

        group.MapGet("/{orderId:int}", async (
            int orderId,
            IOrderRepository orderRepository,
            CancellationToken cancellationToken) =>
        {
            var order = await orderRepository.GetOrderById(orderId, cancellationToken);
            return order is null ? Results.NotFound() : Results.Ok(order);
        });

        group.MapPost("/", async (
            Order order,
            IOrderRepository orderRepository,
            CancellationToken cancellationToken) =>
        {
            var orderId = await orderRepository.CreateOrder(order, cancellationToken);
            return Results.Created($"/orders/{orderId}", order);
        });

        group.MapPut("/{orderId:int}", async (
            int orderId,
            Order order,
            IOrderRepository orderRepository,
            CancellationToken cancellationToken) =>
        {
            order.OrderId = orderId;
            var updated = await orderRepository.UpdateOrder(order, cancellationToken);
            return updated ? Results.NoContent() : Results.NotFound();
        });

        group.MapDelete("/{orderId:int}", async (
            int orderId,
            IOrderRepository orderRepository,
            CancellationToken cancellationToken) =>
        {
            var deleted = await orderRepository.DeleteOrder(orderId, cancellationToken);
            return deleted ? Results.NoContent() : Results.NotFound();
        });

        return app;
    }
}
