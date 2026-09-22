using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniCommerce.Web.Models.Checkout;
using MiniCommerce.Web.Models.Orders;
using MiniCommerce.Web.Services.Orders;
using System.IdentityModel.Tokens.Jwt;

namespace MiniCommerce.Web.Pages.Checkout;

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
        var userGuid = Guid.Empty;
        var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.NameId);
        if (userIdClaim != null)
        {
            Guid.TryParse(userIdClaim.Value, out userGuid);
        }

        var order = new OrderModel
        {
            OrderGuid = Guid.NewGuid(),
            UserGuid = userGuid,
            ProductGuid = model.ProductGuid,
            PurchasedPrice = model.PurchasedPrice,
            Quantity = model.Quantity,
            TotalPrice = model.PurchasedPrice * model.Quantity,
            CreatedAt = DateTime.UtcNow,
        };

        await orderService.CreateOrder(order, cancellationToken);
        return RedirectToPage("/Checkout/Index", routeValues: model);
    }
}
