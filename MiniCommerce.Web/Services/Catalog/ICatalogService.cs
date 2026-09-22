using MiniCommerce.Web.Models.Catalog;

namespace MiniCommerce.Web.Services.Catalog;

public interface ICatalogService
{
    Task<IEnumerable<ProductModel>> GetProducts(CancellationToken cancellationToken);
    Task<IEnumerable<ProductModel>> GetProductsByCategory(string categoryName, CancellationToken cancellationToken);
}
