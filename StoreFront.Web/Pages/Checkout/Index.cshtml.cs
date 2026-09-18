using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StoreFront.Web.Models.Checkout;
using StoreFront.Web.Services.Orders;

namespace StoreFront.Web.Pages.Checkout;

//[Authorize(Roles = "Customer")]
public class IndexModel(IOrderService orderService) : PageModel
{
    public string CheckoutResult { get; set; } = string.Empty;

    public void OnGet(CheckoutModel model)
    {
        CheckoutResult = $"Checked out {model.Quantity} of product {model.ProductGuid}";
    }

    public async Task<IActionResult> OnPostCheckout(CheckoutModel model, CancellationToken cancellationToken)
    {
        await orderService.CreateOrder(model, cancellationToken);
        return RedirectToPage("/Checkout/Index", routeValues: model);
    }
}
