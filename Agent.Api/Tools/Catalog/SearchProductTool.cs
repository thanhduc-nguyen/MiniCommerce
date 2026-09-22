using System.Text.Json;

namespace Agent.Api.Tools.Catalog;

public class SearchProductTool(MiniCommerceClient miniCommerceClient) : ITool
{
    public string Name => "search_products";
    public string Description => "Searches the MiniCommerce product catalog by name, description, or category. Input: a search term such as \"shoes\".";

    public async Task<string> InvokeAsync(string input, CancellationToken cancellationToken = default)
    {
        var products = await miniCommerceClient.GetProductsAsync(cancellationToken);

        var matches = string.IsNullOrWhiteSpace(input)
            ? products
            : products
                .Where(p =>
                    p.ProductName.Contains(input, StringComparison.OrdinalIgnoreCase) ||
                    p.Description.Contains(input, StringComparison.OrdinalIgnoreCase) ||
                    p.CategoryName.Contains(input, StringComparison.OrdinalIgnoreCase))
                .ToList();

        // Return text the model can read back.
        return JsonSerializer.Serialize(matches);
    }
}
