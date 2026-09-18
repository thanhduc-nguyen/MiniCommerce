namespace StoreFront.Web.Models.Checkout;

public class CheckoutModel
{
    public Guid ProductGuid { get; set; }
    public decimal PurchasedPrice { get; set; } // Price at the time of purchase (price after discount)
    public int Quantity { get; set; }
}
