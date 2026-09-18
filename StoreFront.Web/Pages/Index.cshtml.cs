using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StoreFront.Web.Models.Catalog;
using StoreFront.Web.Services.Catalog;

namespace StoreFront.Web.Pages;

public class IndexModel(ICatalogService catalogService) : PageModel
{
    public string Slogan { get; set; } = string.Empty;

    public IEnumerable<ProductModel> ProductsOfTheWeek { get; set; } = [];

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        var result = await catalogService.GetProducts(cancellationToken);

        ProductsOfTheWeek = result.TakeRandom(4);
    }

    public override async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context,
                                              PageHandlerExecutionDelegate next)
    {
        Slogan = "I'm feeling lucky!";
        await next.Invoke();
    }
}
