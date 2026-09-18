namespace Orders.Api.Models;

public class Order
{
    public int OrderId { get; set; }
    public Guid OrderGuid { get; set; }
    public Guid ProductGuid { get; set; }
    public decimal PurchasedPrice { get; set; } // Price at the time of purchase (price after discount)
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
}
