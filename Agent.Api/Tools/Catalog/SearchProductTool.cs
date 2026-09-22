using System.Text.Json;

namespace Agent.Api.Tools.Catalog;

public class SearchProductTool(MiniCommerceClient miniCommerceClient) : ITool
{
    public string Name => "search_products";
    public string Description =>
        "Searches the MiniCommerce product catalog by name, description, or category. " +
        "Input: a search term, optionally followed by \"| sort=cheapest\" or \"| sort=expensive\" " +
        "to order results by price. Example: \"shoes | sort=cheapest\".";

    public async Task<string> InvokeAsync(string input, CancellationToken cancellationToken = default)
    {
        var (term, sort) = ParseInput(input);

        var products = await miniCommerceClient.GetProductsAsync(cancellationToken);

        var matches = string.IsNullOrWhiteSpace(term)
            ? products
            : products.Where(p =>
                p.ProductName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                p.Description.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                p.CategoryName.Contains(term, StringComparison.OrdinalIgnoreCase));

        matches = sort switch
        {
            "cheapest" => matches.OrderBy(p => p.Price),
            "expensive" => matches.OrderByDescending(p => p.Price),
            _ => matches
        };

        // Return text the model can read back.
        return JsonSerializer.Serialize(matches.ToList());
    }

    // Splits "shoes | sort=cheapest" into ("shoes", "cheapest").
    private static (string Term, string Sort) ParseInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return (string.Empty, string.Empty);
        }

        var parts = input.Split('|', 2);
        var term = parts[0].Trim();

        var sort = string.Empty;
        if (parts.Length == 2)
        {
            var option = parts[1].Trim();
            if (option.StartsWith("sort=", StringComparison.OrdinalIgnoreCase))
            {
                sort = option["sort=".Length..].Trim().ToLowerInvariant();
            }
        }

        return (term, sort);
    }
}
