using StoreFront.Web.Models.Catalog;

namespace StoreFront.Web.Services.Catalog;

public class CatalogService(HttpClient httpClient) : ICatalogService
{
    public async Task<IEnumerable<ProductModel>> GetProducts(CancellationToken cancellationToken)
    {
        return await httpClient.GetFromJsonAsync<IEnumerable<ProductModel>>("products", cancellationToken) ?? [];
    }

    public async Task<IEnumerable<ProductModel>> GetProductsByCategory(string categoryName, CancellationToken cancellationToken)
    {
        return await httpClient.GetFromJsonAsync<IEnumerable<ProductModel>>($"products/category/{categoryName}", cancellationToken) ?? [];
    }
}
