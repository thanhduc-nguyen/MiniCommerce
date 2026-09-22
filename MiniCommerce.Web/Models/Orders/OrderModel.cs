namespace MiniCommerce.Web.Models.Orders;

public class OrderModel
{
    public Guid OrderGuid { get; set; }
    public Guid UserGuid { get; set; }
    public Guid ProductGuid { get; set; }
    public decimal PurchasedPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
}
