using Microsoft.EntityFrameworkCore;
using Orders.Api.Models;

namespace Orders.Api;

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
                ProductGuid = Guid.Parse("28d65b5d-f522-448c-8ce5-203e4efe2c17"),
                PurchasedPrice = 15.95m,
                Quantity = 1,
                TotalPrice = 15.95m,
                CreatedAt = new DateTime(2026, 9, 1, 10, 15, 0, DateTimeKind.Utc)
            },
            new Order
            {
                OrderGuid = Guid.NewGuid(),
                ProductGuid = Guid.Parse("a8814752-c866-4944-8020-61279d380087"),
                PurchasedPrice = 10.95m,
                Quantity = 2,
                TotalPrice = 21.90m,
                CreatedAt = new DateTime(2026, 9, 5, 14, 30, 0, DateTimeKind.Utc)
            },
            new Order
            {
                OrderGuid = Guid.NewGuid(),
                ProductGuid = Guid.Parse("51149bd0-c9e0-4068-b8f2-3368caa5c85c"),
                PurchasedPrice = 5.685m,
                Quantity = 1,
                TotalPrice = 5.685m,
                CreatedAt = new DateTime(2026, 9, 10, 9, 45, 0, DateTimeKind.Utc)
            },
            new Order
            {
                OrderGuid = Guid.NewGuid(),
                ProductGuid = Guid.Parse("4ae642de-3ccf-4bca-8c24-14ae508a759f"),
                PurchasedPrice = 3.465m,
                Quantity = 3,
                TotalPrice = 10.395m,
                CreatedAt = new DateTime(2026, 9, 15, 16, 20, 0, DateTimeKind.Utc)
            });

        await context.SaveChangesAsync();
    }
}
