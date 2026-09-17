using Microsoft.EntityFrameworkCore;
using OrderManagement.Api;
using OrderManagement.Api.Models;
using OrderManagement.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderDB")));
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGet("/orders", async (IOrderRepository orderRepository, CancellationToken cancellationToken) =>
{
    var orders = await orderRepository.GetOrders(cancellationToken);
    return Results.Ok(orders);
});

app.MapGet("/orders/{orderId:int}", async (
    int orderId,
    IOrderRepository orderRepository,
    CancellationToken cancellationToken) =>
{
    var order = await orderRepository.GetOrderById(orderId, cancellationToken);
    return order is null ? Results.NotFound() : Results.Ok(order);
});

app.MapPost("/orders", async (
    Order order,
    IOrderRepository orderRepository,
    CancellationToken cancellationToken) =>
{
    var orderId = await orderRepository.CreateOrder(order, cancellationToken);
    return Results.Created($"/orders/{orderId}", order);
});

app.MapPut("/orders/{orderId:int}", async (
    int orderId,
    Order order,
    IOrderRepository orderRepository,
    CancellationToken cancellationToken) =>
{
    order.OrderId = orderId;
    var updated = await orderRepository.UpdateOrder(order, cancellationToken);
    return updated ? Results.NoContent() : Results.NotFound();
});

app.MapDelete("/orders/{orderId:int}", async (
    int orderId,
    IOrderRepository orderRepository,
    CancellationToken cancellationToken) =>
{
    var deleted = await orderRepository.DeleteOrder(orderId, cancellationToken);
    return deleted ? Results.NoContent() : Results.NotFound();
});

if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}

app.Run();
