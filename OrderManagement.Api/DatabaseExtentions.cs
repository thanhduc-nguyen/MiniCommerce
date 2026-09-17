using Microsoft.EntityFrameworkCore;
using OrderManagement.Api.Models;

namespace OrderManagement.Api;

public static class DatabaseExtentions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.EnsureCreated();

        if (await context.Orders.AnyAsync())
        {
            return;
        }

        await context.Orders.AddRangeAsync(
            new Order
            {
                OrderGuid = Guid.NewGuid(),
                LineItems =
                [
                    new LineItem
                    {
                        ProductGuid = Guid.Parse("28d65b5d-f522-448c-8ce5-203e4efe2c17"),
                        ProductName = "Kudu Purple Sweatshirt",
                        Quantity = 1,
                        Price = 15.95m
                    },
                    new LineItem
                    {
                        ProductGuid = Guid.Parse("a8814752-c866-4944-8020-61279d380087"),
                        ProductName = "Cup<T> White Mug",
                        Quantity = 2,
                        Price = 10.95m
                    }
                ],
                TotalPrice = 37.85m
            },
            new Order
            {
                OrderGuid = Guid.NewGuid(),
                LineItems =
                [
                    new LineItem
                    {
                        ProductGuid = Guid.Parse("51149bd0-c9e0-4068-b8f2-3368caa5c85c"),
                        ProductName = ".NET Blue Sweatshirt",
                        Quantity = 1,
                        Price = 18.95m
                    },
                    new LineItem
                    {
                        ProductGuid = Guid.Parse("4ae642de-3ccf-4bca-8c24-14ae508a759f"),
                        ProductName = "Cup<T> Badge",
                        Quantity = 3,
                        Price = 4.95m
                    }
                ],
                TotalPrice = 33.80m
            });

        await context.SaveChangesAsync();
    }
}
