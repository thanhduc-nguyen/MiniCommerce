using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniCommerce.Web.Models.Catalog;
using MiniCommerce.Web.Services.Catalog;

namespace MiniCommerce.Web.Pages.Catalog
{
    public class ListModel(ICatalogService catalogService) : PageModel
    {
        public IEnumerable<ProductModel> Products { get; set; } = [];
        public string? CurrentCategory { get; set; }

        public async Task OnGet(CancellationToken cancellationToken, string categoryName = "", int pageNumber = 1, int pageSize = 10)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                Products = await catalogService.GetProducts(cancellationToken);
            }
            else
            {
                Products = await catalogService.GetProductsByCategory(categoryName, cancellationToken);
                CurrentCategory = categoryName;
            }
        }
    }
}
