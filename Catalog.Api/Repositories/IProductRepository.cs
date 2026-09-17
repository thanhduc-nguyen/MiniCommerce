using Catalog.Api.Models;

namespace Catalog.Api.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProducts(CancellationToken cancellationToken);
    Task<Product> GetProductById(int productId, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetProductsByCategory(string categoryName, CancellationToken cancellationToken);
    Task<int> CreateProduct(Product product, CancellationToken cancellationToken);
    Task<bool> UpdateProduct(Product product, CancellationToken cancellationToken);
    Task<bool> DeleteProduct(int productId, CancellationToken cancellationToken);
}
