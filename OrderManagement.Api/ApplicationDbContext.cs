using Microsoft.EntityFrameworkCore;
using OrderManagement.Api.Models;

namespace OrderManagement.Api;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(order =>
        {
            order.HasKey(o => o.OrderId);
            order.OwnsMany(o => o.LineItems, lineItems =>
            {
                lineItems.WithOwner().HasForeignKey("OrderId");
                lineItems.Property<int>("LineItemId");
                lineItems.HasKey("LineItemId");
            });
        });
    }
}
