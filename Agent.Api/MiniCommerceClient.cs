namespace Agent.Api.Tools;

// Talks to MiniCommerce.Web over HTTP; Agent.Api has no direct DB access.
public class MiniCommerceClient(HttpClient httpClient)
{
    public async Task<IReadOnlyList<ProductDto>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        var products = await httpClient.GetFromJsonAsync<List<ProductDto>>("/products", cancellationToken);
        return products ?? [];
    }
}
