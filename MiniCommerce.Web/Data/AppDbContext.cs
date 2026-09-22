using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniCommerce.Web.Data.Entities;
using MiniCommerce.Web.Models.Account;

namespace MiniCommerce.Web.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(product =>
        {
            product.ToTable("Products");
            product.HasKey(p => p.ProductId);
            product.Property(p => p.ProductName).HasMaxLength(100).IsRequired();
            product.Property(p => p.Description).HasMaxLength(1000);
            product.Property(p => p.ImageUrl).HasMaxLength(500);
            product.Property(p => p.Price).HasColumnType("decimal(30, 5)");
            product.Property(p => p.DiscountRate).HasColumnType("decimal(5, 4)");
            product.Property(p => p.CategoryName).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Order>(order =>
        {
            order.ToTable("Orders");
            order.HasKey(o => o.OrderId);
            order.Property(o => o.PurchasedPrice).HasColumnType("decimal(30, 5)");
            order.Property(o => o.TotalPrice).HasColumnType("decimal(30, 5)");
            order.Property(o => o.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();
        });
    }
}
