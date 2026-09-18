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
                UserGuid = Guid.Parse("40e6ce14-f560-40b1-82ae-c1d9ec1015ff"),
                ProductGuid = Guid.Parse("28d65b5d-f522-448c-8ce5-203e4efe2c17"),
                PurchasedPrice = 15.95m,
                Quantity = 1,
                TotalPrice = 15.95m,
                CreatedAt = new DateTime(2026, 9, 1, 10, 15, 0, DateTimeKind.Utc)
            },
            new Order
            {
                OrderGuid = Guid.NewGuid(),
                UserGuid = Guid.Parse("50e6ce14-f560-40b1-82ae-c1d9ec1015ff"),
                ProductGuid = Guid.Parse("a8814752-c866-4944-8020-61279d380087"),
                PurchasedPrice = 10.95m,
                Quantity = 2,
                TotalPrice = 21.90m,
                CreatedAt = new DateTime(2026, 9, 5, 14, 30, 0, DateTimeKind.Utc)
            },
            new Order
            {
                OrderGuid = Guid.NewGuid(),
                UserGuid = Guid.Parse("60e6ce14-f560-40b1-82ae-c1d9ec1015ff"),
                ProductGuid = Guid.Parse("51149bd0-c9e0-4068-b8f2-3368caa5c85c"),
                PurchasedPrice = 5.685m,
                Quantity = 1,
                TotalPrice = 5.685m,
                CreatedAt = new DateTime(2026, 9, 10, 9, 45, 0, DateTimeKind.Utc)
            },
            new Order
            {
                OrderGuid = Guid.NewGuid(),
                UserGuid = Guid.Parse("40e6ce14-f560-40b1-82ae-c1d9ec1015ff"),
                ProductGuid = Guid.Parse("4ae642de-3ccf-4bca-8c24-14ae508a759f"),
                PurchasedPrice = 3.465m,
                Quantity = 3,
                TotalPrice = 10.395m,
                CreatedAt = new DateTime(2026, 9, 15, 16, 20, 0, DateTimeKind.Utc)
            },
            new Order
            {
                OrderGuid = Guid.NewGuid(),
                UserGuid = Guid.Parse("50e6ce14-f560-40b1-82ae-c1d9ec1015ff"),
                ProductGuid = Guid.Parse("5df4c8d9-77ba-469a-8a23-5b87f9f7d9c5"),
                PurchasedPrice = 14.2125m,
                Quantity = 1,
                TotalPrice = 14.2125m,
                CreatedAt = new DateTime(2026, 9, 18, 11, 10, 0, DateTimeKind.Utc)
            },
            new Order
            {
                OrderGuid = Guid.NewGuid(),
                UserGuid = Guid.Parse("60e6ce14-f560-40b1-82ae-c1d9ec1015ff"),
                ProductGuid = Guid.Parse("293d91cd-0de8-4b18-b2e9-a1a26678dada"),
                PurchasedPrice = 8.975m,
                Quantity = 2,
                TotalPrice = 17.95m,
                CreatedAt = new DateTime(2026, 9, 20, 13, 5, 0, DateTimeKind.Utc)
            },
            new Order
            {
                OrderGuid = Guid.NewGuid(),
                UserGuid = Guid.Parse("40e6ce14-f560-40b1-82ae-c1d9ec1015ff"),
                ProductGuid = Guid.Parse("ad237f2f-94ce-46f5-91e5-950d328922a3"),
                PurchasedPrice = 18.95m,
                Quantity = 1,
                TotalPrice = 18.95m,
                CreatedAt = new DateTime(2026, 9, 22, 8, 40, 0, DateTimeKind.Utc)
            },
            new Order
            {
                OrderGuid = Guid.NewGuid(),
                UserGuid = Guid.Parse("50e6ce14-f560-40b1-82ae-c1d9ec1015ff"),
                ProductGuid = Guid.Parse("03eb5b7c-3e42-4361-8210-3cb6ce0cf88f"),
                PurchasedPrice = 10.95m,
                Quantity = 3,
                TotalPrice = 32.85m,
                CreatedAt = new DateTime(2026, 9, 24, 15, 25, 0, DateTimeKind.Utc)
            },
            new Order
            {
                OrderGuid = Guid.NewGuid(),
                UserGuid = Guid.Parse("60e6ce14-f560-40b1-82ae-c1d9ec1015ff"),
                ProductGuid = Guid.Parse("90e3af6d-e79a-45b6-b2c8-8bc25f60708a"),
                PurchasedPrice = 2.9625m,
                Quantity = 2,
                TotalPrice = 5.925m,
                CreatedAt = new DateTime(2026, 9, 26, 10, 0, 0, DateTimeKind.Utc)
            },
            new Order
            {
                OrderGuid = Guid.NewGuid(),
                UserGuid = Guid.Parse("40e6ce14-f560-40b1-82ae-c1d9ec1015ff"),
                ProductGuid = Guid.Parse("324a1f34-97f9-4636-be93-ec456b19f90e"),
                PurchasedPrice = 1.975m,
                Quantity = 4,
                TotalPrice = 7.90m,
                CreatedAt = new DateTime(2026, 9, 28, 17, 45, 0, DateTimeKind.Utc)
            });

        await context.SaveChangesAsync();
    }
}
