using Microsoft.EntityFrameworkCore;
using MiniCommerce.Web.Data;
using MiniCommerce.Web.Data.Entities;

namespace MiniCommerce.Web.Repositories;

public class ProductRepository(AppDbContext context) : IProductRepository
{
    public async Task<IEnumerable<Product>> GetProducts(CancellationToken cancellationToken)
    {
        return await context.Products
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetProductById(int productId, CancellationToken cancellationToken)
    {
        return await context.Products
            .AsNoTracking()
            .SingleOrDefaultAsync(product => product.ProductId == productId, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetProductsByCategory(string categoryName, CancellationToken cancellationToken)
    {
        return await context.Products
            .AsNoTracking()
            .Where(product => product.CategoryName == categoryName)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CreateProduct(Product product, CancellationToken cancellationToken)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync(cancellationToken);

        return product.ProductId;
    }

    public async Task<bool> UpdateProduct(Product product, CancellationToken cancellationToken)
    {
        var existingProduct = await context.Products
            .SingleOrDefaultAsync(existing => existing.ProductId == product.ProductId, cancellationToken);

        if (existingProduct is null)
        {
            return false;
        }

        existingProduct.ProductGuid = product.ProductGuid;
        existingProduct.ProductName = product.ProductName;
        existingProduct.Description = product.Description;
        existingProduct.ImageUrl = product.ImageUrl;
        existingProduct.Price = product.Price;
        existingProduct.DiscountRate = product.DiscountRate;
        existingProduct.CategoryName = product.CategoryName;
        existingProduct.Stock = product.Stock;

        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteProduct(int productId, CancellationToken cancellationToken)
    {
        var product = await context.Products
            .SingleOrDefaultAsync(existing => existing.ProductId == productId, cancellationToken);

        if (product is null)
        {
            return false;
        }

        context.Products.Remove(product);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
