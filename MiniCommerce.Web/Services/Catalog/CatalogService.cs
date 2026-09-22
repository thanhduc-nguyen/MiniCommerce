using MiniCommerce.Web.Data.Entities;
using MiniCommerce.Web.Models.Catalog;
using MiniCommerce.Web.Repositories;

namespace MiniCommerce.Web.Services.Catalog;

public class CatalogService(IProductRepository productRepository) : ICatalogService
{
    public async Task<IEnumerable<ProductModel>> GetProducts(CancellationToken cancellationToken)
    {
        var products = await productRepository.GetProducts(cancellationToken);
        return products.Select(MapToModel);
    }

    public async Task<IEnumerable<ProductModel>> GetProductsByCategory(string categoryName, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetProductsByCategory(categoryName, cancellationToken);
        return products.Select(MapToModel);
    }

    private static ProductModel MapToModel(Product product) => new()
    {
        ProductId = product.ProductId,
        ProductGuid = product.ProductGuid,
        ProductName = product.ProductName,
        Description = product.Description,
        ImageUrl = product.ImageUrl,
        Price = product.Price,
        DiscountRate = product.DiscountRate,
        CategoryName = product.CategoryName,
        Stock = product.Stock
    };
}
